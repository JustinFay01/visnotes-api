using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace OCR.Services;

/// <summary>
/// Check out:
/// <see href="https://learn.microsoft.com/en-us/azure/ai-services/document-intelligence/how-to-guides/use-sdk-rest-api?view=doc-intel-4.0.0&tabs=windows&pivots=programming-language-csharp">
/// Document Intelligence
/// </see>
/// </summary>
public interface IDocumentIntelligenceService
{
    Task<List<string>> AnalyzeFileAsync(IFormFile file);
}

public class DocumentIntelligenceService : IDocumentIntelligenceService
{
    private readonly ILogger<DocumentIntelligenceService> _logger;
    
    private DocumentAnalysisClient _client;
    public DocumentIntelligenceService(string diKey, string diEndpoint, ILogger<DocumentIntelligenceService> logger)
    {
        _logger = logger;
        
        _client = new DocumentAnalysisClient(new Uri(diEndpoint), new AzureKeyCredential(diKey));
    }
    
    public async Task<List<string>> AnalyzeFileAsync(IFormFile file)
    {
        _logger.LogInformation("Analyzing document");

        var result = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", file.OpenReadStream());
        
        return result.Value.Pages.SelectMany(page => page.Lines)
            .Select(line => line.Content)
            .ToList();
    }
    
}