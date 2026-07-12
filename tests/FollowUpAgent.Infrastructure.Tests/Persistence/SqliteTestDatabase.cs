using FollowUpAgent.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FollowUpAgent.Infrastructure.Tests.Persistence;

internal sealed class SqliteTestDatabase : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    private SqliteTestDatabase(SqliteConnection connection, FollowUpAgentDbContext dbContext)
    {
        _connection = connection;
        DbContext = dbContext;
    }

    public FollowUpAgentDbContext DbContext { get; }

    public static async Task<SqliteTestDatabase> CreateAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<FollowUpAgentDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new FollowUpAgentDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();

        return new SqliteTestDatabase(connection, dbContext);
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
