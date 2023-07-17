namespace AppSightNet.DataAccess.Azure.Cosmos.Stores;

public interface ICosmosStoreFactory
{
    ICosmosStore CreateStore(string storeName);
}
