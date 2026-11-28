using SteamCylinder.Core;

namespace SteamCylinder.Services.GameManager;

public static class ApplicationBuilderExtensions
{
    public static ApplicationBuilder AddGameManager(this ApplicationBuilder builder)
    {
        builder.Services.AddSingletonService<IGameManager>(new GameManager());
        return builder;
    }
}
