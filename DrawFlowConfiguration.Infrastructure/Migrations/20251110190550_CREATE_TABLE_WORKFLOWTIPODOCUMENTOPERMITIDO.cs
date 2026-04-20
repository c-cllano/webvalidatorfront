using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_TABLE_WORKFLOWTIPODOCUMENTOPERMITIDO : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                  IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='WorkflowsTipoDocumentosPermitidos' AND xtype='U')
                                  BEGIN
                                    CREATE TABLE WorkflowsTipoDocumentosPermitidos (
                                    WorkflowsTipoDocumentosPermitidosId INT IDENTITY(1,1) PRIMARY KEY,
	                                WorkflowId  INT NOT NULL,
	                                DocumentTypeId  INT NOT NULL,
                                    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                                    UpdatedDate DATETIME NULL,
                                    Active BIT NOT NULL DEFAULT 1,
                                    IsDeleted BIT NOT NULL DEFAULT 0


	                                   -- Definición de llaves foráneas
                                    CONSTRAINT FK_WorkflowsTipoDocumentosPermitidos_Workflow FOREIGN KEY (WorkflowId)
                                        REFERENCES Workflows(WorkflowId)
                                );


                                   END
                                      ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                   IF OBJECT_ID('dbo.WorkflowsTipoDocumentosPermitidos', 'U') IS NOT NULL
                                   BEGIN
                                     DROP TABLE dbo.WorkflowsTipoDocumentosPermitidos;
                                   END
                                   ");
        }

    }
}
