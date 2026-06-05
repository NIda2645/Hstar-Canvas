namespace HstarCanvas.Core.Providers;

public sealed record ProviderPublicView(
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
    bool HasSecret,
    string? KeyPreview,
    string? SecretRef = null);
