using HstarCanvas.Persistence.Database;
using Microsoft.Data.Sqlite;

namespace HstarCanvas.Persistence.Tests;

public sealed class DatabaseMigratorTests
{
    [Fact]
    public async Task MigrateAsync_CreatesProviderTables()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        var factory = new SqliteConnectionFactory(databasePath);

        await new DatabaseMigrator(factory).MigrateAsync(CancellationToken.None);

        await using var connection = new SqliteConnection("Data Source=" + databasePath);
        await connection.OpenAsync();
        Assert.Equal("schema_migrations", await FindTableAsync(connection, "schema_migrations"));
        Assert.Equal("providers", await FindTableAsync(connection, "providers"));
    }

    private static async Task<object?> FindTableAsync(SqliteConnection connection, string table)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=$name;";
        command.Parameters.AddWithValue("$name", table);
        return await command.ExecuteScalarAsync();
    }
}
