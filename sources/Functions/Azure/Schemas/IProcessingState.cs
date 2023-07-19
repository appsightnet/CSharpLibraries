namespace AppSightNet.Functions.Azure.Schemas;

public interface IProcessingState
{
    DateTimeOffset LastProcessedTime { get; set; }
}
