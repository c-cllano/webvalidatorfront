using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Crear_tabla_ScreenFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                  IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ScreenFlow' AND xtype='U')
                                  BEGIN
                                      CREATE TABLE dbo.ScreenFlow (
                                          ScreenFlowID INT IDENTITY(1,1) PRIMARY KEY,
                                          AgreementID UNIQUEIDENTIFIER NOT NULL,
                                          SelectedIdWorkflow INT NOT NULL,
                                          ContScreenFlow INT NOT NULL,
                                          OperationScreenFlowID INT NULL,
                                          StateScreenFlow BIT NOT NULL DEFAULT 1,
                                          CreatorUserID INT NULL,
                                          CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
                                          UpdatedDate DATETIME2 NULL,
                                          Active BIT NOT NULL DEFAULT 1,
                                          IsDeleted BIT NOT NULL DEFAULT 0,
                                  
                                          CONSTRAINT FK_ScreenFlow_OperationScreenFlow FOREIGN KEY (OperationScreenFlowID)
                                              REFERENCES dbo.OperationScreenFlow(OperationScreenFlowID)
                                      );
                                   END
                                      ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                   IF OBJECT_ID('dbo.ScreenFlow', 'U') IS NOT NULL
                                   BEGIN
                                     DROP TABLE dbo.ScreenFlow;
                                   END
                                   ");
        }
    }
}
