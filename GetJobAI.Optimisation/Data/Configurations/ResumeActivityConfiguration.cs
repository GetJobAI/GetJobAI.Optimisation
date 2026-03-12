using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class ResumeActivityConfiguration : IEntityTypeConfiguration<ResumeActivity>
{
    public void Configure(EntityTypeBuilder<ResumeActivity> builder)
    {
        builder.ToTable("resume_activities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.ResumeId)
            .HasColumnName("resume_id")
            .IsRequired();

        builder.Property(x => x.ActivityName)
            .HasColumnName("activity_name")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.Organization)
            .HasColumnName("organization")
            .HasMaxLength(300);

        builder.Property(x => x.Role)
            .HasColumnName("role")
            .HasMaxLength(200);

        builder.Property(x => x.Highlights)
            .HasColumnName("highlights")
            .HasColumnType("text[]")
            .IsRequired();

        builder.HasIndex(x => x.ResumeId)
            .HasDatabaseName("ix_resume_activities_resume_id");
    }
}
