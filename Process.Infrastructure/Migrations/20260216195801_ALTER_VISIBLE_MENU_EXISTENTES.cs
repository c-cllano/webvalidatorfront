using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ALTER_VISIBLE_MENU_EXISTENTES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        UPDATE MENU
        SET Visible = 
            CASE 
                WHEN Link IN (
                    'dashboard/main',
                    'flow/screen-creation-process',
                    'flow/workflow-list',
                    '/procesos',
                    '/equipo/usuarios',
                    '/equipo/roles'
                )
                OR Title = 'Equipo'
                THEN 1
                ELSE 0
            END
        WHERE Visible IS NULL;
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        UPDATE MENU
        SET Visible = NULL;
    ");
        }

    }
}
