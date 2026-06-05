using Microsoft.Data.Sqlite;

namespace HstarCanvas.Persistence.Database;

public sealed class SqliteConnectionFactory
{
    private readonly string _databasePath;

    public SqliteConnectionFactory(string databasePath)
    {
        _databasePath = databasePath;
    }

    public async Task<SqliteConnection> OpenAsync(CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(_databasePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var connection = new SqliteConnection("Data Source=" + _databasePath);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}

