namespace SteamCylinder.Core;

public class Application(ServiceLocator serviceLocator)
{
    public ServiceLocator Services { get; } = serviceLocator;
}
