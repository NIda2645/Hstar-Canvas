using HstarCanvas.Native;

namespace HstarCanvas.Native.Tests;

public sealed class AppPathsTests
{
    [Fact]
    public void CreateDefault_SeparatesRoamingAndLocalData()
    {
        var paths = AppPaths.CreateDefault();

        Assert.Contains("Hstar Canvas", paths.RoamingDataRoot);
        Assert.Contains("Hstar Canvas", paths.LocalDataRoot);
        Assert.NotEqual(paths.RoamingDataRoot, paths.LocalDataRoot);
        Assert.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), paths.RoamingDataRoot);
        Assert.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), paths.LocalDataRoot);
    }

    [Fact]
    public void CreateDefault_PlacesDatabaseAssetsAndLogsUnderLocalData()
    {
        var paths = AppPaths.CreateDefault();

        Assert.StartsWith(paths.LocalDataRoot, paths.DatabasePath);
        Assert.StartsWith(paths.LocalDataRoot, paths.AssetsRoot);
        Assert.StartsWith(paths.LocalDataRoot, paths.LogsRoot);
        Assert.EndsWith("hstar-canvas.db", paths.DatabasePath);
    }

    [Fact]
    public void EnsureCreated_CreatesRequiredDirectories()
    {
        var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var paths = new AppPaths(
            InstallRoot: Path.Combine(root, "Program"),
            RoamingDataRoot: Path.Combine(root, "Roaming"),
            LocalDataRoot: Path.Combine(root, "Local"),
            DatabasePath: Path.Combine(root, "Local", "hstar-canvas.db"),
            AssetsRoot: Path.Combine(root, "Local", "Assets"),
            LogsRoot: Path.Combine(root, "Local", "Logs"));

        paths.EnsureCreated();

        Assert.True(Directory.Exists(paths.RoamingDataRoot));
        Assert.True(Directory.Exists(paths.LocalDataRoot));
        Assert.True(Directory.Exists(paths.AssetsRoot));
        Assert.True(Directory.Exists(paths.LogsRoot));
    }
}
