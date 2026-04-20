using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrawFlowConfiguration.Infrastructure.Data
{
    public static class DatabaseMigrationManagementService
    {
        public static async Task MigrationInitialisation(this IApplicationBuilder app)
        {

            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceDb = serviceScope.ServiceProvider
                             .GetService<ContextSqlServerDB>();
            if (!await serviceDb?.Database?.CanConnectAsync()!)
            {
                throw new InvalidOperationException($"No se pudo conectar la base de datos. Cadena de conexión: {serviceDb?.Database.GetConnectionString()}");
            }
            IEnumerable<string> migrations = await serviceDb?.Database?.GetPendingMigrationsAsync()!;
            if (migrations.Any())
            {
                await serviceDb?.Database?.MigrateAsync()!;
            }

        }
    }
}
