using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class OptimisationCoverLetterConfiguration : IEntityTypeConfiguration<OptimisationCoverLetter>
{
    public void Configure(EntityTypeBuilder<OptimisationCoverLetter> builder)
    {
        builder.ToTable("optimisation_cover_letters");

        builder.HasKey(x => x.OptimisationId);

        builder.Property(x => x.OptimisationId)
            .HasColumnName("optimisation_id")
            .ValueGeneratedNever();

        builder.Property(x => x.CoverLetter)
            .HasColumnName("cover_letter")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.WordCount)
            .HasColumnName("word_count")
            .IsRequired();

        builder.Property(x => x.SalutationUsed)
            .HasColumnName("salutation_used")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.KeyPointsMade)
            .HasColumnName("key_points_made")
            .HasColumnType("text[]")
            .IsRequired();

        builder.Property(x => x.Accepted)
            .HasColumnName("accepted");

        builder.Property(x => x.RewriteCount)
            .HasColumnName("rewrite_count")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.GeneratedAt)
            .HasColumnName("generated_at")
            .IsRequired();
    }
}
