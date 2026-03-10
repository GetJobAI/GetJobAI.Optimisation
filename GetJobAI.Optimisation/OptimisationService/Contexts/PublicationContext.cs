namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public record PublicationContext(
    Guid EntryId,
    string Title,
    string? Publisher,
    string? PublicationDate,
    string? Description);