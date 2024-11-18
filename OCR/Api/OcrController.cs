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


    [HttpGet]
    public async Task<IActionResult> Test()
    {
        _logger.LogInformation("Test"); 
        var fileName = "test-screen-shot.png";
        var filePath = Path.Combine("C:\\Users\\justi\\code\\OCR\\assets\\", fileName);

        try
        {
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var result = await _visionService.ReadFileFromBytesAsync(fileBytes);
            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading file");
            return BadRequest();
        }
        
    }
    
    [HttpPost]
    public async Task<IActionResult> ReadFile([FromBody] byte[] file)
    {
        _logger.LogInformation("Reading file");
        var result = await _visionService.ReadFileFromBytesAsync(file);
        return Ok();
    }
    
}