using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ocr.Data;
using Ocr.Data.Models;
using Ocr.Services.Dtos;
using Ocr.Services.Exceptions;
using Ocr.Services.SystemStorage;

namespace Ocr.Services;

public interface INoteService
{
    public Task<IEnumerable<NoteDto>> GetNotesAsync();

    public Task<NoteDto> GetNoteByIdAsync(Guid id);

    public Task<NoteDto> CreateNoteAsync(IFormFile note);

    public Task DeleteNoteByIdAsync(Guid id);

    public Task<IEnumerable<AnalysisDto>> GetAnalysesByNoteIdAsync(Guid id);

    public Task<AnalysisDto> CreateAnalysisByNoteIdAsync(Guid id);
}

public class NoteService : INoteService
{
    private readonly OcrDbContext _context;
    private readonly IDocumentIntelligenceService _documentIntelligenceService;
    private readonly IFileSystemStorage _fileSystemStorage;
    private readonly ILogger<NoteService> _logger;
    private readonly IMapper _mapper;

    public NoteService(OcrDbContext context, IFileSystemStorage fileSystemStorage, IMapper mapper,
        IDocumentIntelligenceService documentIntelligenceService, ILogger<NoteService> logger)
    {
        _context = context;
        _fileSystemStorage = fileSystemStorage;
        _mapper = mapper;
        _documentIntelligenceService = documentIntelligenceService;
        _logger = logger;
    }

    public async Task<IEnumerable<NoteDto>> GetNotesAsync()
    {
        var notes = await _context.Notes
            .Include(n => n.Analyses)
            .ToListAsync();
        var mappedNotes = _mapper.Map<IEnumerable<NoteDto>>(notes);
        return mappedNotes;
    }

    public async Task<NoteDto> GetNoteByIdAsync(Guid id)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        if (note == null) throw new NoteNotFoundException();

        var mappedNote = _mapper.Map<NoteDto>(note);
        return mappedNote;
    }

    public async Task<NoteDto> CreateNoteAsync(IFormFile note)
    {
        await _fileSystemStorage.SaveFileAsync(note);

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

    public async Task DeleteNoteByIdAsync(Guid id)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        if (note == null) throw new NoteNotFoundException();

        var file = await _fileSystemStorage.GetFileAsync(note.Path);
        await _fileSystemStorage.DeleteFileAsync(file.FileName);

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AnalysisDto>> GetAnalysesByNoteIdAsync(Guid id)
    {
        var analyses = await _context.Analyses.Where(a => a.NoteId == id).ToListAsync();
        var mappedAnalyses = _mapper.Map<IEnumerable<AnalysisDto>>(analyses);
        return mappedAnalyses;
    }

    public async Task<AnalysisDto> CreateAnalysisByNoteIdAsync(Guid id)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);

        if (note == null) throw new NoteNotFoundException();

        var file = await _fileSystemStorage.GetFileAsync(note.Path);

        var analysisDto = await _documentIntelligenceService.AnalyzeFileAsync(file);
        analysisDto.NoteId = id;

        var mappedAnalysis = _mapper.Map<Analysis>(analysisDto);

        var analysisEntity = _context.Analyses.Add(mappedAnalysis);
        await _context.SaveChangesAsync();

        analysisDto.Id = analysisEntity.Entity.Id;

        return analysisDto;
    }
}