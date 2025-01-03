namespace Ocr.Services.Dtos;

public class AnalysisDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     The raw content of the result.
    /// </summary>
    public required string RawValue { get; set; }

    public string? FilteredValue { get; set; }

    public Guid NoteId { get; set; }
}