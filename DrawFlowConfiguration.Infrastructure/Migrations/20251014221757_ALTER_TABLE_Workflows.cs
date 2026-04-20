using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ALTER_TABLE_Workflows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
            -- Agregar columna WorkflowStatesId solo si no existe
            IF COL_LENGTH('Workflows', 'WorkflowStatesId') IS NULL
            BEGIN
                ALTER TABLE Workflows ADD WorkflowStatesId INT;
            END;
        
            -- Agregar la FK solo si no existe
            IF NOT EXISTS (
                SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Workflows_WorkflowStates'
            )
            BEGIN
                ALTER TABLE Workflows
                ADD CONSTRAINT FK_Workflows_WorkflowStates
                FOREIGN KEY (WorkflowStatesId) REFERENCES WorkflowStates(WorkflowStatesId);
            END;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
                ALTER TABLE Workflows
                DROP CONSTRAINT FK_Workflows_WorkflowStates;

                ALTER TABLE Workflows
                DROP COLUMN WorkflowStatesId;
            ");
        }
    }
}
