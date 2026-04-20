using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ALTER_TABLE_DOCUMENTTYPEBYCOUNTRY_ADD_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- Agregar FK solo si no existe
IF NOT EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE name = 'FK_DocumentTypeByCountry_Country'
)
BEGIN
    ALTER TABLE DocumentTypeByCountry
    ADD CONSTRAINT FK_DocumentTypeByCountry_Country
        FOREIGN KEY (CountryId) REFERENCES Country(CountryId);
END;

-- Agregar FK solo si no existe
IF NOT EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE name = 'FK_DocumentTypeByCountry_DocumentType'
)
BEGIN
    ALTER TABLE DocumentTypeByCountry
    ADD CONSTRAINT FK_DocumentTypeByCountry_DocumentType
        FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(DocumentTypeId);
END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- Eliminar FK si existe
IF EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE name = 'FK_DocumentTypeByCountry_Country'
)
BEGIN
    ALTER TABLE DocumentTypeByCountry
    DROP CONSTRAINT FK_DocumentTypeByCountry_Country;
END;

IF EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE name = 'FK_DocumentTypeByCountry_DocumentType'
)
BEGIN
    ALTER TABLE DocumentTypeByCountry
    DROP CONSTRAINT FK_DocumentTypeByCountry_DocumentType;
END;
");
        }


    }
}
