using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class ResumeConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.ToTable("resumes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.CandidateName)
            .HasColumnName("candidate_name")
            .HasMaxLength(300);

        builder.Property(x => x.ExistingSummary)
            .HasColumnName("existing_summary")
            .HasColumnType("text");

        builder.Property(x => x.DetectedLanguage)
            .HasColumnName("detected_language")
            .HasMaxLength(10);

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasMany(x => x.WorkExperiences)
            .WithOne(x => x.Resume)
            .HasForeignKey(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Skills)
            .WithOne(x => x.Resume)
            .HasForeignKey(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Publications)
            .WithOne(x => x.Resume)
            .HasForeignKey(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Activities)
            .WithOne(x => x.Resume)
            .HasForeignKey(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AdditionalSections)
            .WithOne(x => x.Resume)
            .HasForeignKey(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("ix_resumes_user_id");
    }
}
