using Azure.AI.TextAnalytics;

namespace MinimalApi
{
    public interface IAzureAnalyticsService
    {
        Task<TextAnalyticsClient> BuildClient();
        Task<TextAnalyticsClient> BuildClient(string ANALITICS_KEY, string ANALITICS_ENDPOINT);
        Task<string> EntityRecognition(string text);
        Task<List<CategorizedData>> EntityRecognitionObject(string text);
        Task<string> KeyPhraseExtraction(string text);
        Task<Response<KeyPhraseCollection>> KeyPhraseExtractionObject(string text);
        Task<string> LanguageDetection(string text);
        Task<DetectedLanguage> LanguageDetectionObject(string text);
        Task<string> SentimentAnalysisWithOpinionMining(string text);
        Task<AnalyzeSentimentResultCollection> SentimentAnalysisWithOpinionMiningObject(string text);
        Task<string> TextSummarization(string text);
    }
}
