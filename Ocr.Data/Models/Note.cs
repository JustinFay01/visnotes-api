using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ocr.Data.Models.Abstract;

namespace Ocr.Data.Models;

public class Note : BaseEntity
{
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

    public ICollection<Analysis>? Analyses { get; set; }
}

public class NoteConfiguration : BaseEntityConfiguration<Note>
{
    public override void Configure(EntityTypeBuilder<Note> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Size)
            .IsRequired();

        builder.Property(c => c.Type)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Path)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}