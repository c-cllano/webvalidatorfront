using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREAR_TABLA_REGION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                                  IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Region' AND xtype='U')
                                  BEGIN
                                    CREATE TABLE Region (
                                        RegionId INT IDENTITY(1,1) PRIMARY KEY,
                                        Name NVARCHAR(100) NOT NULL,
                                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                                        UpdatedDate DATETIME NULL,
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

                                   IF OBJECT_ID('dbo.Region', 'U') IS NOT NULL
                                   BEGIN
                                     DROP TABLE dbo.Region;
                                   END
                                   ");
        }
    }
}
