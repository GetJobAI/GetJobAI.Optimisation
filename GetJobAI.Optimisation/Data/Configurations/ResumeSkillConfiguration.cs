using GetJobAI.Optimisation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class ResumeSkillConfiguration : IEntityTypeConfiguration<ResumeSkill>
{
    public void Configure(EntityTypeBuilder<ResumeSkill> builder)
    {
        builder.ToTable("resume_skills");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.ResumeId)
            .HasColumnName("resume_id")
            .IsRequired();

        builder.Property(x => x.SkillName)
            .HasColumnName("skill_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.SkillNameRaw)
            .HasColumnName("skill_name_raw")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Proficiency)
            .HasColumnName("proficiency")
            .HasMaxLength(50);

        builder.Property(x => x.Category)
            .HasColumnName("category")
            .HasMaxLength(100);

        builder.HasIndex(x => x.ResumeId)
            .HasDatabaseName("ix_resume_skills_resume_id");
    }
}
