using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_TRIGGER_trg_workflows_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
               CREATE OR ALTER TRIGGER trg_workflows_history
                ON workflows
                AFTER INSERT, UPDATE, DELETE
                AS
                BEGIN
                    SET NOCOUNT ON;
                
                    -- 1. Inserciones
                    IF EXISTS(SELECT 1 FROM inserted) AND NOT EXISTS(SELECT 1 FROM deleted)
                    BEGIN
                        INSERT INTO workflowhistory 
                            (WorkflowId, Changedescription, CreatedDate, UpdatedDate, UserId, TypeChange, Active, IsDeleted)
                        SELECT
                            i.WorkFlowID,
                            'Se creó un nuevo flujo de trabajo',
                            GETDATE(),
                            GETDATE(),
                            -1,
                            'INSERCION',
                            1,
                            0
                        FROM inserted i;
                    END
                
                    -- 2. Eliminaciones
                    IF EXISTS(SELECT 1 FROM deleted) AND NOT EXISTS(SELECT 1 FROM inserted)
                    BEGIN
                        INSERT INTO workflowhistory
                            (WorkflowId, Changedescription, CreatedDate, UpdatedDate, UserId, TypeChange, Active, IsDeleted)
                        SELECT
                            d.WorkFlowID,
                            'Eliminación definitiva del flujo',
                            GETDATE(),
                            GETDATE(),
                            -1,
                            'ELIMINACION',
                            1,
                            0
                        FROM deleted d;
                    END
                
                    -- 3. Actualizaciones
                    IF EXISTS(SELECT 1 FROM inserted) AND EXISTS(SELECT 1 FROM deleted)
                    BEGIN
                        -- Cambios en name
                        INSERT INTO workflowhistory
                            (WorkflowId, Changedescription, CreatedDate, UpdatedDate, UserId, TypeChange, Active, IsDeleted)
                        SELECT
                            i.WorkFlowID,
                            'Se actualizó información en la columna name',
                            GETDATE(),
                            GETDATE(),
                            -1,
                            'EDICION',
                            1,
                            0
                        FROM inserted i
                        INNER JOIN deleted d ON i.WorkFlowID = d.WorkFlowID
                        WHERE ISNULL(i.name, '') <> ISNULL(d.name, '');
                
                        -- Cambios en description
                        INSERT INTO workflowhistory
                            (WorkflowId, Changedescription, CreatedDate, UpdatedDate, UserId, TypeChange, Active, IsDeleted)
                        SELECT
                            i.WorkFlowID,
                            'Se actualizó información en la columna description',
                            GETDATE(),
                            GETDATE(),
                            -1,
                            'EDICION',
                            1,
                            0
                        FROM inserted i
                        INNER JOIN deleted d ON i.WorkFlowID = d.WorkFlowID
                        WHERE ISNULL(i.description, '') <> ISNULL(d.description, '');
                
                        -- Cambios en version
                        INSERT INTO workflowhistory
                            (WorkflowId, Changedescription, CreatedDate, UpdatedDate, UserId, TypeChange, Active, IsDeleted)
                        SELECT
                            i.WorkFlowID,
                            'Se actualizó información en la columna version',
                            GETDATE(),
                            GETDATE(),
                            -1,
                            'EDICION',
                            1,
                            0
                        FROM inserted i
                        INNER JOIN deleted d ON i.WorkFlowID = d.WorkFlowID
                        WHERE ISNULL(i.version, '') <> ISNULL(d.version, '');
                
                        -- Cambios en isdeleted (0 -> 1)
                        INSERT INTO workflowhistory
                            (WorkflowId, Changedescription, CreatedDate, UpdatedDate, UserId, TypeChange, Active, IsDeleted)
                        SELECT
                            i.WorkFlowID,
                            'Se eliminó',
                            GETDATE(),
                            GETDATE(),
                            -1,
                            'EDICION',
                            1,
                            0
                        FROM inserted i
                        INNER JOIN deleted d ON i.WorkFlowID = d.WorkFlowID
                        WHERE ISNULL(i.isdeleted, 0) = 1 AND ISNULL(d.isdeleted, 0) = 0;
                
                        -- Cambios en active (1 -> 0)
                        INSERT INTO workflowhistory
                            (WorkflowId, Changedescription, CreatedDate, UpdatedDate, UserId, TypeChange, Active, IsDeleted)
                        SELECT
                            i.WorkFlowID,
                            'Se inactivó',
                            GETDATE(),
                            GETDATE(),
                            -1,
                            'EDICION',
                            1,
                            0
                        FROM inserted i
                        INNER JOIN deleted d ON i.WorkFlowID = d.WorkFlowID
                        WHERE ISNULL(i.active, 1) = 0 AND ISNULL(d.active, 1) = 1;
                
                        -- Cambios en WorkflowStatesId (antes workflow_state_id)
                        INSERT INTO workflowhistory
                            (WorkflowId, Changedescription, CreatedDate, UpdatedDate, UserId, TypeChange, Active, IsDeleted)
                        SELECT
                            i.WorkFlowID,
                            'Se cambió el estado a ' + ws.State,
                            GETDATE(),
                            GETDATE(),
                            -1,
                            'EDICION',
                            1,
                            0
                        FROM inserted i
                        INNER JOIN deleted d ON i.WorkFlowID = d.WorkFlowID
                        INNER JOIN workflowstates ws ON i.WorkflowStatesId = ws.WorkflowStatesId
                        WHERE ISNULL(i.WorkflowStatesId, 0) <> ISNULL(d.WorkflowStatesId, 0);
                    END
                END;
              ");
          }
  
          /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('trg_workflows_history', 'TR') IS NOT NULL
                    DROP TRIGGER trg_workflows_history;
            ");
        }
    }
}
