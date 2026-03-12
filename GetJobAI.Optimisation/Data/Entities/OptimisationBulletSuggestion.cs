namespace GetJobAI.Optimisation.Data.Entities;

public class OptimisationBulletSuggestion
{
    public Guid Id { get; private set; }
    
    public Guid WorkExperienceSuggestionId { get; private set; }
    
    public string Original { get; private set; } = string.Empty;
    
    public string Rewritten { get; set; } = string.Empty;
    
    public List<string> KeywordsAdded { get; set; } = [];
    
    public bool XyzApplied { get; set; }
    
    public bool? Accepted { get; set; }

    public OptimisationWorkExperienceSuggestion WorkExperienceSuggestion { get; private set; } = null!;

    private OptimisationBulletSuggestion() { }

    public static OptimisationBulletSuggestion Create(
        Guid workExperienceSuggestionId,
        string original,
        string rewritten,
        List<string> keywordsAdded,
        bool xyzApplied) => new()
    {
        Id = Guid.NewGuid(),
        WorkExperienceSuggestionId = workExperienceSuggestionId,
        Original = original,
        Rewritten = rewritten,
        KeywordsAdded = keywordsAdded,
        XyzApplied = xyzApplied
    };
}
