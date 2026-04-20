using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREAR_CAMPOS_MINLENGHT_MAXLENGHT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agregar MinLength si no existe
            migrationBuilder.Sql(@"
        IF NOT EXISTS (
            SELECT * 
            FROM sys.columns 
            WHERE object_id = OBJECT_ID('DocumentType') 
              AND name = 'MinLength'
        )
        BEGIN
            ALTER TABLE DocumentType ADD MinLength INT NULL;
        END
    ");

            // Agregar MaxLength si no existe
            migrationBuilder.Sql(@"
        IF NOT EXISTS (
            SELECT * 
            FROM sys.columns 
            WHERE object_id = OBJECT_ID('DocumentType') 
              AND name = 'MaxLength'
        )
        BEGIN
            ALTER TABLE DocumentType ADD MaxLength INT NULL;
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar MinLength si existe
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT * 
            FROM sys.columns 
            WHERE object_id = OBJECT_ID('DocumentType') 
              AND name = 'MinLength'
        )
        BEGIN
            ALTER TABLE DocumentType DROP COLUMN MinLength;
        END
    ");

            // Eliminar MaxLength si existe
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT * 
            FROM sys.columns 
            WHERE object_id = OBJECT_ID('DocumentType') 
              AND name = 'MaxLength'
        )
        BEGIN
            ALTER TABLE DocumentType DROP COLUMN MaxLength;
        END
    ");
        }
    }
}
