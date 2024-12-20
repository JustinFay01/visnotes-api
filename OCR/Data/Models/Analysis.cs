﻿using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OCR.Data.Models.Abstract;

namespace OCR.Data.Models;

public class Analysis : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// The raw content of the result.
    /// </summary>
    public required string RawValue { get; set; }
    public required Guid NoteId { get; set; }
    public required Note Note { get; set; }
}

public class AnalysisConfiguration : BaseEntityConfiguration<Analysis>
{
    public override void Configure(EntityTypeBuilder<Analysis> builder)
    {
        base.Configure(builder);
        
        builder.Property(c => c.RawValue)
            .IsRequired();
        
        builder.Property(c => c.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        builder.HasOne(c => c.Note)
            .WithMany(c => c.Analyses)
            .HasForeignKey(c => c.NoteId)
            .IsRequired();
    }
}