namespace SteamCylinder.Core;

public class ApplicationBuilder
{
    public ServiceLocator Services { get; } = new();

    public Application Build()
    {
        Services.InitializeSingletonServices();
        return new Application(Services);
    }
}
