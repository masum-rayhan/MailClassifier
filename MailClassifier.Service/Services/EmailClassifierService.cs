using MailClassifier.Service.Dtos;
using MailClassifier.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MailClassifier.Service.Services;

public class EmailClassifierService(IConfiguration configuration, IHttpClientFactory httpClient) : IEmailClassifierService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpClientFactory _httpClient = httpClient;

    public async Task<GeneralResponse> ClassifyEmailsAsync(List<string> messages)
    {
        var response = new GeneralResponse();

        var apiKey = _configuration["OpenRouter:ApiKey"];
        var apiUrl = _configuration["OpenRouter:ApiUrl"];

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiUrl))
        {
            response.IsSuccess = false;
            response.Message = "API key or URL is not configured.";
            return response;
        }

        var results = new List<MailClassificationResult>();
        foreach (var message in messages)
        {
            var tags = await GetTags(message, apiKey, apiUrl);

            results.Add(new MailClassificationResult
            {
                Message = message,
                Tags = tags
            });
        }

        response.IsSuccess = true;
        response.Data = results;
        response.Message = "Classification completed successfully.";

        return response;
    }

    private async Task<List<string>> GetTags(string message, string apiKey, string apiUrl)
    {
        var client = _httpClient.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var prompt = $@"You are an assistant. Classify this customer message into one or more of: 
                        Bug Report, Billing Issue, Praise, Complaint, Feature Request, Technical Support, Sales Inquiry, Security Concern, Spam/Irrelevant, Refund Request, Shipping/Delivery, Other
                        Message: 
                        ""{message}""
                        Response format: {{""tags"": [""...""]}}";

        var body = new
        {
            model = "mistralai/mixtral-8x7b-instruct",
            messages = new[] { new { role = "user", content = prompt } }
        };

        var response = await client.PostAsync(apiUrl, new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));

        var responseContent = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(responseContent);

        var content = document.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

        try
        {
            using var parsedDocument = JsonDocument.Parse(content);
            return parsedDocument.RootElement.GetProperty("tags")
                                 .EnumerateArray()
                                 .Select(x => x.GetString() ?? "")
                                 .Where(x => !string.IsNullOrEmpty(x))
                                 .ToList();
        }
        catch
        {
            return new List<string> { "Other" };
        }
    }
}
