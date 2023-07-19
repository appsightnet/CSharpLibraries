namespace AppSightNet.DataAccess.Azure.Cosmos.Stores.Results;

public record class ListResult<TItem>
{
    public ListResult() { }

    public ListResult(IList<TItem> items, string? continuationToken = default)
    {
        this.Items = items;
        this.ContinuationToken = continuationToken;
    }

    public IList<TItem> Items { get; init; } = new List<TItem>();

    public string? ContinuationToken { get; init; }
}
