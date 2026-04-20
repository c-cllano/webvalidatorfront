using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREAR_HIJOS_MENU_SOLICITUDES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DECLARE @_IdMenuPadre INT;
        DECLARE @_LinkPadre NVARCHAR(200) = 'flow/screen-creation-process';

        SELECT @_IdMenuPadre = MenuId 
        FROM MENU 
        WHERE Link = @_LinkPadre;

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (
            SELECT 1 FROM MENU WHERE Link = 'flow/screen-created-process'
        )
        BEGIN
            INSERT INTO MENU 
            (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES 
            (@_IdMenuPadre, 1, 'Created', 'Created', '', 'flow/screen-created-process', 0, GETDATE(), NULL, 1, 0, 0);
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM MENU 
        WHERE Link = 'flow/screen-created-process';
    ");
        }

    }
}
