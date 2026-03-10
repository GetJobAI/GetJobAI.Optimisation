namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public record ActivityContext(
    Guid EntryId,
    string ActivityName,
    string? Organization,
    string? Role,
    List<string> Highlights);