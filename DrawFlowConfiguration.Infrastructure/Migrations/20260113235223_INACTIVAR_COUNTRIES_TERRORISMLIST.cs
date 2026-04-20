using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class INACTIVAR_COUNTRIES_TERRORISMLIST : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@" UPDATE COUNTRY SET ACTIVE = 0, ISDELETED = 1 WHERE UPPER(NAMEESP) 
                IN('IRÁN', 'COREA DEL NORTE', 'SIRIA', 'RUSIA', 'CUBA', 'AFGANISTÁN');  ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" UPDATE COUNTRY SET ACTIVE = 1, ISDELETED = 0 WHERE UPPER(NAMEESP) 
                IN('IRÁN', 'COREA DEL NORTE', 'SIRIA', 'RUSIA', 'CUBA', 'AFGANISTÁN');  ");
        }
    }
}
