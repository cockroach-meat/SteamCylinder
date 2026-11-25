using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SteamCylinder.Services.SteamCMD;

public sealed partial class SteamCMD : ISteamCMD
{
    public string Output { get; private set; } = string.Empty;
    public bool IsSteamGuardCodeNeeded { get; private set; } = false;

    private string RemoveAnsi(string input)
    {
        return Regex.Replace(input, @"\x1b\[\d+m", "");
    }

    private async Task ExecuteCommandAsync(string command)
    {
        Process process = new();
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.FileName = _executableFileName;

        process.StartInfo.Arguments = "+@sSteamCmdForcePlatformType " + (_platform == SteamCMDPlatform.Windows ? "windows" : "linux");

        if (!string.IsNullOrEmpty(_installDirectory))
        {
            process.StartInfo.Arguments += $" +force_install_dir \"{_installDirectory}\"";
        }

        if (_loggedIn)
        {
            process.StartInfo.Arguments += $" +login \"{_username}\" \"{_password}\"";
        }

        process.StartInfo.Arguments += $" +{command} +quit";

        Console.WriteLine("Running SteamCMD with " + process.StartInfo.Arguments);

        process.Start();
        await process.StandardInput.WriteLineAsync();
        await process.StandardInput.DisposeAsync();
        await process.WaitForExitAsync();

        Output = await process.StandardOutput.ReadToEndAsync();

        process.StandardInput.Close();
        process.StandardOutput.Close();
    }

    private bool OutputContains(params string[] queries)
    {
        foreach (string query in queries)
        {
            if (Output.Contains(query))
                return true;
        }

        return false;
    }
}
