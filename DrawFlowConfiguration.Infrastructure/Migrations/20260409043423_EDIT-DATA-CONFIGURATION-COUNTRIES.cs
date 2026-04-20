using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EDITDATACONFIGURATIONCOUNTRIES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
UPDATE DOCUMENTTYPE
SET MinLength = 4, RegularExpression = '^\d+$', UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'CC' AND UPPER(C.NameESP) = 'COLOMBIA') 
)

UPDATE DOCUMENTTYPE
SET RegularExpression = '^[a-zA-Z0-9]+$', UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'CE' AND UPPER(C.NameESP) = 'COLOMBIA') 
)

UPDATE DOCUMENTTYPE
SET RegularExpression = '^[a-zA-Z0-9]+$', UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PP' AND UPPER(C.NameESP) = 'COLOMBIA')
)


UPDATE DOCUMENTTYPE
SET MinLength = 6, RegularExpression = '^[a-zA-Z0-9]+$', UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PEP' AND UPPER(C.NameESP) = 'COLOMBIA')
)

UPDATE DOCUMENTTYPE
SET Code= 'RUMV', MinLength = 6, [MaxLength]=12 ,RegularExpression = '^\d+$', UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'Y' AND UPPER(C.NameESP) = 'COLOMBIA')
)

UPDATE DOCUMENTTYPE
SET MinLength = 6, [MaxLength]=9 ,RegularExpression = '^[a-zA-Z0-9]+$' , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPM' AND UPPER(C.NameESP) = 'MÉXICO')
)


UPDATE DOCUMENTTYPE
SET RegularExpression =  '^\d+$' , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'CCE' AND UPPER(C.NameESP) = 'ECUADOR')
)

UPDATE DOCUMENTTYPE
SET  MinLength = 6, [MaxLength]=9 , RegularExpression =   '^[a-zA-Z0-9]+$' , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPE' AND  UPPER(C.NameESP) = 'ECUADOR')
)


UPDATE DOCUMENTTYPE
SET  [MaxLength]=9 , RegularExpression =  '^\d+$' , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'DNIA' AND  UPPER(C.NameESP) = 'ARGENTINA')
)


UPDATE DOCUMENTTYPE
SET   RegularExpression =  '^\d+$' , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'DNIP' AND  UPPER(C.NameESP) = 'PERÚ')
)


UPDATE DOCUMENTTYPE
SET  MinLength = 6 , RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PSP' AND UPPER(C.NameESP) = 'PERÚ')
)

UPDATE DOCUMENTTYPE
SET  MinLength = 5, [MaxLength]= 10 , RegularExpression =  '^\d+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'CIPP' AND UPPER(C.NameESP) = 'PANAMÁ')
)

UPDATE DOCUMENTTYPE
SET  MinLength = 6, [MaxLength]= 9 , RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPP' AND UPPER(C.NameESP) = 'PANAMÁ')
)

UPDATE DOCUMENTTYPE
SET  MinLength = 7, RegularExpression =  '^\d+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'RG' AND  UPPER(C.NameESP) = 'BRASIL')
)

UPDATE DOCUMENTTYPE
SET  Code='CIP', MinLength = 7, [MaxLength] = 8 ,RegularExpression =  '^\d+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'RUT' AND  UPPER(C.NameESP) = 'CHILE')
)

UPDATE DOCUMENTTYPE
SET   MinLength = 6, [MaxLength] = 9 ,RegularExpression =  '^[a-zA-Z0-9]+$'   , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPC' AND  UPPER(C.NameESP) = 'CHILE')
)


UPDATE DOCUMENTTYPE
SET  MinLength = 6, [MaxLength]= 9 , RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPV' AND   UPPER(C.NameESP) = 'VENEZUELA')
)

UPDATE DOCUMENTTYPE
SET  RegularExpression =   '^\d+$'   , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'DPI' AND  UPPER(C.NameESP) = 'GUATEMALA')
)

UPDATE DOCUMENTTYPE
SET  MinLength = 6, [MaxLength]= 9 , RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPG' AND  UPPER(C.NameESP) = 'GUATEMALA')
)

UPDATE DOCUMENTTYPE
SET  MinLength = 6, [MaxLength]= 9 , RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPS' AND  UPPER(C.NameESP) = 'EL SALVADOR')
)


UPDATE DOCUMENTTYPE
SET RegularExpression =  '^\d+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'CI' AND UPPER(C.NameESP) = 'COSTA RICA')
)


UPDATE DOCUMENTTYPE
SET MinLength = 6, [MaxLength]= 9 , RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPR' AND UPPER(C.NameESP) = 'COSTA RICA' )
)

UPDATE DOCUMENTTYPE
SET  RegularExpression =   '^\d+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'DNIH' AND  UPPER(C.NameESP) = 'HONDURAS')
)

UPDATE DOCUMENTTYPE
SET   RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPES' AND  UPPER(C.NameESP) = 'ESPAÑA')
)

DECLARE @USAId INT;
DECLARE @USAPPUSId INT;

-- Obtener CountryId de Estados Unidos
SELECT @USAId = CountryId  FROM COUNTRY  WHERE UPPER(NAMEESP) = 'ESTADOS UNIDOS';

-- Solo insertar el DocumentType si no existe
IF NOT EXISTS (
    SELECT 1  FROM DOCUMENTTYPE  WHERE UPPER(CODE) = 'PPUS'
)
BEGIN
    INSERT INTO DocumentType 
        (CODE, [NAME], [LENGTH], REGULAREXPRESSION, CREATEDDATE, UPDATEDDATE, ACTIVE, ISDELETED, MINLENGTH, [MAXLENGTH])
    VALUES
        ('PPUS','Pasaporte USA', 0, '^[a-zA-Z0-9]+$', GETDATE(), NULL, 1, 0, 9, 9);
END

-- Obtener el DocumentTypeId recién creado o existente
SELECT @USAPPUSId = DocumentTypeId FROM DOCUMENTTYPE WHERE UPPER(CODE) = 'PPUS';

-- Solo insertar en DocumentTypeByCountry si no existe
IF NOT EXISTS (
    SELECT 1 FROM DocumentTypeByCountry WHERE CountryId = @USAId AND DocumentTypeId = @USAPPUSId
)
BEGIN
    INSERT INTO DocumentTypeByCountry 
        (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
    VALUES
        (@USAId, @USAPPUSId, GETDATE(), NULL, 1, 0);
END

UPDATE DOCUMENTTYPE
SET   RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPDE' AND UPPER(C.NameESP) = 'ALEMANIA')
)

UPDATE DOCUMENTTYPE
SET   RegularExpression =   '^\d+$' , Code = 'DUI'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'DIU' AND  UPPER(C.NameESP) = 'EL SALVADOR')
)

DECLARE @ArgentinaId INT;
DECLARE @PPAId INT;

-- Obtener CountryId de Argentina
SELECT @ArgentinaId = CountryId FROM COUNTRY WHERE UPPER(NAMEESP) = 'ARGENTINA';

-- Solo insertar el DocumentType si no existe
IF NOT EXISTS (
    SELECT 1  FROM DOCUMENTTYPE WHERE UPPER(CODE) = 'PPA'
)
BEGIN
    INSERT INTO DocumentType
        (CODE, [NAME], [LENGTH], REGULAREXPRESSION, CREATEDDATE, UPDATEDDATE, ACTIVE, ISDELETED, MINLENGTH, [MAXLENGTH])
    VALUES
        ('PPA','Pasaporte Argentino', 0, '^[a-zA-Z0-9]+$', GETDATE(), NULL, 1, 0, 6, 9);
END

-- Obtener DocumentTypeId recién creado o existente
SELECT @PPAId = DocumentTypeId FROM DOCUMENTTYPE WHERE UPPER(CODE) = 'PPA';

-- Solo insertar en DocumentTypeByCountry si no existe
IF NOT EXISTS (
    SELECT 1 FROM DocumentTypeByCountry WHERE CountryId = @ArgentinaId AND DocumentTypeId = @PPAId
)
BEGIN
    INSERT INTO DocumentTypeByCountry
        (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
    VALUES
        (@ArgentinaId, @PPAId, GETDATE(), NULL, 1, 0);
END


UPDATE DOCUMENTTYPE
SET  Code = 'PPO', MinLength = 6 , [MaxLength] = 16 , RegularExpression =  '^[a-zA-Z0-9]+$'  , UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (
		SELECT 
		DT.DocumentTypeId 
		FROM DocumentType DT
		INNER JOIN DOCUMENTTYPEBYCOUNTRY DTC ON (DT.DocumentTypeId = DTC.DocumentTypeId)
		INNER JOIN COUNTRY C ON (C.CountryId = DTC.CountryId)
		INNER JOIN REGION R ON (R.RegionId = c.RegionId)
		WHERE 
		(( DTC.ACTIVE =1 AND DTC.ISDELETED = 0)  AND (C.Active = 1 AND C.IsDeleted =0 ) AND (DT.ACTIVE =1 AND DT.IsDeleted = 0) AND (R.ACTIVE= 1 AND R.IsDeleted = 0))
		AND (DT.Code = 'PPGE' )
)

UPDATE  DocumentTypeByCountry 
SET ACTIVE = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE COUNTRYID IN  ( SELECT COUNTRYID FROM Country WHERE UPPER(NAMEESP) IN ('ARGENTINA' ))
AND (DocumentTypeId IN (SELECT DocumentTypeId from DocumentType where code  ='PPGE'))
AND ( ACTIVE= 1 AND  IsDeleted = 0)


UPDATE  DocumentTypeByCountry 
SET ACTIVE = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE COUNTRYID IN  ( SELECT COUNTRYID FROM Country WHERE UPPER(NAMEESP) IN ('ESTADOS UNIDOS'))
AND (DocumentTypeId IN (SELECT DocumentTypeId from DocumentType where code  ='PPES'))
AND ( ACTIVE= 1 AND  IsDeleted = 0)
            ");
        }


    }
}
