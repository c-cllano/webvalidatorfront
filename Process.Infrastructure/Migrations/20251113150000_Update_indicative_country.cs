using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_indicative_country : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
                    UPDATE country
                    SET indicative = '+' + indicative
                    WHERE indicative NOT LIKE '+%';
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@" 
                        IF EXISTS (
                            SELECT 1
                            FROM country
                            WHERE indicative LIKE '+%'
                        )
                        BEGIN

                            UPDATE country
                            SET indicative = SUBSTRING(indicative, 2, LEN(indicative))
                            WHERE indicative LIKE '+%';
                        END;
            ");

        }
    }
}
