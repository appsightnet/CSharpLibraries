using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppSightNet.Net.Twitter.Clients.OAuth2;

public class TwitterOAuth2IssueTokenResponse
{
    [JsonPropertyName("token_type")]
    [Required]
    public string TokenType { get; set; } = string.Empty;

    [JsonPropertyName("expires_in")]
    [Required]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("access_token")]
    [Required]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("scope")]
    [Required]
    public string Scope { get; set; } = string.Empty;

    [JsonPropertyName("refresh_token")]
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
