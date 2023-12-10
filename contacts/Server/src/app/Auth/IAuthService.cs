using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Server.Auth;

public interface IAuthService
{
    Task<Result<string>> Register(LoginRequest registerRequest);
    Task<Result<string>> Login(LoginRequest loginRequest);
}