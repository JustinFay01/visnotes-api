namespace Ocr.Services.Dtos;

public class NoteDto
{
    public Guid Id { get; set; }

    /// <summary>
    ///     Name of the note.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Size of the note in bytes.
    /// </summary>
    public required double Size { get; set; }

    /// <summary>
    ///     MIME type of the note.
    /// </summary>
    public required string Type { get; set; }

    public required string Path { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<AnalysisDto>? Analyses { get; set; }
}