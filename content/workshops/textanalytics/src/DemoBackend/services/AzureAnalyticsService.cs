namespace MinimalApi;

public class AzureAnalyticsService : IAzureAnalyticsService
{
    public TextAnalyticsClient AnalyticsClient { get; set; }
    private IConfiguration Config { get; }
    private AzureKeyCredential? Credentials { get; set; }
    private Uri? Endpoint { get; set; }
    private StringBuilder Results { get; set; }

    public AzureAnalyticsService(IConfiguration config)
    {
        Results = new StringBuilder();
        Config = config;
        AnalyticsClient = BuildClient().Result;
    }

    public async Task<TextAnalyticsClient> BuildClient()
    {
        Credentials = new AzureKeyCredential(Config["ANALITICS_KEY"]);
        Endpoint = new Uri(Config["ANALITICS_ENDPOINT"]);
        return await Task.FromResult(new TextAnalyticsClient(Endpoint, Credentials));
    }

    public async Task<TextAnalyticsClient> BuildClient(string ANALITICS_KEY, string ANALITICS_ENDPOINT)
    {
        Credentials = new AzureKeyCredential(ANALITICS_KEY);
        Endpoint = new Uri(ANALITICS_ENDPOINT);
        return await Task.FromResult(new TextAnalyticsClient(Endpoint, Credentials));
    }

    public async Task<string> EntityRecognition(string text)
    {
        Results = new StringBuilder();
        Response<CategorizedEntityCollection> response = await AnalyticsClient.RecognizeEntitiesAsync(text);

        // Write named entities.
        Results.AppendLine("Named Entities:");

        foreach (var entity in response.Value)
        {
            Results.AppendLine($"\tText: {entity.Text},\tCategory: {entity.Category},\tSub-Category: {entity.SubCategory}");
            Results.AppendLine($"\t\tScore: {entity.ConfidenceScore:F2},\tLength: {entity.Length},\tOffset: {entity.Offset}\n");
        }

        return Results.ToString();
    }

    public async Task<List<CategorizedData>> EntityRecognitionObject(string text)
    {
        var result = await AnalyticsClient.RecognizeEntitiesAsync(text);
        var outValue = new List<CategorizedData>();
        foreach (var item in result.Value)
        {
            outValue.Add(new CategorizedData
            {
                Text = item.Text,
                Category = item.Category.ToString(),
                SubCategory = item.SubCategory,
                ConfidenceScore = item.ConfidenceScore,
                Offset = item.Offset,
                Length = item.Length,

            });
        }

        return outValue;
    }

    public async Task<string> KeyPhraseExtraction(string text)
    {
        Results = new StringBuilder();
        Response<KeyPhraseCollection> response = await AnalyticsClient.ExtractKeyPhrasesAsync(text);

        // Write key phrases.
        Results.AppendLine("Key phrases:");

        foreach (string keyphrase in response.Value)
        {
            Results.AppendLine($"\t{keyphrase}");
        }

        return Results.ToString();
    }

    public async Task<Response<KeyPhraseCollection>> KeyPhraseExtractionObject(string text)
    {
        return await AnalyticsClient.ExtractKeyPhrasesAsync(text);
    }

    public async Task<string> LanguageDetection(string text)
    {
        Results = new StringBuilder();
        DetectedLanguage detectedLanguage = await AnalyticsClient.DetectLanguageAsync(text);

        // Write language.
        Results.AppendLine("Language:");

        Results.AppendLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");

        return Results.ToString();
    }

    public async Task<DetectedLanguage> LanguageDetectionObject(string text)
    {
        return await AnalyticsClient.DetectLanguageAsync(text);
    }

    public async Task<string> SentimentAnalysisWithOpinionMining(string text)
    {
        Results = new StringBuilder();

        // Create document to process.
        var documents = new List<string> { text };

        AnalyzeSentimentResultCollection reviews = await AnalyticsClient.AnalyzeSentimentBatchAsync(documents, options: new AnalyzeSentimentOptions()
        {
            IncludeOpinionMining = true
        });

        foreach (AnalyzeSentimentResult review in reviews)
        {
            Results.AppendLine($"Document sentiment: {review.DocumentSentiment.Sentiment}\n");
            Results.AppendLine($"\tPositive score: {review.DocumentSentiment.ConfidenceScores.Positive:0.00}");
            Results.AppendLine($"\tNegative score: {review.DocumentSentiment.ConfidenceScores.Negative:0.00}");
            Results.AppendLine($"\tNeutral score: {review.DocumentSentiment.ConfidenceScores.Neutral:0.00}\n");
            foreach (SentenceSentiment sentence in review.DocumentSentiment.Sentences)
            {
                Results.AppendLine($"\tText: \"{sentence.Text}\"");
                Results.AppendLine($"\tSentence sentiment: {sentence.Sentiment}");
                Results.AppendLine($"\tSentence positive score: {sentence.ConfidenceScores.Positive:0.00}");
                Results.AppendLine($"\tSentence negative score: {sentence.ConfidenceScores.Negative:0.00}");
                Results.AppendLine($"\tSentence neutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");

                foreach (SentenceOpinion sentenceOpinion in sentence.Opinions)
                {
                    Results.AppendLine($"\tTarget: {sentenceOpinion.Target.Text}, Value: {sentenceOpinion.Target.Sentiment}");
                    Results.AppendLine($"\tTarget positive score: {sentenceOpinion.Target.ConfidenceScores.Positive:0.00}");
                    Results.AppendLine($"\tTarget negative score: {sentenceOpinion.Target.ConfidenceScores.Negative:0.00}");
                    foreach (AssessmentSentiment assessment in sentenceOpinion.Assessments)
                    {
                        Results.AppendLine($"\t\tRelated Assessment: {assessment.Text}, Value: {assessment.Sentiment}");
                        Results.AppendLine($"\t\tRelated Assessment positive score: {assessment.ConfidenceScores.Positive:0.00}");
                        Results.AppendLine($"\t\tRelated Assessment negative score: {assessment.ConfidenceScores.Negative:0.00}");
                    }
                }
            }
            Results.AppendLine();
        }

        return Results.ToString();
    }

    public async Task<AnalyzeSentimentResultCollection> SentimentAnalysisWithOpinionMiningObject(string text)
    {

        return await AnalyticsClient.AnalyzeSentimentBatchAsync
        (
            new List<string> { text },
            options: new AnalyzeSentimentOptions() { IncludeOpinionMining = true }
        );
    }

    public async Task<string> TextSummarization(string text)
    {
        Results = new StringBuilder();

        var batchInput = new List<string>
            {
                text
            };

        TextAnalyticsActions actions = new()
        {
            ExtractSummaryActions = new List<ExtractSummaryAction>() { new ExtractSummaryAction() }
        };

        // Start analysis process.
        AnalyzeActionsOperation operation = await AnalyticsClient.StartAnalyzeActionsAsync(batchInput, actions);
        await operation.WaitForCompletionAsync();

        // View operation status.
        Results.AppendLine($"AnalyzeActions operation has completed");
        Results.AppendLine();

        Results.AppendLine($"Created On   : {operation.CreatedOn}");
        Results.AppendLine($"Expires On   : {operation.ExpiresOn}");
        Results.AppendLine($"Id           : {operation.Id}");
        Results.AppendLine($"Status       : {operation.Status}");

        Results.AppendLine();
        // View operation results.
        await foreach (AnalyzeActionsResult documentsInPage in operation.Value)
        {
            IReadOnlyCollection<ExtractSummaryActionResult> summaryResults = documentsInPage.ExtractSummaryResults;

            foreach (ExtractSummaryActionResult summaryActionResults in summaryResults)
            {
                if (summaryActionResults.HasError)
                {
                    Results.AppendLine($"  Error!");
                    Results.AppendLine($"  Action error code: {summaryActionResults.Error.ErrorCode}.");
                    Results.AppendLine($"  Message: {summaryActionResults.Error.Message}");
                    continue;
                }

                foreach (ExtractSummaryResult documentResults in summaryActionResults.DocumentsResults)
                {
                    if (documentResults.HasError)
                    {
                        Results.AppendLine($"  Error!");
                        Results.AppendLine($"  Document error code: {documentResults.Error.ErrorCode}.");
                        Results.AppendLine($"  Message: {documentResults.Error.Message}");
                        continue;
                    }

                    Results.AppendLine($"  Extracted the following {documentResults.Sentences.Count} sentence(s):");
                    Results.AppendLine();

                    foreach (SummarySentence sentence in documentResults.Sentences)
                    {
                        Results.AppendLine($"  Sentence: {sentence.Text}");
                        Results.AppendLine();
                    }
                }
            }
        }

        return Results.ToString();
    }

}
