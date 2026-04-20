using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_ALTER_TABLE_DocumentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
                           IF COL_LENGTH('dbo.DocumentType', 'RegularExpression') IS NOT NULL
                           BEGIN
                               ALTER TABLE dbo.DocumentType
                               ALTER COLUMN [RegularExpression] NVARCHAR(MAX);
                           END

                     ");
        }

    }
}
