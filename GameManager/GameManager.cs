using System.Text.Json;

using SteamCylinder.Core;
using SteamCylinder.Shared;
using SteamCylinder.Services.SteamCMD;

namespace SteamCylinder.Services.GameManager;

public sealed class GameManager : IGameManager
{
    private ISteamCMD steamCmd = null!;

    public void Initialize(ServiceLocator serviceLocator)
    {
        steamCmd = serviceLocator.Get<ISteamCMD>();
    }

    public async Task<IEnumerable<SteamAppInfo>> GetInstalledGamesAsync()
    {
        List<SteamAppInfo> apps = new();

        foreach (string directory in Directory.EnumerateDirectories(steamCmd.GetInstallDirectory()))
        {
            string appInfoPath = Path.Combine(steamCmd.GetInstallDirectory(), directory, "appinfo.json");

            if (File.Exists(appInfoPath))
            {
                apps.Add(JsonSerializer.Deserialize<SteamAppInfo>(await File.ReadAllTextAsync(appInfoPath))!);
            }
        }

        return apps;
    }

    public async Task<SteamAppInfo> InstallSteamGameAsync(int appId)
    {
        if (!(await steamCmd.UpdateAppAsync(appId)))
        {
            throw new GameManagerException("Game installation failed. SteamCMD output:" + Environment.NewLine + steamCmd.Output);
        }

        SteamAppInfo gameInfo = await steamCmd.GetAppInfoAsync(appId);

        await File.WriteAllTextAsync(Path.Combine(steamCmd.GetInstallDirectory(), appId.ToString(), "appinfo.json"), JsonSerializer.Serialize(gameInfo));

        return gameInfo;
    }
}
