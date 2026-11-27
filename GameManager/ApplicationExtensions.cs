using SteamCylinder.Core;

namespace SteamCylinder.Services.GameManager;

public static class ApplicationExtensions
{
    public static IGameManager GetGameManager(this Application application)
    {
        return application.Services.Get<IGameManager>();
    }
}
