using contacts.Shared;
using contacts.Shared.Result;

namespace contacts.Client.Services;

public interface IAuthService
{
    Task<Result<string>> Login(LoginRequest request);
    Task<Result<string>> Register(RegisterRequest request);
    Task Logout();
}