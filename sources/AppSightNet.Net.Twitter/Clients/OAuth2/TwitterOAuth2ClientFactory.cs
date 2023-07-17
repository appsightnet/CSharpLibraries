using AppSightNet.Net.Twitter.Clients.Api.V2;

namespace AppSightNet.Net.Twitter.Clients.OAuth2;

public class TwitterOAuth2ClientFactory
{
    private IHttpClientFactory _httpClientFactory { get; }

    public TwitterOAuth2ClientFactory(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
    }

    public TwitterOAuth2Client CreateClient()
    {
        var httpClient = this._httpClientFactory.CreateClient();
        httpClient.BaseAddress = Constants.ApiBaseUri;
        return new TwitterOAuth2Client(httpClient);
    }
}
