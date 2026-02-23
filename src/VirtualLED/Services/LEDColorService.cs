using Microsoft.AspNetCore.SignalR;
using VirtualLED.Hubs;
using VirtualLED.Models;

namespace VirtualLED.Services;

public class LEDColorService : IColorService
{
    private readonly IHubContext<LEDColorHub> _hub;

    public LEDColorService(IHubContext<LEDColorHub> hub)
    {
        _hub = hub;
    }

    public Task SetColor(LEDColorChange change)
    {
        return _hub.Clients.All.SendAsync("ReceiveLEDColor", change);
    }
}