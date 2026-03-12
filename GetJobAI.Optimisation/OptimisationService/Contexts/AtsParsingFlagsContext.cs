namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class AtsParsingFlagsContext
{
    public bool HasComplexLayout { get; set; }
    
    public bool HasGraphics { get; set; }
    
    public bool HasHeadersFooters { get; set; }
    
    public bool HasNonStandardFonts { get; set; }
}