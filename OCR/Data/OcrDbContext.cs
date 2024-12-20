using Microsoft.EntityFrameworkCore;
using OCR.Data.Models;

namespace OCR.Data;

public class OcrDbContext : DbContext
{
    
    public DbSet<Note> Notes { get; set; }
    
    public OcrDbContext(DbContextOptions<OcrDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}