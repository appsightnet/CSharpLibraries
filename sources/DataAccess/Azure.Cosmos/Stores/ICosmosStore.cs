using AppSightNet.DataAccess.Azure.Cosmos.Entities;
using AppSightNet.DataAccess.Azure.Cosmos.Stores.Results;
using Microsoft.Azure.Cosmos;

namespace AppSightNet.DataAccess.Azure.Cosmos.Stores;

public interface ICosmosStore
{
    Task CreateAsync<T>(
        CosmosItem<T> item,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    );
    TransactionalBatch CreateTransactionalBatch(string partitionKeyValue);
    Task DeleteAsync<T>(
        string itemId,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    );
    Task<FirstResult<CosmosItem<T>>?> FirstOrDefaultAsync<T>(
        QueryDefinition queryDefinition,
        CancellationToken cancellationToken = default
    );
    Task<FirstResult<CosmosItem<T>>?> FirstOrDefaultAsync<T>(
        string type,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    );
    Task<GetResult<CosmosItem<T>>?> GetOrDefaultAsync<T>(
        string itemId,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    );
    Task<ListResult<CosmosItem<T>>> ListAsync<T>(
        QueryDefinition queryDefinition,
        string? continuationToken = null,
        CancellationToken cancellationToken = default
    );
    Task<ListResult<CosmosItem<T>>> ListAsync<T>(
        string type,
        string? partitionKeyValue = null,
        string? continuationToken = null,
        CancellationToken cancellationToken = default
    );
    Task UpdateAsync<T>(
        string itemId,
        CosmosItem<T> item,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    );
    Task UpsertAsync<T>(
        CosmosItem<T> item,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    );
}
