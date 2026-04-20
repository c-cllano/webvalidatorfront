using Microsoft.EntityFrameworkCore;

namespace DrawFlowConfiguration.Infrastructure.Data
{
    public class ContextSqlServerDB(DbContextOptions<ContextSqlServerDB> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
