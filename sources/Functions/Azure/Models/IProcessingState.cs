namespace AppSightNet.Functions.Azure.Models;

public interface IProcessingState
{
    DateTimeOffset LastProcessedTime { get; set; }
}
