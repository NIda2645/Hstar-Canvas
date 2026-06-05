namespace HstarCanvas.Persistence.Database;

public sealed class DatabaseMigrator
{
    private readonly SqliteConnectionFactory _factory;

    public DatabaseMigrator(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using var connection = await _factory.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = BaselineSql;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private const string BaselineSql = """
CREATE TABLE IF NOT EXISTS schema_migrations (
    version INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    applied_at TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS providers (
    id TEXT PRIMARY KEY,
    name TEXT NOT NULL,
    protocol TEXT NOT NULL,
    endpoint TEXT NOT NULL,
    enabled INTEGER NOT NULL,
    primary_provider INTEGER NOT NULL,
    capabilities_json TEXT NOT NULL,
    image_models_json TEXT NOT NULL,
    chat_models_json TEXT NOT NULL,
    video_models_json TEXT NOT NULL,
    secret_ref TEXT NULL,
    updated_at TEXT NOT NULL
);

INSERT OR IGNORE INTO schema_migrations (version, name, applied_at)
VALUES (1, 'baseline', strftime('%Y-%m-%dT%H:%M:%fZ', 'now'));
""";
}
