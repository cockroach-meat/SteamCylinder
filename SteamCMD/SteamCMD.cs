using System.Reflection;

using SteamCylinder.Core;

namespace SteamCylinder.Services.SteamCMD;

public sealed partial class SteamCMD : ISteamCMD
{
    private readonly string _steamCmdStandardLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "steamcmd");

    private string _executableFileName = string.Empty;
    private string _executableArguments = string.Empty;

    public void Initialize(ServiceLocator serviceLocator)
    {
    
    }

    public async Task<ISteamCMD> StartAsync()
    {
        StartProcess();
        await WaitForReadyAsync();

        return this;
    }

    public async Task<ISteamCMD> SetInstallDirectory(string installDirectory)
    {
        await ExecuteCommandAsync($"force_install_dir \"{installDirectory}\"");

        return this;
    }

    public async Task<ISteamCMD> SetPlatform(SteamCMDPlatform platform)
    {
        await ExecuteCommandAsync($"@sSteamCmdForcePlatformType {(platform == SteamCMDPlatform.Linux ? "linux" : "windows")}");

        return this;
    }

    public async Task<bool> AuthenticateAnonymousAsync()
    {
        await ExecuteCommandAsync("login anonymous");

        return !OutputContains("ERROR");
    }

    public async Task<bool> AuthenticateAsync(string username, string password = "")
    {
        await ExecuteCommandAsync($"login \"{username}\"{(string.IsNullOrEmpty(password) ? string.Empty : $" \"{password}\"")}");

        return !OutputContains("ERROR");
    }

    public async Task<bool> SetSteamGuardCodeAsync(string code)
    {
        await ExecuteCommandAsync(code);

        return !OutputContains("ERROR", "invalid");
    }

    public async Task<bool> UpdateAppAsync(int appId, bool validate = true)
    {
        await ExecuteCommandAsync($"app_update {appId}{(validate ? " validate" : string.Empty)}");
        
        return OutputContains("Success");
    }
}
