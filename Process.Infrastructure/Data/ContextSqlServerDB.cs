using Microsoft.EntityFrameworkCore;

namespace Process.Infrastructure.Data
{
    public class ContextSqlServerDB(DbContextOptions<ContextSqlServerDB> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
