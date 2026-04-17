using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;

namespace Data;

public class DbHealthCheckService(IDbConnectionFactory factory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = factory.CreateConnection();

            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";

            await command.ExecuteScalarAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Database is not reachable after startup retries.", ex);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}