namespace HstarCanvas.Native;

public sealed record AppPaths(
    string InstallRoot,
    string RoamingDataRoot,
    string LocalDataRoot,
    string DatabasePath,
    string AssetsRoot,
    string LogsRoot)
{
    public static AppPaths CreateDefault()
    {
        var installRoot = AppContext.BaseDirectory;
        var roamingRoot = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Hstar Canvas");
        var localRoot = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Hstar Canvas");

        return new AppPaths(
            installRoot,
            roamingRoot,
            localRoot,
            Path.Combine(localRoot, "hstar-canvas.db"),
            Path.Combine(localRoot, "Assets"),
            Path.Combine(localRoot, "Logs"));
    }

    public void EnsureCreated()
    {
        Directory.CreateDirectory(RoamingDataRoot);
        Directory.CreateDirectory(LocalDataRoot);
        Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath)!);
        Directory.CreateDirectory(AssetsRoot);
        Directory.CreateDirectory(LogsRoot);
    }
}
