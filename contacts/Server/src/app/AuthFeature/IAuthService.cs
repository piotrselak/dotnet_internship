using contacts.Shared;

namespace contacts.Server.AuthFeature;

public interface IAuthService
{
    Task<string> Register(RegisterRequest registerRequest);
    Task<string> Login(LoginRequest loginRequest);
}