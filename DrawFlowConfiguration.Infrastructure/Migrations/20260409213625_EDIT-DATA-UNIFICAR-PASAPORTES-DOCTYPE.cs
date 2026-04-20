using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EDITDATAUNIFICARPASAPORTESDOCTYPE : Migration
    {

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
    /*** SE INACTIVAN DE LOS PAISES QUE SE HABIAN CREADO LOS PASAPORTES INDEPENDIENTES ***/
    UPDATE DocumentTypeByCountry
    SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
    WHERE DocumentTypeByCountryId IN (
        SELECT DocumentTypeByCountryId
        FROM DocumentTypeByCountry
        WHERE CountryId IN (
            SELECT CountryId 
            FROM Country
            WHERE UPPER(NameESP) IN (
                'COLOMBIA','MÉXICO','ECUADOR','ARGENTINA','PERÚ',
                'PANAMÁ','CHILE','BRASIL','VENEZUELA','GUATEMALA',
                'EL SALVADOR','COSTA RICA','HONDURAS','ESTADOS UNIDOS',
                'ESPAÑA','ALEMANIA'
            )
        )
        AND DocumentTypeId IN (
            SELECT DocumentTypeId 
            FROM DocumentType
            WHERE Code IN (
                'PP','PPM','PPE','PPA','PSP',
                'PPP','PPC','PPV','PPG','PPS',
                'PPR','PPUS','PPES','PPDE'
            )
        )
        AND Active = 1 AND IsDeleted = 0
    );

    /*** SE INACTIVAN LOS PASAPORTES QUE ESTABAN 'SUELTOS' ***/
    UPDATE DocumentType
    SET Active = 0, IsDeleted = 1, UpdatedDate = GETDATE()
    WHERE Code IN (
        'PP','PPM','PPE','PPA','PSP',
        'PPP','PPC','PPV','PPG','PPS',
        'PPR','PPUS','PPES','PPDE'
    );

    /*** SE INSERTAN LOS DOCUMENTOS - PAIS RELACIONADOS A PASAPORTE 'GENERICO' ***/
    INSERT INTO DocumentTypeByCountry (
        CountryId,
        DocumentTypeId,
        Active,
        IsDeleted
    )
    SELECT 
        c.CountryId,
        dt.DocumentTypeId,
        1,
        0
    FROM Country c
    INNER JOIN DocumentType dt 
        ON dt.Code = 'PPO'
    WHERE UPPER(c.NameESP) IN (
        'COLOMBIA','MÉXICO','ECUADOR','ARGENTINA','PERÚ',
        'PANAMÁ','CHILE','BRASIL','VENEZUELA','GUATEMALA',
        'EL SALVADOR','COSTA RICA','HONDURAS','ESTADOS UNIDOS',
        'ESPAÑA','ALEMANIA'
    )
    AND NOT EXISTS (
        SELECT 1
        FROM DocumentTypeByCountry d
        WHERE d.CountryId = c.CountryId
          AND d.DocumentTypeId = dt.DocumentTypeId
          AND d.Active = 1
          AND d.IsDeleted = 0
    );
    ");
        }


    }
}
