using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_MENU_CONFIGURATIONUI : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM MENU WHERE TITLE = 'Configuración UI')
        BEGIN
            UPDATE MENU
            SET IsDeleted = 0,
                Active = 1,
                Visible = 1,
                UpdatedDate = GETDATE()
            WHERE TITLE = 'Configuración UI'
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM MENU WHERE TITLE = 'Configuración UI')
        BEGIN
            UPDATE MENU
            SET IsDeleted = 1,
                Active = 1,
                Visible = 0,
                UpdatedDate = GETDATE()
            WHERE TITLE = 'Configuración UI'
        END
    ");
        }
    }
}
