using System.Text.Json;
using HstarCanvas.Core.Providers;
using HstarCanvas.Persistence.Database;

namespace HstarCanvas.Persistence.Providers;

public sealed class ProviderSettingsStore
{
    private readonly SqliteConnectionFactory _factory;

    public ProviderSettingsStore(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task SaveAsync(ProviderDefinition provider, CancellationToken cancellationToken)
    {
        await using var connection = await _factory.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
INSERT INTO providers (id, name, protocol, endpoint, enabled, primary_provider, capabilities_json, image_models_json, chat_models_json, video_models_json, secret_ref, updated_at)
VALUES ($id, $name, $protocol, $endpoint, $enabled, $primary, $capabilities, $imageModels, $chatModels, $videoModels, $secretRef, strftime('%Y-%m-%dT%H:%M:%fZ', 'now'))
ON CONFLICT(id) DO UPDATE SET
    name = excluded.name,
    protocol = excluded.protocol,
    endpoint = excluded.endpoint,
    enabled = excluded.enabled,
    primary_provider = excluded.primary_provider,
    capabilities_json = excluded.capabilities_json,
    image_models_json = excluded.image_models_json,
    chat_models_json = excluded.chat_models_json,
    video_models_json = excluded.video_models_json,
    secret_ref = excluded.secret_ref,
    updated_at = excluded.updated_at;
""";
        command.Parameters.AddWithValue("$id", provider.Id);
        command.Parameters.AddWithValue("$name", provider.Name);
        command.Parameters.AddWithValue("$protocol", provider.Protocol.ToString());
        command.Parameters.AddWithValue("$endpoint", provider.Endpoint);
        command.Parameters.AddWithValue("$enabled", provider.Enabled ? 1 : 0);
        command.Parameters.AddWithValue("$primary", provider.Primary ? 1 : 0);
        command.Parameters.AddWithValue("$capabilities", JsonSerializer.Serialize(provider.Capabilities.Select(x => x.ToString())));
        command.Parameters.AddWithValue("$imageModels", JsonSerializer.Serialize(provider.ImageModels));
        command.Parameters.AddWithValue("$chatModels", JsonSerializer.Serialize(provider.ChatModels));
        command.Parameters.AddWithValue("$videoModels", JsonSerializer.Serialize(provider.VideoModels));
        command.Parameters.AddWithValue("$secretRef", (object?)provider.SecretRef ?? DBNull.Value);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProviderDefinition>> ListAsync(CancellationToken cancellationToken)
    {
        await using var connection = await _factory.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, name, protocol, endpoint, enabled, primary_provider, capabilities_json, image_models_json, chat_models_json, video_models_json, secret_ref FROM providers ORDER BY id;";
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var providers = new List<ProviderDefinition>();
        while (await reader.ReadAsync(cancellationToken))
        {
            providers.Add(ProviderDefinition.Create(
                reader.GetString(0),
                reader.GetString(1),
                Enum.Parse<ProviderProtocol>(reader.GetString(2)),
                reader.GetString(3),
                reader.GetInt32(4) == 1,
                reader.GetInt32(5) == 1,
                ReadCapabilities(reader.GetString(6)),
                ReadStrings(reader.GetString(7)),
                ReadStrings(reader.GetString(8)),
                ReadStrings(reader.GetString(9)),
                reader.IsDBNull(10) ? null : reader.GetString(10)));
        }

        return providers;
    }

    private static IReadOnlyList<ProviderCapability> ReadCapabilities(string json)
        => ReadStrings(json).Select(Enum.Parse<ProviderCapability>).ToArray();

    private static IReadOnlyList<string> ReadStrings(string json)
        => JsonSerializer.Deserialize<string[]>(json) ?? [];
}
