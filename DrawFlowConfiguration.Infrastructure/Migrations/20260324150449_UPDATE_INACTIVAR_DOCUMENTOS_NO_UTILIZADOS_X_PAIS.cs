using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_INACTIVAR_DOCUMENTOS_NO_UTILIZADOS_X_PAIS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
/*************** COLOMBIA *********/

DECLARE @IDPAIS NVARCHAR(80);
SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'COLOMBIA';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarCol
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
          'Cédula de Ciudadanía', 
          'Cédula extranjería', 
          'Pasaporte', 
          'Permiso Especial de Permanencia', 
          'Permiso por Protección Temporal'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarCol);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarCol);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarCol;


/*************** MÉXICO *********/

SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'MÉXICO';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarMx
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'CURP' , 'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarMx);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarMx);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarMx;



/*************** ECUADOR *********/


SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'ECUADOR';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarEcu
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Cedula' , 'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarEcu);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarEcu);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarEcu;


/*************** ARGENTINA *********/


SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'ARGENTINA';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarArg
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Documento Nacional de Identidad' , 'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarArg);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarArg);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarArg;



/*************** PERÚ *********/

SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'PERÚ';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarPer
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
       'Documento Nacional de Identidad' , 'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarPer);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarPer);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarPer;



/*************** PANAMÁ *********/

SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'PANAMÁ';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarPan
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
       'Cedula de identidad' , 'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarPan);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarPan);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarPan;


/*************** CHILE *********/


SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'CHILE';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarChi
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Rol unico tributario' , 'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarChi);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarChi);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarChi;


/*************** BRASIL *********/


SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'BRASIL';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarBra
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Registro General'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarBra);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarBra);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarBra;



/*************** VENEZUELA *********/


SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'VENEZUELA';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarVen
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarVen);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarVen);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarVen;



/*************** GUATEMALA *********/


SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'GUATEMALA';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarGua
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Documento Personal de Identificación' , 'Pasaporte Guatemala'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarGua);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarGua);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarGua;



/*************** EL SALVADOR  *********/

SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'EL SALVADOR';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarElSa
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Documento Único de Identidad' , 'Pasaporte El Salvador'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarElSa);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarElSa);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarElSa;


/*************** COSTA RICA  *********/


SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'COSTA RICA';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarCR
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Cédula de identidad' , 'Pasaporte Costa Rica'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarCR);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarCR);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarCR;



/*************** HONDURAS  *********/

SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'HONDURAS';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarHon
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Documento Nacional de Identificación'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarHon);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarHon);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarHon;


/*************** ESTADOS UNIDOS  *********/

SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'ESTADOS UNIDOS';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarUSA
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarUSA);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarUSA);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarUSA;


/*************** ESPAÑA  *********/

SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'ESPAÑA';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarEsp
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarEsp);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarEsp);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarEsp;


/*************** ALEMANIA  *********/

SELECT @IDPAIS = COUNTRYID 
FROM COUNTRY 
WHERE UPPER(NAMEESP) = 'ALEMANIA';

-- 1. Guardamos los IDs a inactivar en una tabla temporal
SELECT DOCUMENTTYPEID 
INTO #IdsAInactivarAle
FROM DOCUMENTTYPEBYCOUNTRY 
WHERE COUNTRYID = @IDPAIS 
  AND ACTIVE = 1 
  AND ISDELETED = 0
  AND DOCUMENTTYPEID NOT IN (
      SELECT DOCUMENTTYPEID FROM DOCUMENTTYPE
      WHERE [NAME] IN (
        'Pasaporte'
      )
  );

-- 2. Primer UPDATE (Ahora sí encuentra los datos)
UPDATE DocumentType
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarAle);

-- 3. Segundo UPDATE (La tabla temporal sigue existiendo)
UPDATE DocumentTypeByCountry 
SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
WHERE CountryId = @IDPAIS 
  AND DocumentTypeId IN (SELECT DocumentTypeId FROM #IdsAInactivarAle);

-- 4. Limpieza (Opcional, se borra sola al terminar la sesión)
DROP TABLE #IdsAInactivarAle;
");

        }

    

    }
}
