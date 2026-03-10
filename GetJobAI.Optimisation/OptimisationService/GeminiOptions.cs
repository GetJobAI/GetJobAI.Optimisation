namespace GetJobAI.Optimisation.OptimisationService;

public class GeminiOptions
{
    public static readonly string SectionName = "Gemini";
    
    public string ApiKey { get; set; } = null!;
    
    public string PrimaryModel { get; set; } = null!;
}