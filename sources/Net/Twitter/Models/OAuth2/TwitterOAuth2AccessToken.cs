using AppSightNet.Net.Twitter.Clients.OAuth2;

namespace AppSightNet.Net.Twitter.Models.OAuth2;

public record class TwitterOAuth2AccessToken
{
    public DateTimeOffset ExpiresAt { get; init; }

    public string Token { get; init; } = string.Empty;

    public string Scope { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public TwitterOAuth2AccessToken() { }

    public TwitterOAuth2AccessToken(
        DateTimeOffset expiresAt,
        string token,
        string scope,
        string refreshToken
    )
    {
        this.ExpiresAt = expiresAt;
        this.Token = token;
        this.Scope = scope;
        this.RefreshToken = refreshToken;
    }

    public static TwitterOAuth2AccessToken FromRefreshTokenResponse( // TODO: Migrate to extensions
        RefreshAccessTokenResponse response
    )
    {
        if (response.TokenType != "bearer")
        {
            throw new ArgumentException($"Invalid token type '{response.TokenType}' was provided.");
        }

        if (string.IsNullOrEmpty(response.AccessToken))
        {
            throw new ArgumentException($"{nameof(response.AccessToken)} must be provided.");
        }

        if (string.IsNullOrEmpty(response.RefreshToken))
        {
            throw new ArgumentException($"{nameof(response.RefreshToken)} must be provided.");
        }

        if (string.IsNullOrEmpty(response.Scope))
        {
            throw new ArgumentException($"{nameof(response.Scope)} must be provided.");
        }

        return new TwitterOAuth2AccessToken(
            expiresAt: DateTimeOffset.Now + TimeSpan.FromSeconds(response.ExpiresIn),
            token: response.AccessToken,
            scope: response.Scope,
            refreshToken: response.RefreshToken
        );
    }
}
