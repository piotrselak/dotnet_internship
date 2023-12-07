using contacts.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace contacts.Server.AuthFeature;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (registerRequest.Password == null ||
            registerRequest.UserName == null)
            return BadRequest();
        
        var response = await _authService.Register(registerRequest);

        if (!response.Succeeded)
        {
            // TODO Error handling
            
            // return Ok(response.Message); // bad
        }

        return Ok(response.Data);
    }
}