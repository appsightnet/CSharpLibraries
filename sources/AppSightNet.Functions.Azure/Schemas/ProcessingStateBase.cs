namespace AppSightNet.Functions.Azure.Schemas;

public abstract class ProcessingStateBase : IProcessingState
{
    public DateTimeOffset LastProcessedTime { get; set; }
}
