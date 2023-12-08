using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;


    public async Task<Result<string>> Login(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> Register(RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}