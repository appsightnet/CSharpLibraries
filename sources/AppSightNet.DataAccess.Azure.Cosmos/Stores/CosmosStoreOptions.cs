using Microsoft.Azure.Cosmos;

namespace AppSightNet.DataAccess.Azure.Cosmos.Stores;

public class CosmosStoreOptions
{
    public CosmosClient? Client { get; set; }

    public string DatabaseId { get; set; } = string.Empty;

    public string ContainerId { get; set; } = string.Empty;
}
