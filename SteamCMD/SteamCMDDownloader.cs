using System.Formats.Tar;
using System.IO.Compression;

namespace SteamCylinder.Services.SteamCMD;

public sealed partial class SteamCMD : ISteamCMD
{
    private string GetArchiveURL()
    {
        if (OperatingSystem.IsWindows())
        {
            return "https://client-update.steamstatic.com/installer/steamcmd.zip";
        }

        if (OperatingSystem.IsLinux())
        {
            return "https://client-update.steamstatic.com/installer/steamcmd_linux.tar.gz";
        }

        throw new PlatformNotSupportedException("SteamCMD is supported only on Windows and Linux");
    }

    private string GetArchivePath()
    {
        if (OperatingSystem.IsWindows())
        {
            return Path.Combine(Path.GetTempPath(), "steamcmd.zip");
        }

        if (OperatingSystem.IsLinux())
        {
            return Path.Combine(Path.GetTempPath(), "steamcmd.tar.gz");
        }
        
        throw new PlatformNotSupportedException("SteamCMD is supported only on Windows and Linux");
    }

    private string GetExecutableFileName()
    {
        if (OperatingSystem.IsWindows())
        {
            return "steamcmd.exe";
        }

        if (OperatingSystem.IsLinux())
        {
            return "steamcmd.sh";
        }

        throw new PlatformNotSupportedException("SteamCMD is supported only on Windows and Linux");
    }

    private string GetExecutablePath()
    {
        return Path.Combine(_steamCmdStandardLocation, GetExecutableFileName());
    }

    private async Task ExtractArchiveAsync(string archiveFileName)
    {
        if (OperatingSystem.IsWindows())
        {
            await ZipFile.ExtractToDirectoryAsync(archiveFileName, _steamCmdStandardLocation);

            return;
        }

        if (OperatingSystem.IsLinux())
        {
            using FileStream sourceStream = File.OpenRead(archiveFileName);
            using GZipStream stream = new(sourceStream, CompressionMode.Decompress);

            await TarFile.ExtractToDirectoryAsync(stream, _steamCmdStandardLocation, true, CancellationToken.None);

            string executablePath = GetExecutablePath();
            UnixFileMode mode = File.GetUnixFileMode(executablePath);
            File.SetUnixFileMode(executablePath, mode | UnixFileMode.UserExecute);

            return;
        }

        throw new PlatformNotSupportedException("SteamCMD is supported only on Windows and Linux");
    }


    public ISteamCMD SetExecutable(string fileName)
    {
        _executableFileName = fileName;
        return this;
    }

    public async Task<ISteamCMD> EnsureDownloadAsync()
    {
        string executablePath = GetExecutablePath();
        string archivePath = GetArchivePath();

        _executableFileName = executablePath;

        if (File.Exists(executablePath))
        {
            return this;
        }

        if (File.Exists(archivePath))
        {
            File.Delete(archivePath);
        }

        Directory.CreateDirectory(_steamCmdStandardLocation);

        using HttpClient httpClient = new();
        await File.WriteAllBytesAsync(archivePath, await httpClient.GetByteArrayAsync(GetArchiveURL()));

        if (!File.Exists(archivePath))
        {
            throw new InvalidOperationException("Archive not found after download completed");
        }

        await ExtractArchiveAsync(archivePath);

        if (!File.Exists(executablePath))
        {
            File.Delete(archivePath);
            throw new InvalidOperationException("Executable not found after archive extraction");
        }

        File.Delete(archivePath);

        return this;
    }
}
