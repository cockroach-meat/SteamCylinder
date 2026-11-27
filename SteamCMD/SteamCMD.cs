using System.Reflection;

using SteamCylinder.Core;

namespace SteamCylinder.Services.SteamCMD;

public sealed partial class SteamCMD : ISteamCMD
{
    private readonly string _steamCmdStandardLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "steamcmd");

    private string _executableFileName = string.Empty;

    private string _installDirectory = string.Empty;
    private SteamCMDPlatform _platform = OperatingSystem.IsWindows() ? SteamCMDPlatform.Windows : SteamCMDPlatform.Linux;

    private bool _loggedIn = false;
    private string _username = string.Empty;
    private string _password = string.Empty;

    public void Initialize(ServiceLocator serviceLocator)
    {
    
    }

    public string GetInstallDirectory() => _installDirectory;

    public ISteamCMD SetInstallDirectory(string installDirectory)
    {
        _installDirectory = installDirectory;

        if (!Directory.Exists(installDirectory))
        {
            Directory.CreateDirectory(installDirectory);
        }

        return this;
    }

    public ISteamCMD SetPlatform(SteamCMDPlatform platform)
    {
        _platform = platform;

        return this;
    }

    public async Task<bool> AuthenticateAnonymousAsync()
    {
        _username = "anonymous";
        _loggedIn = true;

        return true;
    }

    public async Task<bool> AuthenticateAsync(string username, string password = "")
    {
        _username = username;
        _password = password;

        await ExecuteCommandAsync($"login \"{username}\" \"{password}\"");

        if (OutputContains("Steam Guard code:"))
        {
            IsSteamGuardCodeNeeded = true;
            return false;
        }

        if (OutputContains("ERROR"))
        {
            return false;
        }

        _loggedIn = true;
        return true;
    }

    public async Task<bool> SetSteamGuardCodeAsync(string code)
    {
        await ExecuteCommandAsync($"set_steam_guard_code \"{code}\" +login \"{_username}\" \"{_password}\"");

        if (OutputContains("ERROR", "invalid"))
        {
            IsSteamGuardCodeNeeded = false;
            _loggedIn = true;
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateAppAsync(int appId, bool validate = true)
    {
        await ExecuteCommandAsync($"app_update {appId}{(validate ? " validate" : string.Empty)}", $"force_install_dir \"{Path.Combine(_installDirectory, appId.ToString())}\"");
        
        return OutputContains("Success");
    }
}
