namespace GetJobAI.Optimisation.Prompts;

public sealed record PromptTemplate(
    string PromptId,
    string Version,
    string SystemMessage,
    float Temperature,
    int MaxTokens,
    bool SupportsStreaming = false);