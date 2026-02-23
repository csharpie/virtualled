using Microsoft.AspNetCore.SignalR;
using VirtualLED.Models;

namespace VirtualLED.Hubs;

public class LEDColorHub : Hub
{
    public async Task SetLEDColor(LEDColorChange change)
    {
        await Clients.All.SendAsync("ReceiveLEDColor", change);
    }
}
