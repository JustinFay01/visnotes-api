using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocr.Services;

namespace Ocr.Api.Controllers;

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
    [Authorize("read:notes")]
    public async Task<IActionResult> GetNotes()
    {
        _logger.LogInformation("Call received to get all notes at {time}", DateTime.Now);
        var notes = await _noteService.GetNotesAsync();
        return Ok(notes);
    }

    [HttpPost]
    [Authorize("write:notes")]
    public async Task<IActionResult> CreateNote()
    {
        if(!Request.Form.Files.Any())
        {
            return BadRequest("No file was uploaded");
        }
        
        var file = Request.Form.Files[0];
        
        var createdNote = await _noteService.CreateNoteAsync(file);
        return Ok(createdNote);
    }

    // Notes/{id}

    [HttpGet("{id}")]
    [Authorize("read:notes")]
    public async Task<IActionResult> GetNoteById(Guid id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);

        return Ok(note);
    }

    [HttpDelete("{id}")]
    [Authorize("write:notes")]
    public async Task<IActionResult> DeleteNoteById(Guid id)
    {
        await _noteService.DeleteNoteByIdAsync(id);

        return NoContent();
    }

    // Notes/{id}/analyses
    
    [HttpGet("{id}/analyses")]
    [Authorize("read:notes")]
    public async Task<IActionResult> GetAnalysesByNoteId(Guid id)
    {
        var analyses = await _noteService.GetAnalysesByNoteIdAsync(id);

        return Ok(analyses);
    }

    [HttpPost("{id}/analyses")]
    [Authorize("write:notes")]
    public async Task<IActionResult> AnalyzeNoteById(Guid id)
    {
        var analysis = await _noteService.CreateAnalysisByNoteIdAsync(id);
        return Ok(analysis);
    }
}