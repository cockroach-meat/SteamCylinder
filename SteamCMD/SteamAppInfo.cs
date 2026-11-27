namespace SteamCylinder.Services.SteamCMD;

public class SteamAppInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Directory { get; set; } = string.Empty;
    public string ExecutableName { get; set; } = string.Empty;
    public string ExecutablePath { get => Path.Combine(Directory ?? string.Empty, ExecutableName ?? string.Empty); }
}
