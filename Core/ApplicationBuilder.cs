namespace SteamCylinder.Core;

public class ApplicationBuilder
{
    public ServiceLocator Services { get; } = new();

    public Application Build()
    {
        return new Application(Services);
    }
}
