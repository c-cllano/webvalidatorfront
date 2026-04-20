using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_COUNTRY_COLUMN_FRECUENT_COUNTRY : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
                UPDATE country
                SET frecuentCountry = 1
                WHERE LOWER(nameesp) IN ('mexico', 'méxico', 'colombia');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                update country set frecuentCountry=0 ; -- false por defecto
            ");
        }

    }
}
