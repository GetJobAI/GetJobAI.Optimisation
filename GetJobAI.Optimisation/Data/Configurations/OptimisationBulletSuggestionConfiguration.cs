using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class OptimisationBulletSuggestionConfiguration : IEntityTypeConfiguration<OptimisationBulletSuggestion>
{
    public void Configure(EntityTypeBuilder<OptimisationBulletSuggestion> builder)
    {
        builder.ToTable("optimisation_bullet_suggestions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.WorkExperienceSuggestionId)
            .HasColumnName("work_experience_suggestion_id")
            .IsRequired();

        builder.Property(x => x.Original)
            .HasColumnName("original")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Rewritten)
            .HasColumnName("rewritten")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.KeywordsAdded)
            .HasColumnName("keywords_added")
            .HasColumnType("text[]")
            .IsRequired();

        builder.Property(x => x.XyzApplied)
            .HasColumnName("xyz_applied")
            .IsRequired();

        builder.Property(x => x.Accepted)
            .HasColumnName("accepted");

        builder.HasIndex(x => x.WorkExperienceSuggestionId)
            .HasDatabaseName("ix_optimisation_bullet_suggestions_work_experience_id");
    }
}
