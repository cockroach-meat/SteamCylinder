using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SteamCylinder.Services.SteamCMD;

public sealed partial class SteamCMD : ISteamCMD
{
    private Process _process = new();
    private bool _ready = false;

    public List<string> Output { get; } = new();
    public bool IsSteamGuardCodeNeeded { get; private set; } = false;

    private string RemoveAnsi(string input)
    {
        return Regex.Replace(input, @"\x1b\[\d+m", "");
    }

    private async void StartProcess()
    {
        _process.StartInfo.FileName = _executableFileName;
        _process.StartInfo.Arguments = _executableArguments;
        _process.StartInfo.RedirectStandardInput = true;
        _process.StartInfo.RedirectStandardOutput = true;

        _process.OutputDataReceived += (_, eventArgs) =>
        {
            string line = RemoveAnsi(eventArgs.Data ?? string.Empty).Trim();

            _ready = false;
            IsSteamGuardCodeNeeded = false;

            if (line == "Steam>")
            {
                _ready = true;
            }
            else if (line == "Steam Guard code:")
            {
                _ready = true;
                IsSteamGuardCodeNeeded = true;
            }
            else
            {
                Output.Add(line);
            }
        };

        _process.Start();
        _process.StandardInput.WriteLine();
    }

    private async Task WaitForReadyAsync()
    {
        while (!_ready)
        {
            await Task.Delay(50);
        }
    }

    private async Task ExecuteCommandAsync(string command)
    {
        Output.Clear();
        _process.StandardInput.WriteLine(command);

        await WaitForReadyAsync();
    }

    private bool OutputContains(params string[] queries)
    {
        foreach (string line in Output)
        {
            foreach (string query in queries)
            {
                if (line.Contains(query))
                    return true;
            }
        }

        return false;
    }
}
