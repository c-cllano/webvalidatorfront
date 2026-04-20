using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREAR_MENU_ROLECONSULTAAPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF NOT EXISTS (
            SELECT 1 FROM MENU WHERE Title = 'Usuario consola API'
        )
        BEGIN
            INSERT INTO MENU 
            (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES 
            (NULL, 1, 'Usuario consola API', 'Usuario consola API', '', '', 0, GETDATE(), NULL, 1, 0, 1)
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        UPDATE MENU
        SET IsDeleted = 1, Active = 0,
            UpdatedDate = GETDATE()
        WHERE Title = 'Usuario consola API'
        AND IsDeleted = 0
    ");
        }

    }
}
