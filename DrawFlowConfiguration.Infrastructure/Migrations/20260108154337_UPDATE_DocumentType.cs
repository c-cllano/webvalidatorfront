using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_DocumentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                            update DocumentType
                             set [Length] = 18, RegularExpression = '^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9][12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$'
                             where Code = 'CURP'
                             
                             update DocumentType
                             set RegularExpression = '^\d{7,8}[0-9Kk]$'
                             where Code = 'CIP'
                             
                             update DocumentType
                             set RegularExpression = '^\d{5,12}$'
                             where Code = 'CIPP'

                            update DocumentType
                            set RegularExpression = '^([0-9]{10,11})$'
                            where Code = 'TI'

                     ");
        }

       
    }
}
