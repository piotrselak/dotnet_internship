using contacts.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace contacts.Server.Auth;

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
        var response = await _authService.Login(loginRequest);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok(response.Data);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] LoginRequest registerRequest)
    {
        var response = await _authService.Register(registerRequest);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok(response.Data);
    }
}