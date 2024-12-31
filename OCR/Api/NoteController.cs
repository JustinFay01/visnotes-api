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
    
    public NoteController(ILogger<NoteController> logger, INoteService noteService)
    {
        _logger = logger;
        _noteService = noteService;
    }
    
    // Notes
    
    [HttpGet]
    public async Task<IActionResult> GetNotes()
    {
        _logger.LogInformation("Call received to get all notes at {time}", DateTime.Now);
        
        _logger.LogCritical("V0.29");
        var notes = await _noteService.GetNotesAsync();
        
        
        return Ok(notes);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateNote(IFormFile file)
    {
        var createdNote = await _noteService.CreateNoteAsync(file);
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
    
    // Notes/{id}/analyses
    
    [HttpGet("{id}/analyses")]
    public async Task<IActionResult> GetAnalysesByNoteId(Guid id)
    {
        var analyses = await _noteService.GetAnalysesByNoteIdAsync(id);
        
        return Ok(analyses);
    }
    
    [HttpPost("{id}/analyses")]
    public async Task<IActionResult> AnalyzeNoteById(Guid id)
    {
        var analysis = await _noteService.CreateAnalysisByNoteIdAsync(id);
        return Ok(analysis);
    }
}