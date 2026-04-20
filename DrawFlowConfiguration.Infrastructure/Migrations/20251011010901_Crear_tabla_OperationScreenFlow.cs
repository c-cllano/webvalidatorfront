using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Crear_tabla_OperationScreenFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OperationScreenFlow' AND xtype='U')
                                     BEGIN
                                         CREATE TABLE dbo.OperationScreenFlow (
                                             OperationScreenFlowID INT IDENTITY(1,1) PRIMARY KEY,
                                             OperationScreenNombre NVARCHAR(255) NOT NULL,
                                             PriorityScreenFlow INT NOT NULL,             
                                             CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),     
                                             UpdatedDate DATETIME2 NULL,                        
                                             Active BIT NOT NULL DEFAULT 1,                        
                                             IsDeleted BIT NOT NULL DEFAULT 0                       
                                         );
                                     END
                                      ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                   IF OBJECT_ID('dbo.OperationScreenFlow', 'U') IS NOT NULL
                                   BEGIN
                                     DROP TABLE dbo.OperationScreenFlow;
                                   END
                                   ");
        }
    }
}
