using System.Text.Json;
using GetJobAI.Optimisation.Contracts;
using GetJobAI.Optimisation.Messaging.Events;
using GetJobAI.Optimisation.OptimisationService.Contexts;
using MassTransit;

namespace GetJobAI.Optimisation.Messaging.Consumers;

public class ResumeScoredConsumer(
    IOptimisationOrchestrator orchestrator,
    ILogger<ResumeScoredConsumer> logger) : IConsumer<ResumeScored>
{
    public async Task Consume(ConsumeContext<ResumeScored> context)
    {
        var msg = context.Message;

        logger.LogInformation(
            "Optimisation {OptimisationId} started — resume {ResumeId}, user {UserId}",
            msg.OptimisationId, msg.ResumeId, msg.UserId);

        try
        {
            var ctx = MapToOptimisationContext(msg);
            var suggestions = await orchestrator.RunAsync(ctx, context.CancellationToken);

            await context.Publish(new ResumeOptimised
            {
                OptimisationId = msg.OptimisationId,
                ResumeId = msg.ResumeId,
                UserId = msg.UserId,
                AiSuggestionsJson = JsonSerializer.Serialize(suggestions),
                OriginalAtsScore = msg.OverallScore,
                Status = "AwaitingReview"
            });

            logger.LogInformation("Optimisation {OptimisationId} completed", msg.OptimisationId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Optimisation {OptimisationId} failed", msg.OptimisationId);

            await context.Publish(new ResumeOptimised
            {
                OptimisationId = msg.OptimisationId,
                ResumeId = msg.ResumeId,
                UserId = msg.UserId,
                OriginalAtsScore = msg.OverallScore,
                Status = "Failed",
                ErrorMessage = ex.Message
            });
        }
    }

    private static OptimisationContext MapToOptimisationContext(ResumeScored msg) => new()
    {
        OptimisationId = msg.OptimisationId,
        ResumeId = msg.ResumeId,
        UserId = msg.UserId,
        JobTitle = msg.JobTitle,
        CompanyName = msg.CompanyName,
        CandidateName = msg.CandidateName,
        ExistingSummary = msg.ExistingSummary,
        DetectedLanguage = msg.DetectedLanguage,
        OverallScore = msg.OverallScore,
        ScoreKeyword = msg.ScoreKeyword,
        ScoreSkill = msg.ScoreSkill,
        ScoreFormat = msg.ScoreFormat,
        ScoreExperience = msg.ScoreExperience,

        WorkExperiences = msg.WorkExperiences.Select(we => new WorkExperienceContext
        {
            EntryId = we.EntryId,
            JobTitle = we.JobTitle,
            CompanyName = we.CompanyName,
            StartDate = we.StartDate,
            EndDate = we.EndDate,
            Bullets = we.Bullets
        }).ToList(),

        Skills = msg.Skills.Select(s => new SkillContext
        {
            SkillName = s.SkillName,
            SkillNameRaw = s.SkillNameRaw,
            Proficiency = s.Proficiency,
            Category = s.Category
        }).ToList(),

        JobRequiredSkills = msg.JobRequiredSkills.Select(s => new JobSkillContext
        {
            SkillName = s.SkillName,
            ImportanceScore = s.ImportanceScore,
            Category = s.Category,
            IsRequired = true
        }).ToList(),

        JobPreferredSkills = msg.JobPreferredSkills.Select(s => new JobSkillContext
        {
            SkillName = s.SkillName,
            ImportanceScore = s.ImportanceScore,
            Category = s.Category,
            IsRequired = false
        }).ToList(),

        Publications = msg.Publications.Select(p => new PublicationContext
        {
            EntryId = p.EntryId,
            Title = p.Title,
            Publisher = p.Publisher,
            PublicationDate = p.PublicationDate,
            Description = p.Description
        }).ToList(),

        Activities = msg.Activities.Select(a => new ActivityContext
        {
            EntryId = a.EntryId,
            ActivityName = a.ActivityName,
            Organization = a.Organization,
            Role = a.Role,
            Highlights = a.Highlights
        }).ToList(),

        AdditionalSections = msg.AdditionalSections.Select(a => new AdditionalSectionContext
        {
            EntryId = a.EntryId,
            SectionType = a.SectionType ?? string.Empty,
            Title = a.Title ?? string.Empty,
            ContentJson = a.ContentJson ?? string.Empty
        }).ToList()
    };
}
