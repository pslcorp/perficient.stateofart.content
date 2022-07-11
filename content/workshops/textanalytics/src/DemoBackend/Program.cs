global using Microsoft.OpenApi.Models;
global using MinimalApi;
global using Azure;
global using Azure.AI.TextAnalytics;
global using System.Text;
global using Tweetinvi.Models.V2;
global using Tweetinvi;
global using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<ITwitterService, TwitterService>();
builder.Services.AddTransient<IAzureAnalyticsService, AzureAnalyticsService>();
builder.Services.AddTransient<IProcessorData, ProcessorData>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo TextAnalytics Api", Version = "v1" });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));

app.MapGet("/tweet/{id}", async (long id, ITwitterService twitterService) =>
{
    return await twitterService.GetTweetById(id);
});

app.MapPost("/textanalytics/entities", async (string text,  IAzureAnalyticsService analyticsService) =>
{
    return await analyticsService.EntityRecognition(text);
});

app.MapPost("/textanalytics/keyphrases", async (string text, IAzureAnalyticsService analyticsService) =>
{
    return await analyticsService.KeyPhraseExtraction(text);
});

app.MapPost("/textanalytics/language", async (string text, IAzureAnalyticsService analyticsService) =>
{
    return await analyticsService.LanguageDetection(text);
});

app.MapPost("/textanalytics/sentiments", async (string text, IAzureAnalyticsService analyticsService) =>
{
    return await analyticsService.SentimentAnalysisWithOpinionMining(text);
});

app.MapPost("/textanalytics/summarization", async (string text, IAzureAnalyticsService analyticsService) =>
{
    return await analyticsService.TextSummarization(text);
});

app.MapPost("/textanalytics/demo", async (long id, ITwitterService twitterService, IAzureAnalyticsService analyticsService) =>
{
    StringBuilder Results = new StringBuilder();
    TweetV2 tweet = await twitterService.GetTweetById(id);
    Results.AppendLine("Twitter text: ");
    Results.AppendLine(tweet.Text);
    Results.AppendLine(await analyticsService.EntityRecognition(tweet.Text));
    Results.AppendLine(await analyticsService.KeyPhraseExtraction(tweet.Text));
    Results.AppendLine(await analyticsService.LanguageDetection(tweet.Text));
    Results.AppendLine(await analyticsService.SentimentAnalysisWithOpinionMining(tweet.Text));
    Results.AppendLine(await analyticsService.TextSummarization(tweet.Text));

    return Results.ToString();
});

app.MapPost("/textanalytics/demotext", async (string text, IAzureAnalyticsService analyticsService) =>
{
    StringBuilder Results = new StringBuilder();
    
    Results.AppendLine("Text input : ");
    Results.AppendLine(text);
    Results.AppendLine(await analyticsService.EntityRecognition(text));
    Results.AppendLine(await analyticsService.KeyPhraseExtraction(text));
    Results.AppendLine(await analyticsService.LanguageDetection(text));
    Results.AppendLine(await analyticsService.SentimentAnalysisWithOpinionMining(text));
    Results.AppendLine(await analyticsService.TextSummarization(text));

    return Results.ToString();
});

app.MapGet("/textanalytics/demofront", (long id, IProcessorData processor) =>
{    
    return processor.ProcessData(id);
});

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

app.Run();