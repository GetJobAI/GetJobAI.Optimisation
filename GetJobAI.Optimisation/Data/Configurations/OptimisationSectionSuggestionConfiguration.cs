using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class OptimisationSectionSuggestionConfiguration : IEntityTypeConfiguration<OptimisationSectionSuggestion>
{
    public void Configure(EntityTypeBuilder<OptimisationSectionSuggestion> builder)
    {
        builder.ToTable("optimisation_section_suggestions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.OptimisationId)
            .HasColumnName("optimisation_id")
            .IsRequired();

        builder.Property(x => x.Category)
            .HasColumnName("category")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.EntryId)
            .HasColumnName("entry_id")
            .IsRequired();

        builder.Property(x => x.SectionType)
            .HasColumnName("section_type")
            .HasMaxLength(100);

        builder.Property(x => x.Include)
            .HasColumnName("include")
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasColumnName("reason")
            .HasColumnType("text");

        builder.Property(x => x.Accepted)
            .HasColumnName("accepted");

        builder.HasIndex(x => new { x.OptimisationId, x.Category, x.EntryId })
            .IsUnique()
            .HasDatabaseName("ix_optimisation_section_suggestions_optimisation_category_entry");
    }
}
