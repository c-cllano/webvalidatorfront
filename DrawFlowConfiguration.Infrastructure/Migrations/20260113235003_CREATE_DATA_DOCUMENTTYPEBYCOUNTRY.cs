using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_DATA_DOCUMENTTYPEBYCOUNTRY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"-- 1. Definimos la lista de países y documentos en una tabla temporal
                DECLARE @PaisesDocumentos TABLE (
                    NombreESP VARCHAR(100),
                    NombreENG VARCHAR(100),
                    SiglaDoc VARCHAR(10),
                    NombreDoc VARCHAR(150),
                    Indicativo VARCHAR(15),
                    RegionNombre VARCHAR(75),
                    FlagURL VARCHAR(150)
                );

                -- 2. AQUÍ AGREGAS TODOS LOS QUE NECESITES
                INSERT INTO @PaisesDocumentos VALUES 
                --ALEMANIA
                ('Alemania', 'Germany',  'PPDE', 'Pasaporte', '+49', 'Europa', 'https://flagcdn.com/de.svg'),
                --ARGENTINA
                ('Argentina', 'Argentina',  'DNIA', 'Documento Nacional de Identidad', '+54', 'América del Sur', 'https://flagcdn.com/ar.svg'),
                ('Argentina', 'Argentina',  'CUIT', 'Código Único de Identificación Tributaria', '+54', 'América del Sur', 'https://flagcdn.com/ar.svg'),
                ('Argentina', 'Argentina',  '', 'Sin documento', '+54', 'América del Sur', 'https://flagcdn.com/ar.svg'),
                -- BOLIVIA
                ('Bolivia', 'Bolivia',  'CIB', 'Cédula de Identidad', '+591', 'América del Sur', 'https://flagcdn.com/bo.svg'),
                ('Bolivia', 'Bolivia',  '', 'Sin documento', '+591', 'América del Sur', 'https://flagcdn.com/bo.svg'),
                --BRASIL
                ('Brasil', 'Brazil',  'RG', 'Registro General', '+55', 'América del Sur', 'https://flagcdn.com/br.svg'),
                ('Brasil', 'Brazil',  'CPF', 'CPF', '+55', 'América del Sur', 'https://flagcdn.com/br.svg'),
                ('Brasil', 'Brazil',  '', 'Sin documento', '+55', 'América del Sur', 'https://flagcdn.com/br.svg'),
                ('Brasil', 'Brazil',  'CNPJ', 'CNPJ', '+55', 'América del Sur', 'https://flagcdn.com/br.svg'),
                --CHILE
                ('Chile', 'Chile',  'CIP', 'Cedula de identidad', '+34', 'América del Sur', 'https://flagcdn.com/cl.svg'),
                ('Chile', 'Chile',  'RUT', 'Rol unico tributario', '+34', 'América del Sur', 'https://flagcdn.com/cl.svg'),
                ('Chile', 'Chile',  'RUN', 'Rol unico nacional', '+34', 'América del Sur', 'https://flagcdn.com/cl.svg'),
                ('Chile', 'Chile',  'PPC', 'Pasaporte', '+34', 'América del Sur', 'https://flagcdn.com/cl.svg'),
                ('Chile', 'Chile',  '', 'Sin documento', '+34', 'América del Sur', 'https://flagcdn.com/cl.svg'),
                -- COLOMBIA
                ('Colombia', 'Colombia',  'CC', 'Cedula de identidad', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'TI', 'Tarjeta de identidad', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'RC', 'Registro Civil', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'CE', 'Cedula Extranjera', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'PP', 'Pasaporte', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'CO', 'Contraseña cédula de ciudadanía', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'CA', 'Contraseña cédula de extranjería', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'TR', 'Tarjeta de identidad rosada', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'N', 'NIT', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'PEP', 'Permiso Especial de Permanencia', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'RUMV', 'Registro Único de Migrantes Venezolanos', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'CP', 'Chip predio', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'PPGE', 'Pasaporte Generíco', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  'GEN', 'Documento Generico', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                ('Colombia', 'Colombia',  '', 'Sin documento', '+57', 'América del Sur', 'https://flagcdn.com/co.svg'),
                -- COSTA RICA
                ('Costa Rica', 'Costa Rica',  'CI', 'Cédula de identidad', '+506', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/cr.svg'),
                ('Costa Rica', 'Costa Rica',  'PPR', 'Pasaporte Costa Rica', '+506', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/cr.svg'),
                ('Costa Rica', 'Costa Rica',  '', 'Sin documento', '+506', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/cr.svg'),
                -- ECUADOR
                ('Ecuador', 'Ecuador',  'CCE', 'Cedula', '+593', 'América del Sur', 'https://flagcdn.com/ec.svg'),
                ('Ecuador', 'Ecuador',  'PPE', 'Pasaporte', '+593', 'América del Sur', 'https://flagcdn.com/ec.svg'),
                ('Ecuador', 'Ecuador',  '', 'Sin documento', '+593', 'América del Sur', 'https://flagcdn.com/ec.svg'),
                -- EL SALVADOR
                ('El Salvador', 'El Salvador',  'DIU', 'Documento Único de Identidad', '+503', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/sv.svg'),
                ('El Salvador', 'El Salvador',  'PPS', 'Pasaporte El Salvador', '+503', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/sv.svg'),
                ('El Salvador', 'El Salvador',  '', 'Sin documento', '+503', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/sv.svg'),
                -- ESPAÑA
                ('España', 'Spain',  'PPES', 'Pasaporte', '+34', 'Europa', 'https://flagcdn.com/es.svg'),
                -- ESTADOS UNIDOS
                ('Estados Unidos', 'United States',  'PPES', 'Pasaporte', '+1', 'Europa', 'https://flagcdn.com/us.svg'),
                -- GUATEMALA
                ('Guatemala', 'Guatemala',  'DPI', 'Documento Personal de Identificación', '+502', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/gt.svg'),
                ('Guatemala', 'Guatemala',  'PPG', 'Pasaporte Guatemala', '+502', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/gt.svg'),
                ('Guatemala', 'Guatemala',  '', 'Sin documento', '+502', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/gt.svg'),
                -- HONDURAS
                ('Honduras', 'Honduras',  'DNIH', 'Documento Nacional de Identificación', '+502', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/hn.svg'),
                ('Honduras', 'Honduras',  '', 'Sin documento', '+502', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/hn.svg'),
                -- MEXICO
                ('México', 'Mexico',  'CURP', 'CURP', '+52', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/mx.svg'),
                ('México', 'Mexico',  'PPM', 'Pasaporte', '+52', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/mx.svg'),
                ('México', 'Mexico',  'RFC', 'RFC', '+52', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/mx.svg'),
                ('México', 'Mexico',  'LICF', 'Licencia federal', '+52', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/mx.svg'),
                ('México', 'Mexico',  'MEPV', 'Medida preventiva', '+52', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/mx.svg'),
                ('México', 'Mexico',  '', 'Sin documento', '+52', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/mx.svg'),
                -- PANAMA
                ('Panamá', 'Panama',  'CIPP', 'Cedula de identidad', '+507', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/pa.svg'),
                ('Panamá', 'Panama',  'PPP', 'Pasaporte', '+507', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/pa.svg'),
                ('Panamá', 'Panama',  '', 'Sin documento', '+507', 'América del Norte Centro y El Caribe', 'https://flagcdn.com/pa.svg'),
                -- PARAGUAY
                ('Paraguay', 'Paraguay',  'CIPR', 'Cédula de Identidad', '+595', 'América del Sur', 'https://flagcdn.com/py.svg'),
                ('Paraguay', 'Paraguay',  'CCP', 'Cédula Paraguay', '+595', 'América del Sur', 'https://flagcdn.com/py.svg'),
                ('Paraguay', 'Paraguay',  '', 'Sin documento', '+595', 'América del Sur', 'https://flagcdn.com/py.svg'),
                -- PERU
                ('Perú', 'Peru',  'DNIP', 'Documento Nacional de Identidad', '+51', 'América del Sur', 'https://flagcdn.com/pe.svg'),
                ('Perú', 'Peru',  'CEP', 'Carnet de extranjeria', '+51', 'América del Sur', 'https://flagcdn.com/pe.svg'),
                ('Perú', 'Peru',  'RUC', 'Registro unico de contribuyentes', '+51', 'América del Sur', 'https://flagcdn.com/pe.svg'),
                ('Perú', 'Peru',  'PSP', 'Pasaporte', '+51', 'América del Sur', 'https://flagcdn.com/pe.svg'),
                ('Perú', 'Peru',  '', 'Sin documento', '+51', 'América del Sur', 'https://flagcdn.com/pe.svg'),
                -- URUGUAY
                ('Uruguay', 'Uruguay',  '', 'Sin documento', '+598', 'América del Sur', 'https://flagcdn.com/uy.svg'),
                -- VENEZUELA
                ('Venezuela', 'Venezuela',  'PPV', 'Pasaporte', '+58', 'América del Sur', 'https://flagcdn.com/ve.svg'),
                ('Venezuela', 'Venezuela',  'CCV', 'Cedula', '+58', 'América del Sur', 'https://flagcdn.com/ve.svg'),
                ('Venezuela', 'Venezuela',  '', 'Sin documento', '+58', 'América del Sur', 'https://flagcdn.com/ve.svg')

                --Otro	PPO	Pasaporte

                -- 3. CURSOR PARA RECORRER LA LISTA
                DECLARE @NombreESP VARCHAR(100), @NombreENG VARCHAR(100), @SiglaDoc VARCHAR(5), 
                        @NombreDoc VARCHAR(150), @Indicativo VARCHAR(15), @RegionNombre VARCHAR(75), @FlagURL VARCHAR(150);

                DECLARE pais_cursor CURSOR FOR 
                SELECT * FROM @PaisesDocumentos;

                OPEN pais_cursor;
                FETCH NEXT FROM pais_cursor INTO @NombreESP, @NombreENG, @SiglaDoc, @NombreDoc, @Indicativo, @RegionNombre, @FlagURL;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    DECLARE @CurrentIdPais INT;
                    DECLARE @CurrentIdDoc INT;
                    DECLARE @CurrentIdRegion INT;

                    -- Lógica de País
                    SET @CurrentIdPais = (SELECT CountryId FROM dbo.Country WHERE UPPER(NameESP) = UPPER(@NombreESP));
    
                    IF (@CurrentIdPais IS NULL)
                    BEGIN
                        SET @CurrentIdRegion = (SELECT RegionId FROM dbo.Region WHERE UPPER([NAME]) = UPPER(@RegionNombre) AND Active = 1);
        
                        INSERT INTO dbo.Country ([Name], Indicative, CreatedDate, Active, IsDeleted , RegionId, NameESP, Flag)
                        VALUES (@NombreENG, @Indicativo, GETDATE(), 1, 0, @CurrentIdRegion, @NombreESP, @FlagURL);
        
                        SET @CurrentIdPais = SCOPE_IDENTITY();
                    END

                    -- Lógica de Documento
                    SET @CurrentIdDoc = (SELECT DocumentTypeId FROM dbo.DocumentType WHERE UPPER(Code) = UPPER(@SiglaDoc));
    
                    IF (@CurrentIdDoc IS NULL)
                    BEGIN
                        INSERT INTO dbo.DocumentType (Code, [Name], CreatedDate, Active, IsDeleted)
                        VALUES (@SiglaDoc, @NombreDoc, GETDATE(), 1, 0);
        
                        SET @CurrentIdDoc = SCOPE_IDENTITY();
                    END

                    -- Relación (solo si no existe)
                    IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeByCountry WHERE CountryId = @CurrentIdPais AND DocumentTypeId = @CurrentIdDoc)
                    BEGIN
                        INSERT INTO dbo.DocumentTypeByCountry (CountryId, DocumentTypeId, CreatedDate, Active, IsDeleted)
                        VALUES (@CurrentIdPais, @CurrentIdDoc, GETDATE(), 1, 0);
                    END

                    FETCH NEXT FROM pais_cursor INTO @NombreESP, @NombreENG, @SiglaDoc, @NombreDoc, @Indicativo, @RegionNombre, @FlagURL;
                END

                CLOSE pais_cursor;
                DEALLOCATE pais_cursor;

                PRINT 'Proceso completado para todos los países.';
                ");
        }
    }
}
