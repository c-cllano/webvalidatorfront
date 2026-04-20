using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_REGEXP_DOCXPAIS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DECLARE @IDPAIS NVARCHAR(80);

/** COLOMBIA **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'COLOMBIA';
UPDATE DocumentType SET [Length] = 0, MinLength = 3, MaxLength = 10, RegularExpression = '^[1-9][0-9]{2,9}$' 
WHERE [NAME] = 'Cédula de Ciudadanía' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

UPDATE DocumentType SET [Length] = 0, MinLength = 5, MaxLength = 10, RegularExpression = '^[a-zA-Z0-9]{5,10}$' 
WHERE [NAME] = 'Cédula extranjería' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

UPDATE DocumentType SET [Length] = 0, MinLength = 6, MaxLength = 16, RegularExpression = '^[a-zA-Z0-9]{6,16}$' 
WHERE [NAME] = 'Pasaporte' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

UPDATE DocumentType SET [Length] = 0, MinLength = 15, MaxLength = 15, RegularExpression = '^[0-9]{15}$' 
WHERE [NAME] = 'Permiso Especial de Permanencia' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

UPDATE DocumentType SET [Length] = 0, MinLength = 7, MaxLength = 8, RegularExpression = '^[0-9]{7,8}$' 
WHERE [NAME] = 'Permiso por Protección Temporal' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** MÉXICO **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'MÉXICO';
UPDATE DocumentType SET [Length] = 0, MinLength = 18, MaxLength = 18, RegularExpression = '^[A-Z]{4}[0-9]{6}[H,M][A-Z]{5}[A-Z0-9]{1}[0-9]{1}$' 
WHERE [NAME] = 'CURP' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

UPDATE DocumentType SET [Length] = 0, MinLength = 8, MaxLength = 9, RegularExpression = '^[a-zA-Z0-9]{8,9}$' 
WHERE [NAME] = 'Pasaporte' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** ECUADOR **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'ECUADOR';
UPDATE DocumentType SET [Length] = 0, MinLength = 10, MaxLength = 10, RegularExpression = '^([0-2][0-9])[0-9]{8}$' 
WHERE [NAME] = 'Cedula' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

UPDATE DocumentType SET [Length] = 0, MinLength = 6, MaxLength = 12, RegularExpression = '^[a-zA-Z0-9]{6,12}$' 
WHERE [NAME] = 'Pasaporte' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** ARGENTINA **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'ARGENTINA';
UPDATE DocumentType SET [Length] = 0, MinLength = 7, MaxLength = 8, RegularExpression = '^[0-9]{7,8}$' 
WHERE [NAME] = 'Documento Nacional de Identidad' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** PERÚ **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'PERÚ';
UPDATE DocumentType SET [Length] = 0, MinLength = 8, MaxLength = 8, RegularExpression = '^[0-9]{8}$' 
WHERE [NAME] = 'Documento Nacional de Identidad' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

UPDATE DocumentType SET [Length] = 0, MinLength = 9, MaxLength = 9, RegularExpression = '^[a-zA-Z0-9]{9}$' 
WHERE [NAME] = 'Pasaporte' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** PANAMÁ **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'PANAMÁ';
UPDATE DocumentType SET [Length] = 0, MinLength = 7, MaxLength = 13, RegularExpression = '^[0-9]{7,13}$' 
WHERE [NAME] = 'Cedula de identidad' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

UPDATE DocumentType SET [Length] = 0, MinLength = 8, MaxLength = 9, RegularExpression = '^[a-zA-Z0-9]{8,9}$' 
WHERE [NAME] = 'Pasaporte' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** CHILE **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'CHILE';
UPDATE DocumentType SET [Length] = 0, MinLength = 9, MaxLength = 10, RegularExpression = '^[0-9]{7,8}-[0-9kK]{1}$' 
WHERE [NAME] = 'Rol unico tributario' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** BRASIL **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'BRASIL';
UPDATE DocumentType SET [Length] = 0, MinLength = 8, MaxLength = 9, RegularExpression = '^[0-9]{7,8}[0-9xX]{1}$' 
WHERE [NAME] = 'Registro General' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** GUATEMALA **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'GUATEMALA';
UPDATE DocumentType SET [Length] = 0, MinLength = 13, MaxLength = 13, RegularExpression = '^[0-9]{13}$' 
WHERE [NAME] = 'Documento Personal de Identificación' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** EL SALVADOR **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'EL SALVADOR';
UPDATE DocumentType SET [Length] = 0, MinLength = 9, MaxLength = 9, RegularExpression = '^[0-9]{9}$' 
WHERE [NAME] = 'Documento Único de Identidad' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** COSTA RICA **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'COSTA RICA';
UPDATE DocumentType SET [Length] = 0, MinLength = 9, MaxLength = 9, RegularExpression = '^[1-9][0-9]{8}$' 
WHERE [NAME] = 'Cédula de identidad' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** HONDURAS **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'HONDURAS';
UPDATE DocumentType SET [Length] = 0, MinLength = 13, MaxLength = 13, RegularExpression = '^[0-9]{13}$' 
WHERE [NAME] = 'Documento Nacional de Identificación' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** ESPAÑA **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'ESPAÑA';
UPDATE DocumentType SET [Length] = 0, MinLength = 9, MaxLength = 9, RegularExpression = '^[a-zA-Z]{2}[0-9]{7}$' 
WHERE [NAME] = 'Pasaporte' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);

/** ALEMANIA **/
SELECT @IDPAIS = COUNTRYID FROM COUNTRY WHERE UPPER(NAMEESP) = 'ALEMANIA';
UPDATE DocumentType SET [Length] = 0, MinLength = 9, MaxLength = 9, RegularExpression = '^[CFGHJKLMNPRSTVWXYZ0-9]{9}$' 
WHERE [NAME] = 'Pasaporte' 
AND DocumentTypeId IN (SELECT DocumentTypeId FROM DOCUMENTTYPEBYCOUNTRY WHERE CountryId = @IDPAIS);
");
        }
    }
}
