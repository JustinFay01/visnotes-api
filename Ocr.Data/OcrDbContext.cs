using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Ocr.Data.Models;

namespace Ocr.Data;

public class OcrDbContext : DbContext
{
    public OcrDbContext(DbContextOptions<OcrDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///     The stored Notes that either have been or will be analyzed.
    /// </summary>
    public DbSet<Note> Notes { get; set; }

    /// <summary>
    ///     The successful analyses of any given Note.
    /// </summary>
    public DbSet<Analysis> Analyses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    /// <summary>
    /// This is needed for the dot net ef migrations to work
    /// </summary>
    public class OcrDbContextFactory : IDesignTimeDbContextFactory<OcrDbContext>
    {
        public OcrDbContext CreateDbContext(string[] args)
        {
            const string connectionString = "Host=localhost;Database=ocr;Username=admin;Password=admin";
            
            var optionsBuilder = new DbContextOptionsBuilder<OcrDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new OcrDbContext(optionsBuilder.Options);
        }
    }
}