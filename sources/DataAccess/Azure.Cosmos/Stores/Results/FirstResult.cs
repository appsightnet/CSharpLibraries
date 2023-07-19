namespace AppSightNet.DataAccess.Azure.Cosmos.Stores.Results;

public record class FirstResult<TItem>
{
    public FirstResult(TItem item)
    {
        this.Item = item;
    }

    public TItem Item { get; init; }
}
