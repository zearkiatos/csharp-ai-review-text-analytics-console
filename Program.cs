using System;
using Azure;
using Azure.AI.TextAnalytics;
using AIReviewTextAnalyticsConsole.Configuration;

class Program
{
    private static readonly AzureKeyCredential credentials = new AzureKeyCredential(AppConfiguration.azureKeyCredential);
    private static readonly Uri endpoint = new Uri(AppConfiguration.azureBaseUrl);

    static void Main(string[] args)
    {
        var client = new TextAnalyticsClient(endpoint, credentials);

        string document = "Tuve el mejor dia de mi vida. Realmente disfrute el concierto!";

        AnalyzeSentiment(client, document);

    }

    static void AnalyzeSentiment(TextAnalyticsClient client, string document)
    {
        DocumentSentiment documentSentiment = client.AnalyzeSentiment(document);
        Console.WriteLine("=== Sentiments Analysis ===");
        Console.WriteLine($"Document Sentiment: {documentSentiment.Sentiment}\n");

        foreach (var sentence in documentSentiment.Sentences)
        {
            Console.WriteLine($"Sentence Sentiment: {sentence.Sentiment}");
            Console.WriteLine($"Text: {sentence.Text}");
            Console.WriteLine($"Positive Score: {sentence.ConfidenceScores.Positive}");
            Console.WriteLine($"Neutral Score: {sentence.ConfidenceScores.Neutral}");
            Console.WriteLine($"Negative Score: {sentence.ConfidenceScores.Negative}\n");

            foreach (var opinion in sentence.Opinions)
            {
                Console.WriteLine($"  Target: {opinion.Target.Text}, Sentiment: {opinion.Target.Sentiment}");
                foreach (var assessment in opinion.Assessments)
                {
                    Console.WriteLine($"    Assessment: {assessment.Text}, Sentiment: {assessment.Sentiment}");
                }
            }
            Console.WriteLine();
        }
    }

}
