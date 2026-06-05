using HstarCanvas.Core.Providers;

namespace HstarCanvas.Core.Tests;

public sealed class ProviderDefinitionTests
{
    [Fact]
    public void Create_DisablesProviderWhenEndpointIsBlank()
    {
        var provider = ProviderDefinition.Create(
            id: "empty",
            name: "Empty",
            protocol: ProviderProtocol.OpenAiCompatible,
            endpoint: "   ",
            enabled: true);

        Assert.False(provider.Enabled);
    }

    [Fact]
    public void Create_PreservesCapabilityAndModelLists()
    {
        var provider = ProviderDefinition.Create(
            id: "hstar-image",
            name: "Hstar Image",
            protocol: ProviderProtocol.OpenAiCompatible,
            endpoint: "https://api.example.test/v1/images",
            enabled: true,
            primary: true,
            capabilities: [ProviderCapability.Image, ProviderCapability.Chat],
            imageModels: ["gpt-image-1"],
            chatModels: ["gpt-5"],
            videoModels: []);

        Assert.True(provider.Enabled);
        Assert.True(provider.Primary);
        Assert.Contains(ProviderCapability.Image, provider.Capabilities);
        Assert.Equal(["gpt-image-1"], provider.ImageModels);
        Assert.Equal(["gpt-5"], provider.ChatModels);
    }

    [Fact]
    public void ToPublicView_RedactsSecretReference()
    {
        var provider = ProviderDefinition.Create(
            id: "secure",
            name: "Secure",
            protocol: ProviderProtocol.CustomHttp,
            endpoint: "https://api.example.test/generate",
            enabled: true,
            secretRef: "credential:secure");

        var view = provider.ToPublicView(hasSecret: true, keyPreview: "sk-...abcd");

        Assert.True(view.HasSecret);
        Assert.Equal("sk-...abcd", view.KeyPreview);
        Assert.Null(view.SecretRef);
    }
}
