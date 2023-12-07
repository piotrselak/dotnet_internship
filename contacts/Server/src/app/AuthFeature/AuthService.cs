using contacts.Shared;
using Microsoft.AspNetCore.Identity;

namespace contacts.Server.AuthFeature;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> Register(RegisterRequest registerRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Login(LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }
}