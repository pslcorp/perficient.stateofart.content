using System.Text.Json;
using System.Text.Json.Serialization;

public class ProcessorData : IProcessorData
{
    #region propierties
    private readonly IAzureAnalyticsService _azureAnalytics;
    private readonly ITwitterService _twitterService;
    #endregion

    public ProcessorData([NotNull] IAzureAnalyticsService azureAnalytics, [NotNull] ITwitterService twitterService)
    {
        _azureAnalytics = azureAnalytics;
        _twitterService = twitterService;
    }

    public async Task<OutPutResult> ProcessData([NotNull] long id)
    {
        //Inictal values
        var result = new OutPutResult();
        result.Tweet = await _twitterService.GetTweetById(id);
        
        //Process text async
        var keyFrasess = _azureAnalytics.KeyPhraseExtractionObject(result.Tweet.Text);
        var entities = _azureAnalytics.EntityRecognitionObject(result.Tweet.Text);
        var language = _azureAnalytics.LanguageDetectionObject(result.Tweet.Text);
        var sentiment = _azureAnalytics.SentimentAnalysisWithOpinionMiningObject(result.Tweet.Text);
        await Task.WhenAll(keyFrasess, entities, language, sentiment);

        //assingn values to propierties
        result.KeyFrasess = await keyFrasess;
        result.Entities = await entities;
        result.Language = await language;
        result.Sentiment = await sentiment;

        return result;
    }
}