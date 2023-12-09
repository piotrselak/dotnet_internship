using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using contacts.Client.Domain;
using contacts.Client.Provider;
using contacts.Shared;
using contacts.Shared.Result;
using Microsoft.AspNetCore.Components.Authorization;

namespace contacts.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<AuthService> _logger;

    public AuthService(HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage, ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _logger = logger;
    }

    // Both methods log user in
    public async Task<Result<string>> Login(LoginRequest request)
    {
        var stringContent =
            new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

        var response =
            await _httpClient.PostAsync("api/Auth/login", stringContent);

        if (!response.IsSuccessStatusCode)
            return new Result<string>
            {
                Succeeded = false,
                Error = new Error((int)response.StatusCode,
                    (await response.Content.ReadFromJsonAsync<ErrorResponse>())!
                    .Detail)
            };

        var result = new Result<string>
        {
            Succeeded = true,
            Data = await response.Content.ReadAsStringAsync()
        };


        await _localStorage.SetItemAsync("token", result.Data);
        ((ContactsAuthenticationStateProvider)_authenticationStateProvider)
            .MarkUserAsAuthenticated(request.Username!);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", result.Data);

        return result;
    }

    public async Task<Result<string>> Register(LoginRequest request)
    {
        var stringContent = new StringContent(
            JsonSerializer.Serialize(request), Encoding.UTF8,
            "application/json");

        var res = await _httpClient.PostAsync(
            "api/Auth/register", stringContent);

        if (!res.IsSuccessStatusCode)
            return new Result<string>
            {
                Succeeded = false,
                Error = new Error((int)res.StatusCode,
                    (await res.Content.ReadFromJsonAsync<ErrorResponse>())!
                    .Detail)
            };

        var result = new Result<string>
        {
            Succeeded = true,
            Data = await res.Content.ReadAsStringAsync()
        };

        await _localStorage.SetItemAsync("token", result.Data);
        ((ContactsAuthenticationStateProvider)_authenticationStateProvider)
            .MarkUserAsAuthenticated(request.Username!);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", result.Data);


        return result;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("token");
        ((ContactsAuthenticationStateProvider)_authenticationStateProvider)
            .MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}