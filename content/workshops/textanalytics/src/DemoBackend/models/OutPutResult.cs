public class OutPutResult
{
    public TweetV2? Tweet { get; internal set; }
    public Response<KeyPhraseCollection>? KeyFrasess { get; internal set; }
    public DetectedLanguage Language { get; internal set; }
    public AnalyzeSentimentResultCollection? Sentiment { get; internal set; }
    public List<CategorizedData>? Entities { get; internal set; }
}