using Microsoft.AspNetCore.SignalR;

namespace VirtualLED.Hubs;

public class LEDColorHub : Hub
{
    public async Task SetLEDColor(string color, string changedBy)
    {
        await Clients.All.SendAsync("ReceiveLEDColor", color, changedBy);
    }
}