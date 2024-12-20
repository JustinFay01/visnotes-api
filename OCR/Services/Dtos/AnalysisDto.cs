namespace OCR.Services.DTOs;

public class AnalysisDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// The raw content of the result.
    /// </summary>
    public required string RawValue { get; set; }
    
    public string? FilteredValue { get; set; }
}