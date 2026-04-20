using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DrawFlowConfiguration.Infrastructure.Data
{
    public class ContextSqlServerDBFactory : IDesignTimeDbContextFactory<ContextSqlServerDB>
    {
        public ContextSqlServerDB CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"..\DrawFlowConfiguration.API"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("OKeyWorkflowConnection")
                                   ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

            var optionsBuilder = new DbContextOptionsBuilder<ContextSqlServerDB>();
            optionsBuilder.UseSqlServer(connectionString);
            return new ContextSqlServerDB(optionsBuilder.Options);
        }
    }
}
