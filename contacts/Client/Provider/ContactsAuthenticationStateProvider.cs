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

    // TODO! Handle bad token
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

        return new AuthenticationState(new ClaimsPrincipal(
            new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        throw new NotImplementedException();
    }
}