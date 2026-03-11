namespace GetJobAI.Optimisation.Messaging.Events;

public record ResumeOptimised
{
    public Guid OptimisationId { get; init; }

    public Guid ResumeId { get; init; }
    
    public Guid UserId { get; init; }

    public string AiSuggestionsJson { get; init; } = "{}";
    
    public short OriginalAtsScore { get; init; }

    public string Status { get; init; } = "AwaitingReview";
    
    public string? ErrorMessage { get; init; }
}