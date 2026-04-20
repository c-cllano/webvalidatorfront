using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_REGION_BY_COUNTRY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

              --ame sur
                update Country set RegionId= 1 where CountryId in(6,20,23,34,36,49,70,134,135,165,186,190);
                --ame nte cent caribe
                update country set RegionId= 2 where CountryId in(11,14,17,30,40,42,47,48,51,66,67,71,72,83,109,123,132,143,144,145,176,180,185);
                --oceania
                update country set RegionId= 3 where CountryId in(8,57,88,106,110,119,122,131,133,146,157,173,175,188);
                --africa
                update country set RegionId= 4 where CountryId in(3,5,18,22,26,27,29,31,32,33,37,38,39,46,50,52,53,55,56,60,61,64,68,69,82,87,94,95,96,100,101,104,107,108,115,116,118,124,125,142,148,150,152,153,
                158,159,161,164,171,174,177,181,193,194);
                --asia
                update country set RegionId= 5 where CountryId in(1,7,10,12,13,19,24,28,35,43,62,75,76,77,78,80,84,85,86,89,90,91,93,102,103,113,117,120,126,129,130,136,139,149,154,160,163,168,169,170,172,178,179,183,187,191,192);
                --europa
                update country set RegionId= 6 where CountryId in(2,4,9,15,16,21,25,41,44,45,54,58,59,63,65,73,74,79,81,92,97,98,99,105,111,112,114,121,127,128,137,138,140,141,147,151,155,156,
                162,166,167,182,184,189);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                update country set Regionid=1 ; -- region por defecto
            ");
        }
    }
}
