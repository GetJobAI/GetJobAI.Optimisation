using GetJobAI.Optimisation.Contracts;
using GetJobAI.Optimisation.OptimisationService.Contexts;
using GetJobAI.Optimisation.OptimisationService.Models;

namespace GetJobAI.Optimisation.OptimisationService;

public class OptimisationOrchestrator(IPromptRunner promptRunner) : IOptimisationOrchestrator
{
    public async Task<AiSuggestionsDocument> RunAsync(OptimisationContext ctx, CancellationToken ct)
    {
        var summaryTask = promptRunner.RewriteSummaryAsync(ctx, ct);
        var skillsTask = promptRunner.OptimiseSkillsAsync(ctx, ct);
        var atsExplanationTask = promptRunner.ExplainAtsScoreAsync(ctx, ct);
        var sectionRelevancyTask = promptRunner.AssessSectionRelevancyAsync(ctx, ct);
        var workExpTasks = ctx.WorkExperiences
            .Select(we => promptRunner.RewriteExperienceAsync(we, ctx, ct))
            .ToList();

        await Task.WhenAll(
        [
            summaryTask,
            skillsTask,
            atsExplanationTask,
            sectionRelevancyTask,
            .. workExpTasks
        ]);

        var sectionRelevancy = sectionRelevancyTask.Result.Content;

        return new AiSuggestionsDocument
        {
            Summary = summaryTask.Result.Content,
            WorkExperience = workExpTasks.Select(t => t.Result.Content).ToList(),
            SkillsGap = skillsTask.Result.Content,
            AtsExplanation = atsExplanationTask.Result.Content,
            Publications = sectionRelevancy.Publications,
            Activities = sectionRelevancy.Activities,
            AdditionalSections = sectionRelevancy.AdditionalSections
        };
    }
}
