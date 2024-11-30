using FFMpegCore;

namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.libs;

public class LibraryLoader
{
    public static void LoadOpus()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var architecture = Environment.Is64BitProcess ? "x64" : "x86";
        var sourcePath = Path.Combine(basePath, "libs", architecture, "opus.dll");
        var destinationPath = Path.Combine(basePath, "opus.dll");

        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException($"The required Opus DLL is missing for {architecture}: {sourcePath}");
        }

        if (File.Exists(destinationPath))
        {
            File.Delete(destinationPath);
        }

        File.Copy(sourcePath, destinationPath);
    }

    public static void LoadFfmpeg() => GlobalFFOptions.Configure(new FFOptions { BinaryFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libs") });
}