using Microsoft.EntityFrameworkCore;

namespace OCR.Data;

public class OcrDbContext : DbContext
{
    public OcrDbContext(DbContextOptions<OcrDbContext> options) : base(options)
    {
    }
}