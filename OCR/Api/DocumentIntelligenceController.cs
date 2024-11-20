using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCR.Services;

namespace OCR.Api
{
    //TODO: Switch to a pipeline pattern and use one controller that handles all the different services
    [ApiController]
    [Route("api/di")]
    public class DocumentIntelligenceController : ControllerBase
    {
        private readonly IDocumentIntelligenceService _documentIntelligenceService;

        public DocumentIntelligenceController(IDocumentIntelligenceService documentIntelligenceService)
        {
            _documentIntelligenceService = documentIntelligenceService;
        }
        
        [HttpPost]
        public async Task<IActionResult> AnalyzeDocument(IFormFile file)
        {
            var result = await _documentIntelligenceService.AnalyzeFileAsync(file);
            return new OkObjectResult(result);
        }
    }
}
