using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Server.AuthFeature;

public interface IAuthService
{
    Task<Result<string>> Register(RegisterRequest registerRequest);
    Task<Result<string>> Login(LoginRequest loginRequest);
}