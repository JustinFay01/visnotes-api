using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OCR.Data;
using OCR.Data.Models;
using OCR.Infrastructure.SystemStorage;
using OCR.Services.DTOs;
using OCR.Services.Exceptions;

namespace OCR.Services;

public interface INoteService
{
    public Task<IEnumerable<NoteDto>> GetNotesAsync();
    
    public Task<NoteDto> GetNoteByIdAsync(Guid id);
    
    public Task<NoteDto> CreateNoteAsync(IFormFile note);
    
    public Task DeleteNoteByIdAsync(Guid id);
    
    public Task<IEnumerable<Analysis>> GetAnalysesByNoteIdAsync(Guid id);
    
    public Task<Analysis> CreateAnalysisByNoteIdAsync(Guid id);
}

public class NoteService : INoteService
{
    private readonly OcrDbContext _context;
    private readonly IFileSystemStorage _fileSystemStorage;
    private readonly IMapper _mapper;
    
    public NoteService(OcrDbContext context, IFileSystemStorage fileSystemStorage, IMapper mapper)
    {
        _context = context;
        _fileSystemStorage = fileSystemStorage;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<NoteDto>> GetNotesAsync()
    {
        var notes = await _context.Notes.ToListAsync();
        var mappedNotes = _mapper.Map<IEnumerable<NoteDto>>(notes);
        return mappedNotes;
    }

    public async Task<NoteDto> GetNoteByIdAsync(Guid id)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        if (note == null)
        {
            throw new NoteNotFoundException();
        }
        
        var mappedNote = _mapper.Map<NoteDto>(note);
        return mappedNote;
    }

    public async Task<NoteDto> CreateNoteAsync(IFormFile note)
    { 
        var path = await _fileSystemStorage.SaveFileAsync(note);
        
        var noteEntity = new Note
        {
            Name = note.FileName,
            Path = note.FileName, // For now, it is just the name of the file, but in the future it will be the path (i.e. userID/filename)
            Size = note.Length,
            Type = note.ContentType,
            CreatedAt = DateTime.Now
        };
        
        _context.Notes.Add(noteEntity);
        await _context.SaveChangesAsync();
        
        var mappedNote = _mapper.Map<NoteDto>(noteEntity);
        
        return mappedNote;
    }

    public Task DeleteNoteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Analysis>> GetAnalysesByNoteIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Analysis> CreateAnalysisByNoteIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}