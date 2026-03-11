using GetJobAI.Optimisation.OptimisationService;
using GetJobAI.Optimisation.OptimisationService.Contexts;
using GetJobAI.Optimisation.OptimisationService.Models;
using GetJobAI.Optimisation.OptimisationService.Results;

namespace GetJobAI.Optimisation.Contracts;

public interface IPromptRunner
{
    Task<PromptResult<WorkExperienceSuggestion>> RewriteExperienceAsync(
        WorkExperienceContext entry,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null);

    Task<PromptResult<SummarySuggestion>> RewriteSummaryAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null);

    Task<PromptResult<SkillsSuggestion>> OptimiseSkillsAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null);

    Task<PromptResult<SectionRelevancyRawSuggestions>> AssessSectionRelevancyAsync(
        OptimisationContext ctx,
        CancellationToken ct);

    Task<PromptResult<AtsExplanationResult>> ExplainAtsScoreAsync(
        OptimisationContext ctx,
        CancellationToken ct);

    Task<PromptResult<ActivitySuggestion>> RewriteActivityAsync(
        ActivityContext activity,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null);
    
    Task<PromptResult<XyzDetectResult>> DetectXyzOpportunityAsync(
        string bulletText,
        string jobTitle,
        string companyName,
        CancellationToken ct);
    
    Task<PromptResult<XyzRewriteResult>> RewriteWithXyzAsync(
        string originalBullet,
        string missingComponent,
        string coachQuestion,
        string userAnswer,
        string jobTitle,
        CancellationToken ct);
    
    Task<PromptResult<CoverLetterResult>> GenerateCoverLetterAsync(
        CoverLetterContext ctx,
        CancellationToken ct);
}
