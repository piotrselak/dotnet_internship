using contacts.Server.Result;
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

    // TODO Check password strength
    public async Task<ServiceResult<string>> Register(RegisterRequest registerRequest)
    {
        // We don't have to check if UserName is null here as we do it in controller
        var exists =
            await _userManager.FindByNameAsync(registerRequest.UserName!) != null;
        if (exists)
            return new ServiceResult<string>
            {
                Succeeded = false,
                Error = ErrorType.AlreadyExists
            };
        User user = new User
        {
            UserName = registerRequest.UserName,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result =
            await _userManager.CreateAsync(user, registerRequest.Password!);

        if (!result.Succeeded)
            // TODO! Refactor service error handling as we can't see the messages
            return new ServiceResult<string>
            {
                Succeeded = false,
                Error = ErrorType.Other,
                Message = result.Errors.First().Description
            };

        return await Login(new LoginRequest
        {
            Username = registerRequest.UserName,
            Password = registerRequest.Password
        });
    }

    public async Task<ServiceResult<string>> Login(LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }
}