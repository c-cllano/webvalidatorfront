using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CORREGIDO_SP_DUPLICAR_FLUJOS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE sp_DuplicateWorkflow 
    @WorkflowIdOld INT,
    @AgreementId VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Workflows 
                   WHERE WorkFlowID = @WorkflowIdOld 
                     AND AgreementID = @AgreementId 
                     AND Active = 1 AND IsDeleted = 0)
        BEGIN
            -- 1. Crear el workflow nuevo
            INSERT INTO Workflows (AgreementID, CreatorUserID, [Name], [Description], [Version], Active, IsDeleted, CreatedDate, UpdatedDate, WorkflowStatesId)
            SELECT AgreementId, CreatorUserID, 'Copia de ' + [Name], [Description], [Version], Active, IsDeleted, GETDATE(), NULL, 1
            FROM Workflows
            WHERE WorkFlowID = @WorkflowIdOld 
              AND AgreementID = @AgreementId 
              AND Active = 1 AND IsDeleted = 0;

            DECLARE @NuevoWorkflowID INT = SCOPE_IDENTITY();

            -- 2. Inactivar el registro viejo
            --UPDATE Workflows 
            --SET Active = 0, IsDeleted = 1 
            --WHERE WorkFlowID = @WorkflowIdOld 
             --AND AgreementID = @AgreementId;

            -- 3. Nacionalidades
            INSERT INTO WorkflowsNacionalidadesPermitidas (WorkflowId, CountryId, CreatedDate, UpdatedDate, Active, IsDeleted)
            SELECT @NuevoWorkflowID, CountryId, GETDATE(), NULL, 1, 0
            FROM WorkflowsNacionalidadesPermitidas
            WHERE WorkflowId = @WorkflowIdOld AND Active = 1 AND IsDeleted = 0;

            UPDATE WorkflowsNacionalidadesPermitidas 
            SET Active = 0, IsDeleted = 1 
            WHERE WorkFlowID = @WorkflowIdOld AND Active = 1 AND IsDeleted = 0;

            -- 4. Ubicaciones
            INSERT INTO WorkflowsUbicacionesPermitidas (WorkflowId, CountryId, CreatedDate, UpdatedDate, Active, IsDeleted)
            SELECT @NuevoWorkflowID, CountryId, GETDATE(), NULL, 1, 0
            FROM WorkflowsUbicacionesPermitidas
            WHERE WorkflowId = @WorkflowIdOld AND Active = 1 AND IsDeleted = 0;

            UPDATE WorkflowsUbicacionesPermitidas 
            SET Active = 0, IsDeleted = 1 
            WHERE WorkFlowID = @WorkflowIdOld AND Active = 1 AND IsDeleted = 0;

            -- 5. Documentos
            INSERT INTO WorkflowsTipoDocumentosPermitidos (WorkflowId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
            SELECT @NuevoWorkflowID, DocumentTypeId, GETDATE(), NULL, 1, 0
            FROM WorkflowsTipoDocumentosPermitidos
            WHERE WorkflowId = @WorkflowIdOld AND Active = 1 AND IsDeleted = 0;

            UPDATE WorkflowsTipoDocumentosPermitidos 
            SET Active = 0, IsDeleted = 1 
            WHERE WorkFlowID = @WorkflowIdOld AND Active = 1 AND IsDeleted = 0;

            COMMIT TRANSACTION;
            SELECT @NuevoWorkflowID AS NewWorkflowID;
        END
        ELSE
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('El flujo no existe o no está activo.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_DuplicateWorkflow;");
        }
    }
}
