using SteamCylinder.Core;
using SteamCylinder.Shared;

namespace SteamCylinder.Services.SteamCMD;

public enum SteamCMDPlatform
{
    Windows,
    Linux
}

public interface ISteamCMD : IService
{
    string Output { get; }
    bool IsSteamGuardCodeNeeded { get; }

    ISteamCMD SetExecutable(string fileName);
    Task<ISteamCMD> EnsureDownloadAsync();
    
    string GetInstallDirectory();
    ISteamCMD SetInstallDirectory(string installDirectory);
    ISteamCMD SetPlatform(SteamCMDPlatform platform);

    Task<bool> AuthenticateAnonymousAsync();
    Task<bool> AuthenticateAsync(string username, string password = "");
    Task<bool> SetSteamGuardCodeAsync(string code);

    Task<bool> UpdateAppAsync(int appId, bool validate = true);

    Task<SteamAppInfo> GetAppInfoAsync(int appId);
}
