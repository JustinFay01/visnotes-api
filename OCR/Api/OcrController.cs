using Microsoft.AspNetCore.Mvc;
using OCR.Services;

namespace OCR.Api;

[ApiController]
[Route("/api/ocr")]
public class OcrController : ControllerBase
{
    private readonly IVisionService _visionService;
    private readonly ILogger<OcrController> _logger;

    public OcrController(IVisionService visionService, ILogger<OcrController> logger)
    {
        _visionService = visionService;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> ReadFile([FromBody] byte[] file)
    {
        _logger.LogInformation("Reading file");
        var result = await _visionService.ReadFileFromBytesAsync(file);
        return Ok();
    }
    
}