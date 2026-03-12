using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class ResumeWorkExperienceConfiguration : IEntityTypeConfiguration<ResumeWorkExperience>
{
    public void Configure(EntityTypeBuilder<ResumeWorkExperience> builder)
    {
        builder.ToTable("resume_work_experiences");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.ResumeId)
            .HasColumnName("resume_id")
            .IsRequired();

        builder.Property(x => x.JobTitle)
            .HasColumnName("job_title")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.CompanyName)
            .HasColumnName("company_name")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .HasColumnName("start_date")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .HasColumnName("end_date")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Bullets)
            .HasColumnName("bullets")
            .HasColumnType("text[]")
            .IsRequired();

        builder.HasIndex(x => x.ResumeId)
            .HasDatabaseName("ix_resume_work_experiences_resume_id");
    }
}
