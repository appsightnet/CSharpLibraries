using Microsoft.Extensions.Options;

namespace AppSightNet.DataAccess.Azure.Cosmos.Stores;

public class CosmosStoreFactory : ICosmosStoreFactory
{
    private IOptionsMonitor<CosmosStoreOptions> _optionsMonitor { get; }

    public CosmosStoreFactory(IOptionsMonitor<CosmosStoreOptions> optionsMonitor)
    {
        this._optionsMonitor = optionsMonitor;
    }

    public ICosmosStore CreateStore(string storeName)
    {
        var storeOptions =
            this._optionsMonitor.Get(storeName)
            ?? throw new KeyNotFoundException(
                $"The {nameof(CosmosStoreOptions)} '{storeName}' is not registered."
            );

        if (storeOptions.Client == null)
        {
            throw new InvalidOperationException(
                $"The {nameof(CosmosStoreOptions.Client)} of {nameof(CosmosStoreOptions)} '{storeName}' is null."
            );
        }

        return new CosmosStore(storeOptions.Client, storeOptions);
    }
}
