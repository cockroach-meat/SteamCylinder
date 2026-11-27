using SteamCylinder.Core;
using SteamCylinder.Shared;

namespace SteamCylinder.Services.GameManager;

public interface IGameManager : IService
{
    Task<IEnumerable<SteamAppInfo>> GetInstalledGamesAsync();
    Task<SteamAppInfo> InstallSteamGameAsync(int appId);
}
