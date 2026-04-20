using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_WorkflowHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                               IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='WorkflowHistory' AND xtype='U')
                                BEGIN
                                   CREATE TABLE WorkflowHistory (
                                  WorkflowHistoryId INT IDENTITY(1,1) PRIMARY KEY,
                                  WorkflowId INT NOT NULL,
                                  ChangeDescription VARCHAR(255) NOT NULL,
                                  TypeChange VARCHAR(50) NOT NULL,
                                  UserId VARCHAR(100) NOT NULL,
                                  CreatedDate DATETIME NOT NULL,
                                  UpdatedDate DATETIME NOT NULL,
                                  Active BIT NOT NULL,
                                  IsDeleted BIT NOT NULL,
                                  CONSTRAINT FK_WorkflowHistory_Workflows FOREIGN KEY (workflowid)
                                      REFERENCES workflows(WorkflowId)
                              );
                                END
                         ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                               IF OBJECT_ID('dbo.WorkflowHistory', 'U') IS NOT NULL
                                BEGIN
                                  DROP TABLE dbo.WorkflowHistory;
                                END
                         ");
        }
    }
}
