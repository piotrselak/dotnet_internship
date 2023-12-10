using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using contacts.Shared;
using contacts.Shared.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames =
    Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace contacts.Server.AuthFeature;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    // TODO Check password strength
    public async Task<Result<string>> Register(RegisterRequest registerRequest)
    {
        // We don't have to check if UserName is null here as we do it in controller
        var exists =
            await _userManager.FindByNameAsync(registerRequest.Username!) !=
            null;
        if (exists)
            return new Result<string>
            {
                Succeeded = false,
                Error = new Error(409,
                    "User with given user name already exists")
            };
        User user = new User
        {
            UserName = registerRequest.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result =
            await _userManager.CreateAsync(user, registerRequest.Password!);

        if (!result.Succeeded)
        {
            var errors = result.Errors;
            string parsedErrors = errors.Aggregate("",
                (current, e) => current + (e.Description + ". "));

            return new Result<string>
            {
                Succeeded = false,
                Error = new Error(400, parsedErrors)
            };
        }


        return await Login(new LoginRequest
        {
            Username = registerRequest.Username,
            Password = registerRequest.Password
        });
    }

    public async Task<Result<string>> Login(LoginRequest loginRequest)
    {
        User? user = await _userManager.FindByNameAsync(loginRequest.Username!);

        if (user == null)
            return new Result<string>
            {
                Succeeded = false,
                Error = new Error(401,
                    "Username or password is incorrect.")
            };

        var passwordCheck =
            await _userManager.CheckPasswordAsync(user, loginRequest.Password!);
        if (!passwordCheck)
            return new Result<string>
            {
                Succeeded = false,
                Error = new Error(401,
                    "Username or password is incorrect.")
            };

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = GetToken(authClaims);
        return new Result<string>
        {
            Succeeded = true,
            Data = new JwtSecurityTokenHandler().WriteToken(token),
        };
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(2),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey,
                SecurityAlgorithms.HmacSha256));

        return token;
    }
}