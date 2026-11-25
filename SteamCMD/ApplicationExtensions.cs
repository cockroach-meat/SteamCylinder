using SteamCylinder.Core;

namespace SteamCylinder.Services.SteamCMD;

public static class ApplicationExtensions
{
    public static ISteamCMD GetSteamCMD(this Application application)
    {
        return application.Services.Get<ISteamCMD>();
    }
}
