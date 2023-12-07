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
    public async Task<Result<string>> Register(RegisterRequest registerRequest)
    {
        // We don't have to check if UserName is null here as we do it in controller
        var exists =
            await _userManager.FindByNameAsync(registerRequest.UserName!) != null;
        if (exists)
            return new Result<string>
            {
                Succeeded = false,
                Error = new Error(409,
                    "User with given user name already exists")
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
            return new Result<string>
            {
                Succeeded = false,
                Error = new Error(400,
                    "User name or password incorrect")
            };

        return await Login(new LoginRequest
        {
            Username = registerRequest.UserName,
            Password = registerRequest.Password
        });
    }

    public async Task<Result<string>> Login(LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }
}