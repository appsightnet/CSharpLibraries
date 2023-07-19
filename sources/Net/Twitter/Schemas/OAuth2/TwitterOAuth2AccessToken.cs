using AppSightNet.Net.Twitter.Clients.OAuth2;

namespace AppSightNet.Net.Twitter.Schemas.OAuth2;

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

    public static TwitterOAuth2AccessToken FromRefreshTokenResult( // TODO: Migrate to extensions
        RefreshAccessTokenResult refreshTokenResult
    )
    {
        if (refreshTokenResult.TokenType != "bearer")
        {
            throw new ArgumentException(
                $"Invalid token type '{refreshTokenResult.TokenType}' was provided."
            );
        }

        if (string.IsNullOrEmpty(refreshTokenResult.AccessToken))
        {
            throw new ArgumentException(
                $"{nameof(refreshTokenResult.AccessToken)} must be provided."
            );
        }

        if (string.IsNullOrEmpty(refreshTokenResult.RefreshToken))
        {
            throw new ArgumentException(
                $"{nameof(refreshTokenResult.RefreshToken)} must be provided."
            );
        }

        if (string.IsNullOrEmpty(refreshTokenResult.Scope))
        {
            throw new ArgumentException($"{nameof(refreshTokenResult.Scope)} must be provided.");
        }

        return new TwitterOAuth2AccessToken(
            expiresAt: DateTimeOffset.Now + TimeSpan.FromSeconds(refreshTokenResult.ExpiresIn),
            token: refreshTokenResult.AccessToken,
            scope: refreshTokenResult.Scope,
            refreshToken: refreshTokenResult.RefreshToken
        );
    }
}
