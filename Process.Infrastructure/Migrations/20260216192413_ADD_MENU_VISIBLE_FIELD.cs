using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ADD_MENU_VISIBLE_FIELD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF NOT EXISTS (
            SELECT 1 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'MENU' 
              AND COLUMN_NAME = 'VISIBLE'
        )
        BEGIN
            ALTER TABLE MENU
            ADD VISIBLE BIT NULL;
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT 1 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'MENU' 
              AND COLUMN_NAME = 'VISIBLE'
        )
        BEGIN
            ALTER TABLE MENU
            DROP COLUMN VISIBLE;
        END
    ");
        }

    }
}
