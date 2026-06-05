using HstarCanvas.Core.Providers;
using HstarCanvas.Persistence.Database;
using HstarCanvas.Persistence.Providers;

namespace HstarCanvas.Persistence.Tests;

public sealed class ProviderSettingsStoreTests
{
    [Fact]
    public async Task SaveAsync_PersistsProviderConfiguration()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        var factory = new SqliteConnectionFactory(databasePath);
        await new DatabaseMigrator(factory).MigrateAsync(CancellationToken.None);
        var store = new ProviderSettingsStore(factory);
        var provider = ProviderDefinition.Create(
            id: "openai-image",
            name: "OpenAI Image",
            protocol: ProviderProtocol.OpenAiCompatible,
            endpoint: "https://api.example.test/v1/images",
            enabled: true,
            primary: true,
            capabilities: [ProviderCapability.Image],
            imageModels: ["gpt-image-1"],
            secretRef: "credential:openai-image");

        await store.SaveAsync(provider, CancellationToken.None);

        var providers = await store.ListAsync(CancellationToken.None);
        var saved = Assert.Single(providers);
        Assert.Equal("openai-image", saved.Id);
        Assert.True(saved.Enabled);
        Assert.True(saved.Primary);
        Assert.Equal(["gpt-image-1"], saved.ImageModels);
        Assert.Equal("credential:openai-image", saved.SecretRef);
    }
}
