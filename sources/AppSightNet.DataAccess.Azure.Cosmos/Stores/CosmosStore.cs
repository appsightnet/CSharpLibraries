using AppSightNet.DataAccess.Azure.Cosmos.Entities;
using AppSightNet.DataAccess.Azure.Cosmos.Stores.Results;
using Microsoft.Azure.Cosmos;

namespace AppSightNet.DataAccess.Azure.Cosmos.Stores;

public class CosmosStore : ICosmosStore
{
    private Database _database { get; }
    private string _containerId { get; }

    public CosmosStore(CosmosClient cosmosClient, CosmosStoreOptions options)
    {
        this._database = cosmosClient.GetDatabase(options.DatabaseId);
        this._containerId = options.ContainerId;
    }

    public async Task<FirstResult<CosmosItem<T>>?> FirstOrDefaultAsync<T>(
        string type,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    )
    {
        var queryDefinition = new QueryDefinition(
            "SELECT * FROM c WHERE c.partitionKey = @partitionKeyValue AND c.type = @type"
        )
            .WithParameter("@partitionKeyValue", partitionKeyValue)
            .WithParameter("@type", type);

        return await this.FirstOrDefaultAsync<T>(
                queryDefinition: queryDefinition,
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
    }

    public async Task<FirstResult<CosmosItem<T>>?> FirstOrDefaultAsync<T>(
        QueryDefinition queryDefinition,
        CancellationToken cancellationToken = default
    )
    {
        var container = this._database.GetContainer(this._containerId);
        using var feedIterator = container.GetItemQueryIterator<CosmosItem<T>>(queryDefinition);
        var response = await feedIterator.ReadNextAsync(cancellationToken).ConfigureAwait(false);
        var item = response.FirstOrDefault();

        if (item == null)
        {
            return null;
        }

        return new FirstResult<CosmosItem<T>>(item);
    }

    public async Task<GetResult<CosmosItem<T>>?> GetOrDefaultAsync<T>(
        string itemId,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    )
    {
        var container = this._database.GetContainer(this._containerId);
        var itemResponse = await container
            .ReadItemAsync<CosmosItem<T>>(
                id: itemId,
                partitionKey: new PartitionKey(partitionKeyValue),
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
        var item = itemResponse.Resource;

        if (item == null)
        {
            return null;
        }

        return new GetResult<CosmosItem<T>>(item);
    }

    public TransactionalBatch CreateTransactionalBatch(string partitionKeyValue)
    {
        var container = this._database.GetContainer(this._containerId);
        return container.CreateTransactionalBatch(new PartitionKey(partitionKeyValue));
    }

    public async Task CreateAsync<T>(
        CosmosItem<T> item,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    )
    {
        var container = this._database.GetContainer(this._containerId);
        await container
            .CreateItemAsync(
                item: item,
                partitionKey: new PartitionKey(partitionKeyValue),
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
    }

    public async Task UpdateAsync<T>(
        string itemId,
        CosmosItem<T> item,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    )
    {
        var container = this._database.GetContainer(this._containerId);
        var requestOptions = string.IsNullOrEmpty(item.ETag)
            ? null
            : new ItemRequestOptions { IfMatchEtag = item.ETag };
        await container
            .ReplaceItemAsync(
                item: item,
                id: itemId,
                partitionKey: new PartitionKey(partitionKeyValue),
                requestOptions: requestOptions,
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
    }

    public async Task UpsertAsync<T>(
        CosmosItem<T> item,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    )
    {
        var container = this._database.GetContainer(this._containerId);
        var requestOptions = string.IsNullOrEmpty(item.ETag)
            ? null
            : new ItemRequestOptions { IfMatchEtag = item.ETag };
        await container
            .UpsertItemAsync(
                item: item,
                partitionKey: new PartitionKey(partitionKeyValue),
                requestOptions: requestOptions,
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
    }

    public async Task DeleteAsync<T>(
        string itemId,
        string partitionKeyValue,
        CancellationToken cancellationToken = default
    )
    {
        var container = this._database.GetContainer(this._containerId);
        await container
            .DeleteItemAsync<CosmosItem<T>>(
                id: itemId,
                partitionKey: new PartitionKey(partitionKeyValue),
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
    }

    public async Task<ListResult<CosmosItem<T>>> ListAsync<T>(
        string type,
        string? partitionKeyValue = default,
        string? continuationToken = default,
        CancellationToken cancellationToken = default
    )
    {
        var queryDefinition = string.IsNullOrEmpty(partitionKeyValue)
            ? new QueryDefinition("SELECT * FROM c WHERE c.type = @type").WithParameter(
                "@type",
                type
            )
            : new QueryDefinition(
                "SELECT * FROM c WHERE c.partitionKey = @partitionKeyValue AND c.type = @type"
            )
                .WithParameter("@partitionKeyValue", partitionKeyValue)
                .WithParameter("@type", type);

        return await this.ListAsync<T>(
                queryDefinition: queryDefinition,
                continuationToken: continuationToken,
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
    }

    public async Task<ListResult<CosmosItem<T>>> ListAsync<T>(
        QueryDefinition queryDefinition,
        string? continuationToken = default,
        CancellationToken cancellationToken = default
    )
    {
        var container = this._database.GetContainer(this._containerId);
        using var feedIterator = container.GetItemQueryIterator<CosmosItem<T>>(
            queryDefinition: queryDefinition,
            continuationToken: continuationToken
        );

        var items = new List<CosmosItem<T>>();
        string? newContinuationToken = null;

        while (feedIterator.HasMoreResults)
        {
            var response = await feedIterator
                .ReadNextAsync(cancellationToken)
                .ConfigureAwait(false);
            newContinuationToken = response.ContinuationToken;

            foreach (var resource in response.Resource)
            {
                if (resource != null)
                {
                    items.Add(resource);
                }
            }
        }

        return new ListResult<CosmosItem<T>>(items: items, continuationToken: newContinuationToken);
    }
}
