using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class ResumePublicationConfiguration : IEntityTypeConfiguration<ResumePublication>
{
    public void Configure(EntityTypeBuilder<ResumePublication> builder)
    {
        builder.ToTable("resume_publications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.ResumeId)
            .HasColumnName("resume_id")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Publisher)
            .HasColumnName("publisher")
            .HasMaxLength(300);

        builder.Property(x => x.PublicationDate)
            .HasColumnName("publication_date")
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder.HasIndex(x => x.ResumeId)
            .HasDatabaseName("ix_resume_publications_resume_id");
    }
}
