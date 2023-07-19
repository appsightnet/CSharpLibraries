namespace AppSightNet.Net.Twitter.Clients.OAuth2;

public static class TwitterOAuth2Utility
{
    public static string GenerateAuthorizeUrl(string clientId, string redirectUrl, string scope)
    {
        return $"https://twitter.com/i/oauth2/authorize"
            + $"?response_type=code"
            + $"&client_id={clientId}"
            + $"&redirect_uri={redirectUrl}"
            + $"&scope={Uri.EscapeDataString(scope)}"
            + $"&state=state"
            + $"&code_challenge=challenge"
            + $"&code_challenge_method=plain";
    }
}
