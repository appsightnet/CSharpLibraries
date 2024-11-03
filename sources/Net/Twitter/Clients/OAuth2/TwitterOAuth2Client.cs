using AppSightNet.Net.Twitter.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace AppSightNet.Net.Twitter.Clients.OAuth2;

public class TwitterOAuth2Client
{
    private HttpClient _httpClient { get; }

    public TwitterOAuth2Client(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<TwitterOAuth2IssueTokenResponse> IssueAccessTokenAsync(
        string clientId,
        string clientSecret,
        string redirectUrl,
        string code,
        CancellationToken cancellationToken = default
    )
    {
        var url = "oauth2/token";
        using var content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                { "code", code },
                { "grant_type", "authorization_code" },
                { "client_id", clientId },
                { "redirect_uri", redirectUrl },
                { "code_verifier", "challenge" }
            }
        );
        var base64EncodedCredentials = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")
        );

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        requestMessage.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                base64EncodedCredentials
            );
        requestMessage.Content = content;

        using var response = await this._httpClient
            .SendAsync(requestMessage, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var tokenResponse =
            JsonSerializer.Deserialize<TwitterOAuth2IssueTokenResponse>(
                await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)
            )
            ?? throw new JsonException(
                $"Result of deserialization to '{nameof(TwitterOAuth2IssueTokenResponse)}' was null."
            );

        var validationContext = new ValidationContext(tokenResponse);
        Validator.ValidateObject(tokenResponse, validationContext);

        return tokenResponse;
    }

    public async Task<TwitterOAuth2IssueTokenResponse> RefreshAccessTokenAsync(
        string clientId,
        string clientSecret,
        string refreshToken,
        CancellationToken cancellationToken = default
    )
    {
        var url = "oauth2/token";
        var content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                { "refresh_token", refreshToken },
                { "grant_type", "refresh_token" },
                { "client_id", clientId }
            }
        );
        var base64EncodedCredentials = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")
        );

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        requestMessage.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                base64EncodedCredentials
            );
        requestMessage.Content = content;

        using var response = await this._httpClient
            .SendAsync(requestMessage, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var tokenResponse =
            JsonSerializer.Deserialize<TwitterOAuth2IssueTokenResponse>(
                await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)
            )
            ?? throw new JsonException(
                $"Result of deserialization to '{nameof(TwitterOAuth2IssueTokenResponse)}' was null."
            );

        var validationContext = new ValidationContext(tokenResponse);
        Validator.ValidateObject(tokenResponse, validationContext);

        return tokenResponse;
    }
}
