using System.Diagnostics;
using GetJobAI.Optimisation.Contracts;
using GetJobAI.Optimisation.OptimisationService.Contexts;
using GetJobAI.Optimisation.OptimisationService.Models;
using GetJobAI.Optimisation.OptimisationService.Results;
using ActivityContext = GetJobAI.Optimisation.OptimisationService.Contexts.ActivityContext;

namespace GetJobAI.Optimisation.OptimisationService.MetricsCollector;

public sealed class LoggingPromptRunner(
    IPromptRunner inner,
    PromptMetricsCollector metrics,
    ILogger<LoggingPromptRunner> logger)
    : IPromptRunner
{
    public Task<PromptResult<WorkExperienceSuggestion>> RewriteExperienceAsync(
        WorkExperienceContext entry,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
        => ExecuteAsync(
            promptId: "PR-01",
            meta:     $"entry={entry.EntryId} job=\"{entry.JobTitle}\" is_retry={hint is not null}",
            call:     () => inner.RewriteExperienceAsync(entry, ctx, ct, hint));
    
    public Task<PromptResult<SummarySuggestion>> RewriteSummaryAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
        => ExecuteAsync(
            promptId: "PR-02",
            meta:     $"job=\"{ctx.JobTitle}\" has_existing={ctx.ExistingSummary is not null} is_retry={hint is not null}",
            call:     () => inner.RewriteSummaryAsync(ctx, ct, hint));
    
    public Task<PromptResult<SkillsGapResult>> OptimiseSkillsAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
        => ExecuteAsync(
            promptId: "PR-03",
            meta:     $"job=\"{ctx.JobTitle}\" candidate_skills={ctx.Skills.Count} required_skills={ctx.JobRequiredSkills.Count} is_retry={hint is not null}",
            call:     () => inner.OptimiseSkillsAsync(ctx, ct, hint));
    
    public Task<PromptResult<SectionRelevancyRawSuggestions>> AssessSectionRelevancyAsync(
        OptimisationContext ctx,
        CancellationToken ct)
        => ExecuteAsync(
            promptId: "PR-04",
            meta:     $"publications={ctx.Publications.Count} activities={ctx.Activities.Count} additional={ctx.AdditionalSections.Count}",
            call:     () => inner.AssessSectionRelevancyAsync(ctx, ct));
    
    public Task<PromptResult<AtsExplanationResult>> ExplainAtsScoreAsync(
        OptimisationContext ctx,
        CancellationToken ct)
        => ExecuteAsync(
            promptId: "PR-05",
            meta:     $"job=\"{ctx.JobTitle}\" score={ctx.OverallScore} keyword={ctx.ScoreKeywordEarned}/{ctx.ScoreKeywordMax} skill={ctx.ScoreSkillEarned}/{ctx.ScoreSkillMax}",
            call:     () => inner.ExplainAtsScoreAsync(ctx, ct));
    
    public Task<PromptResult<ActivitySuggestion>> RewriteActivityAsync(
        ActivityContext activity,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
        => ExecuteAsync(
            promptId: "PR-06",
            meta:     $"entry={activity.EntryId} activity=\"{activity.ActivityName}\" is_retry={hint is not null}",
            call:     () => inner.RewriteActivityAsync(activity, ctx, ct, hint));

    public Task<PromptResult<XyzDetectResult>> DetectXyzOpportunityAsync(
        string bulletText,
        string jobTitle,
        string companyName,
        CancellationToken ct,
        string? language = null)
        => ExecuteAsync(
            promptId: "PR-07",
            meta:     $"job=\"{jobTitle}\" bullet_chars={bulletText.Length} lang={language ?? "en"}",
            call:     () => inner.DetectXyzOpportunityAsync(bulletText, jobTitle, companyName, ct, language));

    public Task<PromptResult<XyzRewriteResult>> RewriteWithXyzAsync(
        string originalBullet,
        string missingComponent,
        string coachQuestion,
        string userAnswer,
        string jobTitle,
        CancellationToken ct,
        string? language = null)
        => ExecuteAsync(
            promptId: "PR-08",
            meta:     $"job=\"{jobTitle}\" missing={missingComponent} answer_chars={userAnswer.Length} lang={language ?? "en"}",
            call:     () => inner.RewriteWithXyzAsync(originalBullet, missingComponent, coachQuestion, userAnswer, jobTitle, ct, language));
    
    public Task<PromptResult<CoverLetterResult>> GenerateCoverLetterAsync(
        CoverLetterContext ctx,
        CancellationToken ct)
        => ExecuteAsync(
            promptId: "PR-09",
            meta:     $"job=\"{ctx.JobTitle}\" lang={ctx.Language} rewrite_count={ctx.RewriteCount}",
            call:     () => inner.GenerateCoverLetterAsync(ctx, ct));

    private async Task<PromptResult<T>> ExecuteAsync<T>(
        string promptId,
        string meta,
        Func<Task<PromptResult<T>>> call)
    {
        logger.LogInformation("[{PromptId}] → starting | {Meta}", promptId, meta);

        var sw = Stopwatch.StartNew();

        try
        {
            var result = await call();
            sw.Stop();

            logger.LogInformation(
                "[{PromptId}] ✓ completed | elapsed={ElapsedMs}ms | tokens=({Input}→{Output}) model={Model} finish={FinishReason} | {Meta}",
                promptId, sw.ElapsedMilliseconds,
                result.InputTokens, result.OutputTokens,
                result.Model, result.FinishReason,
                meta);

            metrics.Record(new PromptCallMetrics
            {
                PromptId = promptId,
                PromptVersion = result.PromptVersion,
                Model = result.Model,
                InputTokens = result.InputTokens ?? 0,
                OutputTokens = result.OutputTokens ?? 0,
                CachedTokens = result.CachedTokens ?? 0,
                TotalTokens = result.TotalTokens ?? 0,
                ElapsedMs = sw.ElapsedMilliseconds,
                Success = result.Success,
                Reason = result.FinishReason,
                CreatedAt = DateTime.UtcNow,
                OptimisationId = result.OptimisationId
            });

            return result;
        }
        catch (Exception ex)
        {
            sw.Stop();

            logger.LogError(ex,
                "[{PromptId}] ✗ failed | elapsed={ElapsedMs}ms | error=\"{Error}\" | {Meta}",
                promptId, sw.ElapsedMilliseconds, ex.Message, meta);

            metrics.Record(new PromptCallMetrics
            {
                PromptId = promptId,
                ElapsedMs = sw.ElapsedMilliseconds,
                Success = false,
                Reason = ex.Message,
                CreatedAt = DateTime.UtcNow
            });

            throw;
        }
    }
}