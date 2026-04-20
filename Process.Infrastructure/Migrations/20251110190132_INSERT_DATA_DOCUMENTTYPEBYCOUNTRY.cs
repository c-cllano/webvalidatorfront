using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class INSERT_DATA_DOCUMENTTYPEBYCOUNTRY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 1 , GETDATE() , GETDATE() , 1, 0)


                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 2 , GETDATE() , GETDATE() , 1, 0)


                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 3 , GETDATE() , GETDATE() , 1, 0)

                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 4 , GETDATE() , GETDATE() , 1, 0)

                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 5 , GETDATE() , GETDATE() , 1, 0)

                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 6 , GETDATE() , GETDATE() , 1, 0)

                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 7 , GETDATE() , GETDATE() , 1, 0)

                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 8 , GETDATE() , GETDATE() , 1, 0)

                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 9 , GETDATE() , GETDATE() , 1, 0)

                insert into DocumentTypeByCountry
                (CountryId, DocumentTypeId, CreatedDate, UpdatedDate, Active, IsDeleted)
                values ( 36, 10 , GETDATE() , GETDATE() , 1, 0)
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                DELETE FROM DocumentTypeByCountry;
            ");
        }

    }
}
