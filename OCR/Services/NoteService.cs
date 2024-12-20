using AutoMapper;
using OCR.Data;
using OCR.Data.Models;
using OCR.Infrastructure.SystemStorage;
using OCR.Services.DTOs;

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
    private readonly Mapper _mapper;
    
    public NoteService(OcrDbContext context, IFileSystemStorage fileSystemStorage, Mapper mapper)
    {
        _context = context;
        _fileSystemStorage = fileSystemStorage;
        _mapper = mapper;
    }
    
    public Task<IEnumerable<NoteDto>> GetNotesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<NoteDto> GetNoteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<NoteDto> CreateNoteAsync(IFormFile note)
    { 
        var path = await _fileSystemStorage.SaveFileAsync(note);
        
        var noteEntity = new Note
        {
            Name = note.FileName,
            Path = path, // Need from file system storage
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