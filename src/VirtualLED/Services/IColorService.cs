using VirtualLED.Models;

namespace VirtualLED.Services;

public interface IColorService
{
    Task SetColor(LEDColorChange change);
}