namespace AppSightNet.Net.Twitter.Models;

public record class TwitterAuthSettings
{
    public string ClientId { get; init; } = string.Empty;

    public string ClientSecret { get; init; } = string.Empty;

    public string ApiKey { get; init; } = string.Empty;

    public string ApiKeySecret { get; init; } = string.Empty;

    public string BearerToken { get; init; } = string.Empty;

    public string RedirectUrl { get; init; } = string.Empty;

    public string Scope { get; init; } = string.Empty;

    public TwitterAuthSettings() { }

    public TwitterAuthSettings(
        string clientId,
        string clientSecret,
        string apiKey,
        string apiKeySecret,
        string bearerToken,
        string redirectUrl,
        string scope
    )
    {
        this.ClientId = clientId;
        this.ClientSecret = clientSecret;
        this.ApiKey = apiKey;
        this.ApiKeySecret = apiKeySecret;
        this.BearerToken = bearerToken;
        this.RedirectUrl = redirectUrl;
        this.Scope = scope;
    }
}
