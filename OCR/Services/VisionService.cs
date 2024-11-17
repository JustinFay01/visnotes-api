using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace OCR.Services;

public interface IVisionService
{
    /// <summary>
    /// Reads a file from bytes and returns the text
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<List<string>> ReadFileFromBytesAsync(byte[] file);
    Task<List<string>> ReadFileFromUrlAsync(string url);
}


public class VisionService: IVisionService
{
    private string _visionKey;
    private string _visionEndpoint;
    private readonly ILogger<VisionService> _logger;
    private ComputerVisionClient _client;
    
    public VisionService(string visionEndpoint, string visionKey, ILogger<VisionService> logger)
    {
        _visionEndpoint = visionEndpoint;
        _visionKey = visionKey;
        _logger = logger;
        _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(visionKey))
        {
            Endpoint = visionEndpoint
        };
    }

    public async Task<List<string>> ReadFileFromBytesAsync(byte[] file)
    {
        _logger.LogInformation("Reading file from bytes");

        using var stream = new MemoryStream(file);
        var result = await _client.RecognizePrintedTextInStreamAsync(true, stream,
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