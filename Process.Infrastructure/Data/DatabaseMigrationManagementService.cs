using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Data
{
    public static class DatabaseMigrationManagementService
    {
        public static async Task MigrationInitialisation(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var factory = scope.ServiceProvider.GetRequiredService<SQLServerConnectionFactory>();

            using var connection = factory.CreateConnection(ConnectionsName.OKeyConnection);

            var options = new DbContextOptionsBuilder<ContextSqlServerDB>()
                             .UseSqlServer(connection.ConnectionString)
                             .Options;

            using var serviceDb = new ContextSqlServerDB(options);
            if (!await serviceDb.Database.CanConnectAsync())
            {
                throw new InvalidOperationException($"No se pudo conectar la base de datos. Cadena de conexión: {connection.ConnectionString}");
            }
            IEnumerable<string> migrations = await serviceDb?.Database?.GetPendingMigrationsAsync()!;
            if (migrations.Any())
                await serviceDb.Database.MigrateAsync();
        }
    }
}
