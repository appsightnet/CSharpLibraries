namespace AppSightNet.Functions.Azure.Models;

public abstract class ProcessingStateBase : IProcessingState
{
    public DateTimeOffset LastProcessedTime { get; set; }
}
