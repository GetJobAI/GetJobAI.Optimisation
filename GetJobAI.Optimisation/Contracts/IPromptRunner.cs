using GetJobAI.Optimisation.OptimisationService.Contexts;
using GetJobAI.Optimisation.OptimisationService.Models;
using GetJobAI.Optimisation.OptimisationService.Results;

namespace GetJobAI.Optimisation.Contracts;

public interface IPromptRunner
{
    Task<WorkExperienceSuggestion> RewriteExperienceAsync(
        WorkExperienceContext entry,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null);

    Task<SummarySuggestion> RewriteSummaryAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null);

    Task<SkillsSuggestion> OptimiseSkillsAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null);

    Task<SectionRelevancyRawSuggestions> AssessSectionRelevancyAsync(
        OptimisationContext ctx,
        CancellationToken ct);

    Task<AtsExplanationResult> ExplainAtsScoreAsync(
        OptimisationContext ctx,
        CancellationToken ct);

    Task<ActivitySuggestion> RewriteActivityAsync(
        ActivityContext activity,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null);
    
    Task<XyzDetectResult> DetectXyzOpportunityAsync(
        string bulletText,
        string jobTitle,
        string companyName,
        CancellationToken ct);
    
    Task<XyzRewriteResult> RewriteWithXyzAsync(
        string originalBullet,
        string missingComponent,
        string coachQuestion,
        string userAnswer,
        string jobTitle,
        CancellationToken ct);
    
    Task<CoverLetterResult> GenerateCoverLetterAsync(
        CoverLetterContext ctx,
        CancellationToken ct);
}
