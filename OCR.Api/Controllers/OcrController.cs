using Microsoft.AspNetCore.Mvc;
using Ocr.Services;

namespace OCR.Controllers;

[ApiController]
[Route("/api/ocr")]
public class OcrController : ControllerBase
{
    private readonly ILogger<OcrController> _logger;
    private readonly IVisionService _visionService;

    public OcrController(IVisionService visionService, ILogger<OcrController> logger)
    {
        _visionService = visionService;
        _logger = logger;
    }


    /// <summary>
    ///     Upload a file to be read.
    ///     <see href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore#handle-forms-and-file-uploads">
    ///         Handle Forms
    ///         and File Uploads
    ///     </see>
    ///     for swagger information on how to handle IFormFiles within post.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/upload")]
    public async Task<IActionResult> ReadFile(IFormFile file)
    {
        //TODO: Validate file is an Image
        var result = await _visionService.ReadFileFromBytesAsync(file);
        return new OkObjectResult(result);
    }
}