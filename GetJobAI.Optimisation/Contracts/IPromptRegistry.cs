using GetJobAI.Optimisation.Prompts;

namespace GetJobAI.Optimisation.Contracts;

public interface IPromptRegistry
{
    PromptTemplate Get(string promptId, string version = "1.0");
}