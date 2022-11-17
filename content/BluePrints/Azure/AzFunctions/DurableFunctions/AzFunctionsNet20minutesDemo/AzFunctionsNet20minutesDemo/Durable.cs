using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace AzFunctionsNet20minutesDemo
{
    public static class Durable
    {
        #region Methods

        private static List<int> GetOptionsList() => System.Enum.GetValues(typeof(Functions)).Cast<int>().ToList();

        #endregion

        #region Enums

        public enum Functions
        {
            Chaining = 0,
            FanOutFanIn = 1
        } 

        #endregion

        #region Chaining Pattern

        [FunctionName(nameof(Chaining))]
        public static async Task<List<string>> Chaining([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            var paralllelTask = new List<Task<int>>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            System.Threading.Thread.Sleep(2000);
            return $"Hello {name}!";
        }
        #endregion

        #region FanOutFanIn Pattern

        [FunctionName(nameof(FanOutFanIn))]
        public static async Task<FanInFanOutSummary> FanOutFanIn([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var parallelTasks = new List<Task<FanInFanOutResult>>();

            // Get a list of N work items to process in parallel.
            var workBatch = await context.CallActivityAsync<List<int>>(nameof(GetNumbers), 3);
            workBatch.ForEach(t =>
            {
                Task<FanInFanOutResult> task = context.CallActivityAsync<FanInFanOutResult>(nameof(Factorial), t);
                parallelTasks.Add(task);
            });

            await Task.WhenAll(parallelTasks);
            var result = new FanInFanOutSummary();           
            int sum = parallelTasks.Sum(t => t.Result.Factorial);
            result.Total = sum;
            result.Items = new List<FanInFanOutResult>();
            var index = 0;
            parallelTasks.ForEach(t =>
            {  
                
                var data = t.Result;
                data.Index = index;
                result.Items.Add(data);
                index++;
            });
            await context.CallActivityAsync(nameof(WriteSumFactorial), sum);

            return result;
        }

        [FunctionName(nameof(GetNumbers))]
        public static List<int> GetNumbers([ActivityTrigger] int number, ILogger log)
        {
            log.LogInformation($"Generate the list from 1 to {number}.");
            System.Threading.Thread.Sleep(1000);
            return Enumerable.Range(1, number).ToList();
        }

        [FunctionName(nameof(Factorial))]
        public static FanInFanOutResult Factorial([ActivityTrigger] int number, ILogger log)
        {
            var fact = 1;
            var r = Enumerable.Range(1, number).ToList();
            for (int j = 1; j <= number; j++)
                fact = fact * j;

            log.LogInformation($"Factorial number of {number} is {fact}.");

            System.Threading.Thread.Sleep(2000);
            return new FanInFanOutResult {Index = 0, Number = number, Factorial = fact};
        }

        [FunctionName(nameof(WriteSumFactorial))]
        public static string WriteSumFactorial([ActivityTrigger] int sum, ILogger log)
        {
            var message = $"The sum of the factorial numbers is {sum}.";
            log.LogInformation(message);
            System.Threading.Thread.Sleep(2000);
            return message;
        }

        #endregion

        #region Orchestation Client

        [FunctionName("Durable_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "{option}")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var allowedoptions = GetOptionsList();
            _ = int.TryParse(req.RequestUri.Segments[2], out int option);

            if (!allowedoptions.Contains(option))
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.NotFound };

            string instanceId = await starter.StartNewAsync(((Functions)option).ToString(), null);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
        #endregion

    }

    #region Auxiliary Classes

    public class FanInFanOutResult
    {
        public int Index { get; set; }
        public int Number { get; set; }
        public int Factorial { get; set; }
    }

    public class FanInFanOutSummary
    {
        public int Total { get; set; }
        public List<FanInFanOutResult> Items { get; set; }
    } 

    #endregion

}