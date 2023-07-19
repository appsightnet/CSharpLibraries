namespace AppSightNet.Net.Twitter.Clients.Api.V2;

public class TwitterApiClientFactory
{
    private IHttpClientFactory _httpClientFactory { get; }

    public TwitterApiClientFactory(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
    }

    public TwitterApiClient CreateClient(string accessToken)
    {
        var httpClient = this._httpClientFactory.CreateClient();
        httpClient.BaseAddress = Constants.ApiBaseUri;
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        return new TwitterApiClient(httpClient);
    }
}
