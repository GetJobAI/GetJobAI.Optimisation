using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class OptimisationActivitySuggestionConfiguration : IEntityTypeConfiguration<OptimisationActivitySuggestion>
{
    public void Configure(EntityTypeBuilder<OptimisationActivitySuggestion> builder)
    {
        builder.ToTable("optimisation_activity_suggestions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.OptimisationId)
            .HasColumnName("optimisation_id")
            .IsRequired();

        builder.Property(x => x.EntryId)
            .HasColumnName("entry_id")
            .IsRequired();

        builder.Property(x => x.Include)
            .HasColumnName("include")
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasColumnName("reason")
            .HasColumnType("text");

        builder.Property(x => x.HighlightsRewritten)
            .HasColumnName("highlights_rewritten")
            .HasColumnType("text[]")
            .IsRequired();

        builder.Property(x => x.Accepted)
            .HasColumnName("accepted");

        builder.Property(x => x.RejectionHint)
            .HasColumnName("rejection_hint")
            .HasMaxLength(1000);

        builder.Property(x => x.RewriteCount)
            .HasColumnName("rewrite_count")
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasIndex(x => new { x.OptimisationId, x.EntryId })
            .IsUnique()
            .HasDatabaseName("ix_optimisation_activity_suggestions_optimisation_entry");
    }
}
