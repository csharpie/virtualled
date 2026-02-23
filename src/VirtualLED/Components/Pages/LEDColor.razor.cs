using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using VirtualLED.Models;

namespace VirtualLED.Components.Pages;

public class LEDColorComponent : ComponentBase
{
    private HubConnection? hubConnection;

    [Inject]
    public required NavigationManager Navigation { get; set; }

    [Parameter]
    public string Color { get; set; } = "black";

    [Parameter]
    public string ChangedBy { get; set; } = "No one yet";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && OperatingSystem.IsBrowser())
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri("/ledcolorhub"))
            .Build();

            hubConnection.On("ReceiveLEDColor", (LEDColorChange change) =>
            {
                Color = change.Color;
                ChangedBy = change.ChangedBy;
                InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();
        }
    }
}
