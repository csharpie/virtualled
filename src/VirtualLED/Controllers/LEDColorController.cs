using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualLED.Models;
using VirtualLED.Services;

namespace VirtualLED.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LEDColorController : ControllerBase
{
    private readonly IColorService _service;

    public LEDColorController(IColorService service)
    {
        _service = service;
    }

    [HttpPost]
    [Produces("application/json")]
    public Task ChangeColor([FromBody] LEDColorChange change)
    {
        return _service.SetColor(change);
    }
}
