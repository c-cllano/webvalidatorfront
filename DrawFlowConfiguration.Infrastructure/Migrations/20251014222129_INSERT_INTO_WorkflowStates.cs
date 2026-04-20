using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class INSERT_INTO_WorkflowStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO WorkflowStates (State, BackgroundColor, FontColor, CreatedDate, UpdatedDate, Active, IsDeleted)
                VALUES
                ('En Creación', '#002F47', '#FFFFFF', GETDATE(), GETDATE(), 1, 0),
                ('En pruebas', '#015783', '#FFFFFF', GETDATE(), GETDATE(), 1, 0),
                ('Publicado', '#128937', '#FFFFFF', GETDATE(), GETDATE(), 1, 0),
                ('Pausado', '#FF8307', '#FFFFFF', GETDATE(), GETDATE(), 1, 0),
                ('Sin Recursos', 'GRAY', 'PINK', GETDATE(), GETDATE(), 1, 0),
                ('Archivado', '#494A4B', '#FFFFFF', GETDATE(), GETDATE(), 1, 0);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM WorkflowStates
                WHERE State IN (
                    'En Creación',
                    'En pruebas',
                    'Publicado',
                    'Pausado',
                    'Sin Recursos',
                    'Archivado'
                );
            ");
        }
    }
}
