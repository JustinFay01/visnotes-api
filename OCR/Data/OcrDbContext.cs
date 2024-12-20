using Microsoft.EntityFrameworkCore;
using OCR.Data.Models;

namespace OCR.Data;

public class OcrDbContext : DbContext
{
    
    /// <summary>
    ///  The stored Notes that either have been or will be analyzed.
    /// </summary>
    public DbSet<Note> Notes { get; set; }
    
    /// <summary>
    /// The successful analyses of any given Note. 
    /// </summary>
    public DbSet<Analysis> Analyses { get; set; }
    
    
    public OcrDbContext(DbContextOptions<OcrDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}