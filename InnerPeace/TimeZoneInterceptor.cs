using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InnerPeace;

public class TimeZoneInterceptor : DbConnectionInterceptor
{
    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = "SET TIME ZONE 'Africa/Cairo';";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    // Handle synchronous connections too
    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SET TIME ZONE 'Africa/Cairo';";
        command.ExecuteNonQuery();
    }
}