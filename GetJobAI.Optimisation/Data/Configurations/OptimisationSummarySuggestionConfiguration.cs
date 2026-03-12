using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class OptimisationSummarySuggestionConfiguration : IEntityTypeConfiguration<OptimisationSummarySuggestion>
{
    public void Configure(EntityTypeBuilder<OptimisationSummarySuggestion> builder)
    {
        builder.ToTable("optimisation_summary_suggestions");

        builder.HasKey(x => x.OptimisationId);

        builder.Property(x => x.OptimisationId)
            .HasColumnName("optimisation_id")
            .ValueGeneratedNever();

        builder.Property(x => x.Original)
            .HasColumnName("original")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Rewritten)
            .HasColumnName("rewritten")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.KeywordsIncorporated)
            .HasColumnName("keywords_incorporated")
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
    }
}
