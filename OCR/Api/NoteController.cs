using Microsoft.AspNetCore.Mvc;
using OCR.Infrastructure;
using OCR.Infrastructure.SystemStorage;
using OCR.Services;
using OCR.Services.DTOs;

namespace OCR.Api;

[ApiController]
[Route("api/notes")]
public class NoteController : ControllerBase
{
    
    private readonly ILogger<NoteController> _logger;
    private readonly INoteService _noteService;
    private readonly IFileSystemStorage _fileSystemStorage;
    private readonly IDocumentIntelligenceService _documentIntelligenceService;
    
    
    public NoteController(ILogger<NoteController> logger, INoteService noteService, IFileSystemStorage fileSystemStorage, IDocumentIntelligenceService documentIntelligenceService)
    {
        _logger = logger;
        _noteService = noteService;
        _fileSystemStorage = fileSystemStorage;
        _documentIntelligenceService = documentIntelligenceService;
    }
    
    // Notes
    
    [HttpGet]
    public async Task<IActionResult> GetNotes()
    {
        _logger.LogInformation("Call received to get all notes at {time}", DateTime.Now);
        
        var notes = await _noteService.GetNotesAsync();
        
        return Ok(notes);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] IFormFile note)
    {
        var createdNote = await _noteService.CreateNoteAsync(note);
        return Ok(createdNote);
    }
    
    // Notes/{id}
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById(Guid id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);
        
        return Ok(note);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNoteById(Guid id)
    {
        await _noteService.DeleteNoteByIdAsync(id);
        
        return NoContent();
    }
    
    [HttpPost("{id}")]
    public async Task<IActionResult> AnalyzeNoteById(Guid id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);
        
        var file = await _fileSystemStorage.GetFileAsync(note.Path);
        
        var analyses = await _documentIntelligenceService.AnalyzeFileAsync(file);
        return Ok(analyses);
    }
    
    // Notes/{id}/analyses
    
    [HttpGet("{id}/analyses")]
    public async Task<IActionResult> GetAnalysesByNoteId(Guid id)
    {
        var analyses = await _noteService.GetAnalysesByNoteIdAsync(id);
        
        return Ok(analyses);
    }
    
    [HttpPost("{id}/analyses")]
    public async Task<IActionResult> CreateAnalysisByNoteId(Guid id, [FromBody] AnalysisDto analysis)
    {
        //var createdAnalysis = await _noteService.CreateAnalysisByNoteIdAsync(id, analysis);
        
        //return Ok(createdAnalysis);
        throw new NotImplementedException();
    }
}