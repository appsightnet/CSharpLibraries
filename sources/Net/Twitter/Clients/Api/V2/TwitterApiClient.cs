using System.Text.Json;
using System.Text;

namespace AppSightNet.Net.Twitter.Clients.Api.V2;

public class TwitterApiClient
{
    private HttpClient _httpClient { get; }

    public TwitterApiClient(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<CreateTweetResponse> CreateTweetAsync(
        string message,
        CancellationToken cancellationToken = default
    )
    {
        var contentJson = JsonSerializer.Serialize(new { text = message });
        using var content = new StringContent(contentJson, Encoding.UTF8, "application/json");
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "tweets");
        requestMessage.Content = content;

        using var response = await this._httpClient
            .SendAsync(requestMessage, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var responseContentJson = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        return JsonSerializer.Deserialize<CreateTweetResponse>(responseContentJson)
            ?? throw new JsonException(
                $"Result of deserialization to '{nameof(CreateTweetResponse)}' was null."
            );
    }
}
