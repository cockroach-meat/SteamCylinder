using SteamCylinder.Core;

namespace SteamCylinder.Services.SteamCMD;

public static class ApplicationBuilderExtensions
{
    public static ApplicationBuilder AddSteamCMD(this ApplicationBuilder builder)
    {
        builder.Services.AddSingletonService<ISteamCMD>(new SteamCMD());
        return builder;
    }
}
