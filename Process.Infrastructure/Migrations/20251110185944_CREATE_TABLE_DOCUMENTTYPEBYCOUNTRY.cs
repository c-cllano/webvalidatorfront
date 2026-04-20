using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_TABLE_DOCUMENTTYPEBYCOUNTRY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                                  IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DocumentTypeByCountry' AND xtype='U')
                                  BEGIN
                    CREATE TABLE DocumentTypeByCountry (
                        -- Clave primaria
                        DocumentTypeByCountryId INT IDENTITY(1,1) PRIMARY KEY,

                        -- Claves foráneas
                        CountryId INT NOT NULL,
                        DocumentTypeId INT NOT NULL,

                        -- Campos básicos
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                        UpdatedDate DATETIME NULL,
                        Active BIT NOT NULL DEFAULT 1,
                        IsDeleted BIT NOT NULL DEFAULT 0,                       
                    );

                                   END
                                      ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                   IF OBJECT_ID('dbo.DocumentTypeByCountry', 'U') IS NOT NULL
                                   BEGIN
                                     DROP TABLE dbo.DocumentTypeByCountry;
                                   END
                                   ");
        }

    }
}
