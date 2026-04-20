using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class INSERT_DATA_REGION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

               INSERT INTO Region (Name, CreatedDate, Active, IsDeleted)
                VALUES 
                ('América del Sur', GETDATE(), 1, 0),
                ('América del Norte Centro y El Caribe', GETDATE(), 1, 0),
                ('Oceania', GETDATE(), 1, 0),
                ('África', GETDATE(), 1, 0),
                ('Asia', GETDATE(), 1, 0),
                ('Europa', GETDATE(), 1, 0);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                DELETE FROM Region;
            ");
        }

    }
}
