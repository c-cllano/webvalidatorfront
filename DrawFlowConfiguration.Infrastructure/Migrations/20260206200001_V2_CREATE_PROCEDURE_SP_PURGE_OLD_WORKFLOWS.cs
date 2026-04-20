using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2_CREATE_PROCEDURE_SP_PURGE_OLD_WORKFLOWS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
               CREATE OR ALTER PROCEDURE sp_purge_old_workflows
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @EstadoArchivado INT;

                    -- Obtener el ID del estado ARCHIVADO
                    SELECT @EstadoArchivado = WorkflowStatesId
                    FROM WorkflowStates
                    WHERE UPPER([State]) = 'ARCHIVADO'
                      AND Active = 1
                      AND IsDeleted = 0;

                    -- Eliminado lógico de workflows archivados con más de 30 días
                    UPDATE W
                    SET
                        W.Active = 0,
                        W.IsDeleted = 1,
                        W.UpdatedDate = GETDATE()
                    FROM Workflows W
                    WHERE
                        W.WorkflowStatesId = @EstadoArchivado
                        AND W.ArchivedDate <> '1900-01-01'
                        AND W.ArchivedDate <= DATEADD(DAY, -30, GETDATE())
                        AND W.Active = 1
                        AND W.IsDeleted = 0;
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
