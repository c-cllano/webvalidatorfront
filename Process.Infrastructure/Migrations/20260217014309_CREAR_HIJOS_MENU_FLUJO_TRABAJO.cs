using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREAR_HIJOS_MENU_FLUJO_TRABAJO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DECLARE @_IdMenuPadre INT;
        DECLARE @_LinkPadre NVARCHAR(200) = 'flow/workflow-list';

        SELECT @_IdMenuPadre = MenuId 
        FROM MENU 
        WHERE Link = @_LinkPadre;

        IF @_IdMenuPadre IS NOT NULL
        BEGIN

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'flow/screen-creation-workflow')
            BEGIN
                INSERT INTO MENU 
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES 
                (@_IdMenuPadre, 1, 'Creacion de flujos', 'Creacion de flujos', '', 'flow/screen-creation-workflow', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'flow/workflow')
            BEGIN
                INSERT INTO MENU 
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES 
                (@_IdMenuPadre, 2, 'Editor Canvas Flujo de trabajo', 'Editor canvas de flujos', '', 'flow/workflow', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'flow/screen-setup')
            BEGIN
                INSERT INTO MENU 
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES 
                (@_IdMenuPadre, 3, 'Pantalla pasos flujo', 'Pantalla pasos flujo', '', 'flow/screen-setup', 0, GETDATE(), NULL, 1, 0, 0);
            END

        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM MENU 
        WHERE Link IN (
            'flow/screen-creation-workflow',
            'flow/workflow',
            'flow/screen-setup'
        );
    ");
        }

    }
}
