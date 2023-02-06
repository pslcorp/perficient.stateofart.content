using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//example taken from https://www.geeksforgeeks.org/async-and-await-in-c-sharp/
namespace ProfilingNetAppPOC.Asyncronous
{
    public class TeachingInSchool : IShowProfiling
    {

        public static async Task StartSchoolAssembly()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(8000);
                Console.WriteLine("School Started");
            });
        }

        public static async Task TeachClass11()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("Taught class 11");
            });

        }

        public static async Task TeachClass12()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("Taught class 12");
            });


        }

        public void showProfiling()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var task1 = StartSchoolAssembly();
            var task2 = TeachClass12();
            var task3 = TeachClass11();


            Task.WaitAll(task1, task2, task3);
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
