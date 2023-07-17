using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppSightNet.DataAccess.Azure.Cosmos.Stores.Extensions;

public static class CosmosStoreServiceCollectionExtensions
{
    public static IServiceCollection AddCosmosStore(
        this IServiceCollection services,
        string storeName,
        Action<IServiceProvider, CosmosStoreOptions> configureOptions
    )
    {
        services.Configure<CosmosStoreOptions>(
            storeName,
            options =>
            {
                configureOptions(services.BuildServiceProvider(), options);
            }
        );
        services.TryAddSingleton<ICosmosStoreFactory, CosmosStoreFactory>();

        return services;
    }

    public static IServiceCollection AddCosmosStore(
        this IServiceCollection services,
        string storeName,
        Action<CosmosStoreOptions> configureOptions
    )
    {
        services.Configure<CosmosStoreOptions>(
            storeName,
            options =>
            {
                options.Client = services.BuildServiceProvider().GetRequiredService<CosmosClient>();
                configureOptions(options);
            }
        );
        services.TryAddSingleton<ICosmosStoreFactory, CosmosStoreFactory>();

        return services;
    }
}
