using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using OCR.Services.Extensions;

namespace OCR.Infrastructure;

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

        var words = GetLinesFromPages(result.Value.Pages)
            .SplitBySpace()
            .FilterSpecialCharacters()
            .ToList();
        
        return words;
    }
    
    private static IEnumerable<string> GetLinesFromPages(IReadOnlyList<DocumentPage> pages)
    {
        var lines = pages.SelectMany(page => page.Lines)
            .Select(line => line.Content);
        return lines;
    }
}