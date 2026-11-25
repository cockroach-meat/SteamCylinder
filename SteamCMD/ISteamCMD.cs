using SteamCylinder.Core;

namespace SteamCylinder.Services.SteamCMD;

public enum SteamCMDPlatform
{
    Windows,
    Linux
}

public interface ISteamCMD : IService
{
    List<string> Output { get; }
    bool IsSteamGuardCodeNeeded { get; }

    Task<ISteamCMD> StartAsync();

    ISteamCMD SetExecutable(string fileName, string arguments = "");
    Task<ISteamCMD> EnsureDownloadAsync();
    
    Task<ISteamCMD> SetInstallDirectory(string installDirectory);
    Task<ISteamCMD> SetPlatform(SteamCMDPlatform platform);

    Task<bool> AuthenticateAnonymousAsync();
    Task<bool> AuthenticateAsync(string username, string password = "");
    Task<bool> SetSteamGuardCodeAsync(string code);

    Task<bool> UpdateAppAsync(int appId, bool validate = true);
}
