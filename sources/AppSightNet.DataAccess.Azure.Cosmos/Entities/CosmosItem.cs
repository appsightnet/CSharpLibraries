using Newtonsoft.Json;

namespace AppSightNet.DataAccess.Azure.Cosmos.Entities;

public class CosmosItem<T> // TODO: Divide request/response
{
    public string Id { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string PartitionKey { get; set; } = string.Empty;

    public string UniqueKey { get; set; } = string.Empty;

    public T? Data { get; set; }

    [JsonProperty(PropertyName = "_etag")]
    public string? ETag { get; set; }
}
