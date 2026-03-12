using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class ResumeAdditionalSectionConfiguration : IEntityTypeConfiguration<ResumeAdditionalSection>
{
    public void Configure(EntityTypeBuilder<ResumeAdditionalSection> builder)
    {
        builder.ToTable("resume_additional_sections");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.ResumeId)
            .HasColumnName("resume_id")
            .IsRequired();

        builder.Property(x => x.SectionType)
            .HasColumnName("section_type")
            .HasMaxLength(100);

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(300);

        builder.Property(x => x.ContentJson)
            .HasColumnName("content_json")
            .HasColumnType("text");

        builder.HasIndex(x => x.ResumeId)
            .HasDatabaseName("ix_resume_additional_sections_resume_id");
    }
}
