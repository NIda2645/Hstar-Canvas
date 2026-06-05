namespace HstarCanvas.Core.Providers;

public sealed record ProviderDefinition(
    string Id,
    string Name,
    ProviderProtocol Protocol,
    string Endpoint,
    bool Enabled,
    bool Primary,
    IReadOnlyList<ProviderCapability> Capabilities,
    IReadOnlyList<string> ImageModels,
    IReadOnlyList<string> ChatModels,
    IReadOnlyList<string> VideoModels,
    string? SecretRef)
{
    public static ProviderDefinition Create(
        string id,
        string name,
        ProviderProtocol protocol,
        string endpoint,
        bool enabled,
        bool primary = false,
        IReadOnlyList<ProviderCapability>? capabilities = null,
        IReadOnlyList<string>? imageModels = null,
        IReadOnlyList<string>? chatModels = null,
        IReadOnlyList<string>? videoModels = null,
        string? secretRef = null)
    {
        var normalizedEndpoint = endpoint.Trim();
        return new ProviderDefinition(
            id.Trim(),
            name.Trim(),
            protocol,
            normalizedEndpoint,
            enabled && normalizedEndpoint.Length > 0,
            primary,
            capabilities ?? [],
            imageModels ?? [],
            chatModels ?? [],
            videoModels ?? [],
            secretRef);
    }

    public ProviderPublicView ToPublicView(bool hasSecret, string? keyPreview)
        => new(Id, Name, Protocol, Endpoint, Enabled, Primary, Capabilities, ImageModels, ChatModels, VideoModels, hasSecret, keyPreview);
}
