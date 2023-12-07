using contacts.Server.Result;
using contacts.Shared;

namespace contacts.Server.AuthFeature;

public interface IAuthService
{
    Task<ServiceResult<string>> Register(RegisterRequest registerRequest);
    Task<ServiceResult<string>> Login(LoginRequest loginRequest);
}