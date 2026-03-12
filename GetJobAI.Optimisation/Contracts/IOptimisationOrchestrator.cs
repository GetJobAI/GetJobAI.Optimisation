using GetJobAI.Optimisation.OptimisationService.Contexts;
using GetJobAI.Optimisation.OptimisationService.Models;

namespace GetJobAI.Optimisation.Contracts;

public interface IOptimisationOrchestrator
{
    Task<AiSuggestionsDocument> RunAsync(OptimisationContext ctx, CancellationToken ct);
}
