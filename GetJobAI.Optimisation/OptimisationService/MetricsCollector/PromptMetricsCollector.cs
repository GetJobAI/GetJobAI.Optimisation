namespace GetJobAI.Optimisation.OptimisationService.MetricsCollector;

public sealed class PromptMetricsCollector
{
    public event Action<PromptCallMetrics>? OnCall;

    public void Record(PromptCallMetrics metrics) => OnCall?.Invoke(metrics);
}