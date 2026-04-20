using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_TABLE_WORKFLOWSUBICACIONESPERMITIDAS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                  IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='WorkflowsUbicacionesPermitidas' AND xtype='U')
                                  BEGIN
                                                                        CREATE TABLE WorkflowsUbicacionesPermitidas (
                                        WorkflowsUbicacionesPermitidasId INT IDENTITY(1,1) PRIMARY KEY,
	                                    WorkflowId  INT NOT NULL,
	                                    CountryId  INT NOT NULL,
                                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                                        UpdatedDate DATETIME NULL,
                                        Active BIT NOT NULL DEFAULT 1,
                                        IsDeleted BIT NOT NULL DEFAULT 0


	                                       -- Definición de llaves foráneas
                                        CONSTRAINT FK_WorkflowsUbicacionesPermitidas_Workflow FOREIGN KEY (WorkflowId)
                                            REFERENCES Workflows(WorkflowId)
                                    );

                                   END
                                      ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                   IF OBJECT_ID('dbo.WorkflowsUbicacionesPermitidas', 'U') IS NOT NULL
                                   BEGIN
                                     DROP TABLE dbo.WorkflowsUbicacionesPermitidas;
                                   END
                                   ");
        }

    }
}
