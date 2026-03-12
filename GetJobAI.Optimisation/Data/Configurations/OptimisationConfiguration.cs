using System.Text.Json;
using GetJobAI.Optimisation.OptimisationService.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetJobAI.Optimisation.Data.Configurations;

public class OptimisationConfiguration : IEntityTypeConfiguration<Entities.Optimisation>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public void Configure(EntityTypeBuilder<Entities.Optimisation> builder)
    {
        builder.ToTable("optimisations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.ResumeId)
            .HasColumnName("resume_id")
            .IsRequired();

        builder.Property(x => x.JobAnalysisId)
            .HasColumnName("job_analysis_id")
            .IsRequired();

        builder.Property(x => x.JobTitle)
            .HasColumnName("job_title")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.CompanyName)
            .HasColumnName("company_name")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.OverallScore)
            .HasColumnName("overall_score")
            .IsRequired();

        builder.Property(x => x.ScoreKeywordEarned)
            .HasColumnName("score_keyword_earned")
            .IsRequired();

        builder.Property(x => x.ScoreKeywordMax)
            .HasColumnName("score_keyword_max")
            .IsRequired();

        builder.Property(x => x.ScoreSkillEarned)
            .HasColumnName("score_skill_earned")
            .IsRequired();

        builder.Property(x => x.ScoreSkillMax)
            .HasColumnName("score_skill_max")
            .IsRequired();

        builder.Property(x => x.ScoreFormatEarned)
            .HasColumnName("score_format_earned")
            .IsRequired();

        builder.Property(x => x.ScoreFormatMax)
            .HasColumnName("score_format_max")
            .IsRequired();

        builder.Property(x => x.ScoreExperienceEarned)
            .HasColumnName("score_experience_earned")
            .IsRequired();

        builder.Property(x => x.ScoreExperienceMax)
            .HasColumnName("score_experience_max")
            .IsRequired();

        builder.Property(x => x.AtsDetailsJson)
            .HasColumnName("ats_details")
            .HasColumnType("jsonb");

        builder.Property(x => x.AtsExplanation)
            .HasColumnName("ats_explanation")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonOptions),
                v => JsonSerializer.Deserialize<AtsExplanationResult>(v, JsonOptions));

        builder.Property(x => x.SkillsGap)
            .HasColumnName("skills_gap")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonOptions),
                v => JsonSerializer.Deserialize<SkillsGapResult>(v, JsonOptions));

        builder.Property(x => x.ErrorMessage)
            .HasColumnName("error_message")
            .HasMaxLength(2000);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.StartedAt)
            .HasColumnName("started_at");

        builder.Property(x => x.CompletedAt)
            .HasColumnName("completed_at");

        builder.HasOne(x => x.SummarySuggestion)
            .WithOne(x => x.Optimisation)
            .HasForeignKey<Entities.OptimisationSummarySuggestion>(x => x.OptimisationId);

        builder.HasOne(x => x.CoverLetter)
            .WithOne(x => x.Optimisation)
            .HasForeignKey<Entities.OptimisationCoverLetter>(x => x.OptimisationId);

        builder.HasMany(x => x.WorkExperienceSuggestions)
            .WithOne(x => x.Optimisation)
            .HasForeignKey(x => x.OptimisationId);

        builder.HasMany(x => x.ActivitySuggestions)
            .WithOne(x => x.Optimisation)
            .HasForeignKey(x => x.OptimisationId);

        builder.HasMany(x => x.SectionSuggestions)
            .WithOne(x => x.Optimisation)
            .HasForeignKey(x => x.OptimisationId);

        builder.HasIndex(x => x.ResumeId)
            .HasDatabaseName("ix_optimisations_resume_id");

        builder.HasIndex(x => x.JobAnalysisId)
            .HasDatabaseName("ix_optimisations_job_analysis_id");
        
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("ix_optimisations_status");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("ix_optimisations_created_at");
    }
}
