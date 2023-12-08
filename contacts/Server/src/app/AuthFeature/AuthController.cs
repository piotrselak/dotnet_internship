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
        if (string.IsNullOrEmpty(loginRequest.Username) ||
            string.IsNullOrEmpty(loginRequest.Password))
            return BadRequest("Password or username cannot be empty.");

        var response = await _authService.Login(loginRequest);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok(response.Data);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest registerRequest)
    {
        if (registerRequest.Password == null ||
            registerRequest.Username == null)
            return BadRequest("Password or username cannot be empty.");

        var response = await _authService.Register(registerRequest);

        if (response is { Succeeded: false, Error: not null })
            return Problem(detail: response.Error.Description,
                statusCode: response.Error.Code);

        return Ok(response.Data);
    }
}