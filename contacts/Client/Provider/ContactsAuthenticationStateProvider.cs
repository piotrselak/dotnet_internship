using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace contacts.Client.Provider;

public class ContactsAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public ContactsAuthenticationStateProvider(HttpClient httpClient,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public void MarkUserAsAuthenticated(string name)
    {
        var authenticatedUser = new ClaimsPrincipal(
            new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, name) },
                "apiauth"));
        var authState =
            Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("token");
        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", savedToken);

        IEnumerable<Claim> jwtClaims = ParseClaimsFromJwt(savedToken);

        return new AuthenticationState(new ClaimsPrincipal(
            new ClaimsIdentity(jwtClaims, "jwt")));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();

        string payload = jwt.Split(".")[1];
        byte[] parsedPayload = ParseBase64WithoutPadding(payload);

        var pairs =
            JsonSerializer.Deserialize<Dictionary<string, object>>(
                parsedPayload);

        // We can do simply this as there are no roles needed in this application
        claims.AddRange(pairs!.Select(kvp =>
            new Claim(kvp.Key, kvp.Value.ToString()!)));

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
}