namespace AppSightNet.DataAccess.Azure.Cosmos.Stores.Results;

public record class GetResult<TItem>
{
    public GetResult(TItem item)
    {
        this.Item = item;
    }

    public TItem Item { get; init; }
}
