using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREAR_HIJOS_MENU_ROLES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DECLARE @_IdMenuPadre INT;
        DECLARE @_LinkPadre NVARCHAR(200) = '/equipo/roles';

        SELECT @_IdMenuPadre = MenuId 
        FROM MENU 
        WHERE Link = @_LinkPadre;

        IF @_IdMenuPadre IS NOT NULL
        BEGIN

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/roles/create')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 1, 'Crear roles', 'Crear roles', '', '/equipo/roles/create', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/roles/success')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 2, 'Crear roles ok', 'Crear roles ok', '', '/equipo/roles/success', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/roles/summary')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 3, 'Resumen roles', 'Resumen roles', '', '/equipo/roles/summary', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/roles/edit-info')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 4, 'Edit info roles', 'Edit info roles', '', '/equipo/roles/edit-info', 0, GETDATE(), NULL, 1, 0, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM MENU WHERE Link = '/equipo/roles/edit-permissions')
            BEGIN
                INSERT INTO MENU
                (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
                VALUES
                (@_IdMenuPadre, 5, 'Edit permisos roles', 'Edit permisos roles', '', '/equipo/roles/edit-permissions', 0, GETDATE(), NULL, 1, 0, 0);
            END

        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM MENU
        WHERE Link IN (
            '/equipo/roles/create',
            '/equipo/roles/success',
            '/equipo/roles/summary',
            '/equipo/roles/edit-info',
            '/equipo/roles/edit-permissions'
        );
    ");
        }

    }
}
