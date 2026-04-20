using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PASAPORTE_GENERICO_OTROS_PAISES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- 1. Aseguramos la existencia del DocumentType PPGE
DECLARE @Code NVARCHAR(10) = 'PPGE';
DECLARE @Name NVARCHAR(100) = 'Pasaporte Genérico';
DECLARE @PPGE_Id INT;

IF NOT EXISTS (SELECT 1 FROM DocumentType WHERE Code = @Code)
BEGIN
    INSERT INTO DocumentType (Code, [Name], [Length], Active, IsDeleted, [minLength], [maxLength], regularExpression, CreatedDate)
    VALUES (@Code, @Name, 0, 1, 0, 6, 16, '^[a-zA-Z0-9]{6,16}$', GETDATE());
    
    SET @PPGE_Id = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE DocumentType 
    SET Active = 1, IsDeleted = 0 
    WHERE Code = @Code;

    SELECT @PPGE_Id = DocumentTypeId 
    FROM DocumentType 
    WHERE Code = @Code;
END

-- 2. Insertar en DocumentTypeByCountry para países sin pasaporte
INSERT INTO DocumentTypeByCountry (
    DocumentTypeId, 
    CountryId, 
    Active, 
    IsDeleted, 
    CreatedDate
)
SELECT 
    @PPGE_Id,
    C.CountryId,
    1,
    0,
    GETDATE()
FROM Country C
WHERE C.Active = 1 
  AND C.IsDeleted = 0
  AND NOT EXISTS (
      SELECT 1 
      FROM DocumentTypeByCountry DTBC
      INNER JOIN DocumentType DT 
          ON DTBC.DocumentTypeId = DT.DocumentTypeId
      WHERE DTBC.CountryId = C.CountryId
        AND DT.[Name] LIKE '%pasaporte%'
        AND DTBC.Active = 1 
        AND DTBC.IsDeleted = 0
  );
");
        }
    }
}
