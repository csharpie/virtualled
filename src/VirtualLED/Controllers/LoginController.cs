using Microsoft.AspNetCore.Mvc;
using VirtualLED.Models;
using VirtualLED.Services;

namespace VirtualLED.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public LoginController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(LoginResponse), 201)]
    [ProducesResponseType(201)]
    public IActionResult Login([FromBody] Login model)
    {
        if (model.UserName != Environment.GetEnvironmentVariable("AuthorizedUser") || model.Password != Environment.GetEnvironmentVariable("AuthorizedUserPassword"))
            return Unauthorized();

        var token = _tokenService.GenerateToken();
        var response = new LoginResponse { Token = token };
        return Created("", response);
    }
}
