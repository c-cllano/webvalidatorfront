using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREAR_HIJOS_MENU_USUARIOS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DECLARE @_IdMenuPadre INT;
        DECLARE @_LinkPadre NVARCHAR(200) = '/equipo/usuarios';

        SELECT @_IdMenuPadre = MenuId 
        FROM MENU 
        WHERE Link = @_LinkPadre;

        IF @_IdMenuPadre IS NOT NULL
        BEGIN

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/create')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 1, 'Crear usuarios', 'Crear usuarios', '', '/equipo/create', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/success')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 2, 'Crear usuarios ok', 'Crear usuarios ok', '', '/equipo/success', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/summary')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 3, 'Resumen usuarios', 'Resumen usuarios', '', '/equipo/summary', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/edit-info')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 4, 'Edición info usuarios', 'Edición info usuarios', '', '/equipo/edit-info', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/edit-permissions')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 5, 'Edición permisos usuarios', 'Edición permisos usuarios', '', '/equipo/edit-permissions', 0, GETDATE(), NULL, 1, 0, 0);
            END

        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM MENU
        WHERE Link IN (
            '/equipo/create',
            '/equipo/success',
            '/equipo/summary',
            '/equipo/edit-info',
            '/equipo/edit-permissions'
        );
    ");
        }

    }
}
