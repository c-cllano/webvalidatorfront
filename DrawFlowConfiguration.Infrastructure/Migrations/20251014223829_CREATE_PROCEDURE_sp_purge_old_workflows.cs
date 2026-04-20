using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_PROCEDURE_sp_purge_old_workflows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE sp_purge_old_workflows
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- Verificar IDs de estados 'En creación' y 'Archivado'
                    DECLARE @EstadoEnCreacion INT = (
                        SELECT WorkflowStatesId FROM WORKFLOWSTATES WHERE STATE = 'En creación'
                    );
                    DECLARE @EstadoArchivado INT = (
                        SELECT WorkflowStatesId FROM WORKFLOWSTATES WHERE STATE = 'Archivado'
                    );

                    -- Eliminar workflows con más de 30 días y en esos estados
                    DELETE FROM workflows
                    WHERE WorkflowStatesId IN (@EstadoEnCreacion, @EstadoArchivado)
                      AND createdDate < DATEADD(DAY, -30, GETDATE());
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_purge_old_workflows;
            ");
        }
    }
}
