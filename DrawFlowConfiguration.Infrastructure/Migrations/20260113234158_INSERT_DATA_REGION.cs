using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class INSERT_DATA_REGION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM Region WHERE Name = 'América del Sur')
        BEGIN
            INSERT INTO Region (Name, CreatedDate, Active, IsDeleted)
            VALUES ('América del Sur', GETDATE(), 1, 0)
        END

        IF NOT EXISTS (SELECT 1 FROM Region WHERE Name = 'América del Norte Centro y El Caribe')
        BEGIN
            INSERT INTO Region (Name, CreatedDate, Active, IsDeleted)
            VALUES ('América del Norte Centro y El Caribe', GETDATE(), 1, 0)
        END

        IF NOT EXISTS (SELECT 1 FROM Region WHERE Name = 'Oceania')
        BEGIN
            INSERT INTO Region (Name, CreatedDate, Active, IsDeleted)
            VALUES ('Oceania', GETDATE(), 1, 0)
        END

        IF NOT EXISTS (SELECT 1 FROM Region WHERE Name = 'África')
        BEGIN
            INSERT INTO Region (Name, CreatedDate, Active, IsDeleted)
            VALUES ('África', GETDATE(), 1, 0)
        END

        IF NOT EXISTS (SELECT 1 FROM Region WHERE Name = 'Asia')
        BEGIN
            INSERT INTO Region (Name, CreatedDate, Active, IsDeleted)
            VALUES ('Asia', GETDATE(), 1, 0)
        END

        IF NOT EXISTS (SELECT 1 FROM Region WHERE Name = 'Europa')
        BEGIN
            INSERT INTO Region (Name, CreatedDate, Active, IsDeleted)
            VALUES ('Europa', GETDATE(), 1, 0)
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Region;");
        }

    }
}
