using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ALTER_TABLE_COUNTRY_ADD_COLUMN_FRECUENTCOUNTRY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- Agregar columna Country solo si no existe
IF COL_LENGTH('Country', 'frecuentCountry') IS NULL
BEGIN
    ALTER TABLE Country ADD frecuentCountry BIT;
END;

-- Agregar DEFAULT solo si no existe
IF NOT EXISTS (
    SELECT 1 
    FROM sys.default_constraints 
    WHERE parent_object_id = OBJECT_ID('Country') 
      AND name = 'DF_country_frecuentCountry'
)
BEGIN
    ALTER TABLE Country
    ADD CONSTRAINT DF_country_frecuentCountry DEFAULT 0 FOR frecuentCountry;
END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- Eliminar DEFAULT si existe
IF EXISTS (
    SELECT 1 
    FROM sys.default_constraints 
    WHERE parent_object_id = OBJECT_ID('Country') 
      AND name = 'DF_country_frecuentCountry'
)
BEGIN
    ALTER TABLE Country DROP CONSTRAINT DF_country_frecuentCountry;
END;

-- Eliminar columna si existe
IF COL_LENGTH('Country', 'frecuentCountry') IS NOT NULL
BEGIN
    ALTER TABLE Country DROP COLUMN frecuentCountry;
END;
");
        }


    }
}
