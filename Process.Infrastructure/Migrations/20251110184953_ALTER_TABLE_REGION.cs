using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ALTER_TABLE_REGION : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"

        -- Agregar columna RegionId solo si no existe
        IF COL_LENGTH('dbo.Country', 'RegionId') IS NULL
        BEGIN
            ALTER TABLE dbo.Country ADD RegionId INT NOT NULL DEFAULT 1;
        END;

        -- Agregar columna NameESP solo si no existe
        IF COL_LENGTH('dbo.Country', 'NameESP') IS NULL
        BEGIN
            ALTER TABLE dbo.Country ADD NameESP NVARCHAR(150) NULL;
        END;

        -- Agregar columna Flag solo si no existe
        IF COL_LENGTH('dbo.Country', 'Flag') IS NULL
        BEGIN
            ALTER TABLE dbo.Country ADD Flag NVARCHAR(255) NULL;
        END;
   
        -- Agregar la FK solo si no existe
        IF NOT EXISTS (
            SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Country_Region'
        )
        BEGIN
            ALTER TABLE dbo.Country
            ADD CONSTRAINT FK_Country_Region
            FOREIGN KEY (RegionId) REFERENCES dbo.Region(RegionId);
        END;
    ");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

        ALTER TABLE dbo.Country
        DROP CONSTRAINT FK_Country_Region;

        ALTER TABLE dbo.Country
        DROP COLUMN RegionId;

        ALTER TABLE dbo.Country
        DROP COLUMN NameESP;

        ALTER TABLE dbo.Country
        DROP COLUMN Flag;
    ");
        }


    }
}
