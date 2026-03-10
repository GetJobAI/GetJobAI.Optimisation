using System.Text.Json;
using GetJobAI.Optimisation.Contracts;
using GetJobAI.Optimisation.OptimisationService.Contexts;
using GetJobAI.Optimisation.OptimisationService.Models;
using GetJobAI.Optimisation.OptimisationService.Results;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;

namespace GetJobAI.Optimisation.OptimisationService;

public class PromptRunner(
    Client client, 
    IOptions<GeminiOptions> options, 
    IPromptRegistry promptRegistry) : IPromptRunner
{
    private readonly GeminiOptions _options = options.Value;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<WorkExperienceSuggestion> RewriteExperienceAsync(
        WorkExperienceContext entry,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
    {
        var userMessage = $"""
               I am optimising a resume for the following role:
               <target_role>
                 Job Title: {ctx.JobTitle}
                 Company:   {ctx.CompanyName}
               </target_role>

               Here is one work experience entry to rewrite:
               <experience_entry>
                 Job Title: {entry.JobTitle}
                 Company:   {entry.CompanyName}
                 Period:    {entry.StartDate} – {entry.EndDate}
                 Bullets:
                 {string.Join("\n  ", entry.Bullets.Select((b, i) => $"{i + 1}. {b}"))}
               </experience_entry>

               Missing ATS keywords to naturally incorporate (ordered by importance):
               <missing_keywords>
               {JsonSerializer.Serialize(ctx.JobRequiredSkills
                   .Select(s => new { keyword = s.SkillName, importance = s.ImportanceScore })
                   .OrderByDescending(s => s.importance))}
               </missing_keywords>

               {(hint is not null ? $"The user rejected the previous rewrite. Their feedback:\n<rejection_hint>{hint}</rejection_hint>" : "")}

               Rewrite the bullets. Return only valid JSON matching the schema in your instructions.
               """;

        float? tempOverride = hint is not null
            ? promptRegistry.Get("PR-01").Temperature + 0.2f
            : null;

        var parsed = await CallModelAsync<WorkExperienceSuggestion>("PR-01", userMessage, tempOverride, ct);

        parsed.EntryId = entry.EntryId;
        parsed.RewriteCount = hint is not null ? 1 : 0;

        return parsed;
    }

    public async Task<SummarySuggestion> RewriteSummaryAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
    {
        var userMessage = $"""
               Rewrite the professional summary for a candidate applying for the following role:
               <target_role>
                 Job Title: {ctx.JobTitle}
                 Company:   {ctx.CompanyName}
               </target_role>

               Candidate's current summary:
               <existing_summary>
               {ctx.ExistingSummary ?? "(no existing summary — write from scratch based on the context below)"}
               </existing_summary>

               Candidate's skills for context:
               <candidate_skills>
               {JsonSerializer.Serialize(ctx.Skills.Select(s => new { skill = s.SkillNameRaw, proficiency = s.Proficiency }))}
               </candidate_skills>

               Missing ATS keywords to incorporate naturally:
               <missing_keywords>
               {JsonSerializer.Serialize(ctx.JobRequiredSkills
                   .OrderByDescending(s => s.ImportanceScore)
                   .Take(10)
                   .Select(s => s.SkillName))}
               </missing_keywords>

               {(hint is not null ? $"The user rejected the previous rewrite. Their feedback:\n<rejection_hint>{hint}</rejection_hint>" : "")}

               Return only valid JSON matching the schema in your instructions.
               """;

        float? tempOverride = hint is not null
            ? promptRegistry.Get("PR-02").Temperature + 0.2f
            : null;

        var parsed = await CallModelAsync<SummarySuggestion>("PR-02", userMessage, tempOverride, ct);

        parsed.Original = ctx.ExistingSummary ?? string.Empty;
        parsed.RewriteCount = hint is not null ? 1 : 0;

        return parsed;
    }

    public async Task<SkillsSuggestion> OptimiseSkillsAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
    {
        var userMessage = $"""
            Analyse the candidate's skills against the target role and produce a skills optimisation plan.

            Target role:
            <target_role>
              Job Title: {ctx.JobTitle}
              Company:   {ctx.CompanyName}
            </target_role>

            Candidate's current skills:
            <candidate_skills>
            {JsonSerializer.Serialize(ctx.Skills.Select(s => new
            {
                skill       = s.SkillNameRaw,
                normalised  = s.SkillName,
                proficiency = s.Proficiency,
                category    = s.Category
            }))}
            </candidate_skills>

            Required skills from the job description (ordered by importance):
            <required_skills>
            {JsonSerializer.Serialize(ctx.JobRequiredSkills
                .OrderByDescending(s => s.ImportanceScore)
                .Select(s => new { skill = s.SkillName, importance = s.ImportanceScore, category = s.Category }))}
            </required_skills>

            Preferred skills from the job description:
            <preferred_skills>
            {JsonSerializer.Serialize(ctx.JobPreferredSkills
                .Select(s => new { skill = s.SkillName, category = s.Category }))}
            </preferred_skills>

            ATS skill alignment score: {ctx.ScoreSkill}/100

            {(hint is not null ? $"The user rejected the previous suggestion. Their feedback:\n<rejection_hint>{hint}</rejection_hint>" : "")}

            Return only valid JSON matching the schema in your instructions.
            """;

        float? tempOverride = hint is not null
            ? promptRegistry.Get("PR-03").Temperature + 0.2f
            : null;

        var parsed = await CallModelAsync<SkillsSuggestion>("PR-03", userMessage, tempOverride, ct);

        parsed.RewriteCount = hint is not null ? 1 : 0;

        return parsed;
    }

    public async Task<SectionRelevancyRawSuggestions> AssessSectionRelevancyAsync(
        OptimisationContext ctx,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
    
    public async Task<AtsExplanationResult> ExplainAtsScoreAsync(
        OptimisationContext ctx,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }


    public async Task<ActivitySuggestion> RewriteActivityAsync(
        ActivityContext activity,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
    {
        throw new NotImplementedException();
    }
    
    private async Task<T> CallModelAsync<T>(
        string promptId,
        string userMessage,
        float? temperatureOverride,
        CancellationToken ct)
    {
        var prompt = promptRegistry.Get(promptId);

        var config = new GenerateContentConfig
        {
            SystemInstruction = new Content
            {
                Parts = [new Part { Text = prompt.SystemMessage }]
            },
            Temperature      = temperatureOverride ?? prompt.Temperature,
            MaxOutputTokens  = prompt.MaxTokens,
            ResponseMimeType = "application/json"
        };

        var response = await client.Models.GenerateContentAsync(
            model: _options.PrimaryModel,
            contents: new Content
            {
                Role  = "user",
                Parts = [new Part { Text = userMessage }]
            },
            config: config,
            ct
        );

        if (response.Candidates is null || response.Candidates.Count == 0)
        {
            throw new InvalidOperationException($"{promptId}: model returned no candidates.");
        }

        var json = response.Candidates[0].Content?.Parts?[0].Text;

        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidOperationException($"{promptId}: model response was empty.");
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(json, JsonOptions);
            
            return result ?? throw new InvalidOperationException($"{promptId}: response deserialized to null.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"{promptId}: failed to deserialize response. Raw: {json[..Math.Min(300, json.Length)]}", ex);
        }
    }
    
    private static void MapEntryIds(
        List<SectionRelevancySuggestion> suggestions,
        List<Guid> entryIds)
    {
        for (var i = 0; i < suggestions.Count && i < entryIds.Count; i++)
        {
            suggestions[i].EntryId = entryIds[i];
        }
    }
}