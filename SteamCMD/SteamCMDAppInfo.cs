using Gameloop.Vdf;

namespace SteamCylinder.Services.SteamCMD;

public sealed partial class SteamCMD : ISteamCMD
{
    public async Task<SteamAppInfo> GetAppInfoAsync(int appId)
    {
        await ExecuteCommandAsync($"app_info_print {appId}");

        int startIndex = Output.IndexOf(Environment.NewLine, Output.IndexOf("AppID"));
        int endIndex = Output.IndexOf("Unloading Steam API");

        dynamic data = VdfConvert.Deserialize(Output.Substring(startIndex, endIndex - startIndex));

        return new()
        {
            Id = appId,
            Name = data.Value.common.name.ToString(),
            Developer = data.Value.extended.developer.ToString(),
            Publisher = data.Value.extended.publisher.ToString(),
            Directory = Path.Combine(_installDirectory, appId.ToString()),
            ExecutableName = data.Value.config.launch[0].Value.executable.ToString()
        };
    }
}
