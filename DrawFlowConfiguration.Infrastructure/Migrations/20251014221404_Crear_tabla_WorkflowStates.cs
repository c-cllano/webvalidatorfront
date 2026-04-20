using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Crear_tabla_WorkflowStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='WorkflowStates' AND xtype='U')
                                    BEGIN
                                    CREATE TABLE WorkflowStates (
                                       WorkflowStatesId INT IDENTITY(1,1) PRIMARY KEY,
                                       State VARCHAR(255) NOT NULL,
                                       BackgroundColor VARCHAR(100) NOT NULL,
                                       FontColor VARCHAR(100) NOT NULL,
                                       CreatedDate DATETIME NOT NULL,
                                       UpdatedDate DATETIME NOT NULL,
                                       Active BIT NOT NULL,
                                       IsDeleted BIT NOT NULL
                                   );
                                    END
                                   ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                   IF OBJECT_ID('dbo.WorkflowStates', 'U') IS NOT NULL
                                   BEGIN
                                     DROP TABLE dbo.WorkflowStates;
                                   END
                                   ");
        }
    }
}
