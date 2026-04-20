using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_SP_PUBLICATEWORKFLOW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE sp_SetWorkflowPublicado
(
    @AgreementId NVARCHAR(36),
    @WorkflowId INT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @IdPublicado INT;
    DECLARE @IdPausado INT;

    BEGIN TRY
        BEGIN TRAN;

        -- Obtener IDs de estados
        SELECT @IdPublicado = WorkflowStatesId
        FROM WorkflowStates
        WHERE Active = 1 
          AND IsDeleted = 0 
          AND [State] = 'Publicado';

        SELECT @IdPausado = WorkflowStatesId
        FROM WorkflowStates
        WHERE Active = 1 
          AND IsDeleted = 0 
          AND [State] = 'Pausado';

        IF @IdPublicado IS NULL OR @IdPausado IS NULL
            THROW 50001, 'No se encontraron los estados requeridos.', 1;

        -- Poner en pausa cualquier workflow publicado del Agreement
        UPDATE Workflows
        SET WorkflowStatesId = @IdPausado,
            UpdatedDate = GETDATE()
        WHERE AgreementID = @AgreementId
          AND WorkflowStatesId = @IdPublicado
          AND Active = 1
          AND IsDeleted = 0;

        -- Publicar el workflow recibido
        UPDATE Workflows
        SET WorkflowStatesId = @IdPublicado,
            UpdatedDate = GETDATE()
        WHERE WorkflowId = @WorkflowId
          AND AgreementID = @AgreementId
          AND Active = 1
          AND IsDeleted = 0;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        THROW;
    END CATCH
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID('sp_SetWorkflowPublicado', 'P') IS NOT NULL
    DROP PROCEDURE sp_SetWorkflowPublicado;
");
        }
    }
}
