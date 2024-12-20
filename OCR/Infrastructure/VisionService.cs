using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace OCR.Infrastructure;

public interface IVisionService
{
    /// <summary>
    /// Reads a file from bytes and returns the text
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<List<string>> ReadFileFromBytesAsync(IFormFile file);
    Task<List<string>> ReadFileFromUrlAsync(string url);
}


public class VisionService: IVisionService
{
    private string _visionKey;
    private string _visionEndpoint;
    private readonly ILogger<VisionService> _logger;
    private ComputerVisionClient _client;
    
    public VisionService(string visionKey, string visionEndpoint, ILogger<VisionService> logger)
    {
        _visionEndpoint = visionEndpoint;
        _visionKey = visionKey;
        _logger = logger;
        _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(visionKey))
        {
            Endpoint = visionEndpoint
        };
    }

    public async Task<List<string>> ReadFileFromBytesAsync(IFormFile file)
    {
        _logger.LogInformation("Reading file from bytes");

        var stream = file.OpenReadStream();
        var result = 
            await _client.RecognizePrintedTextInStreamAsync(false, stream,
            OcrLanguages.En);

        return result.Regions.SelectMany(region => region.Lines)
            .SelectMany(line => line.Words)
            .Select(word => word.Text)
            .ToList();
    }

    public  Task<List<string>> ReadFileFromUrlAsync(string url)
    {
        _logger.LogInformation("Reading file from url");
        throw new NotImplementedException();
    }
}