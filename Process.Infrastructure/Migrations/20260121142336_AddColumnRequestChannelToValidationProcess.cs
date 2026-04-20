using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnRequestChannelToValidationProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('dbo.ValidationProcess', 'RequestChannel') IS NULL
                BEGIN
                    ALTER TABLE dbo.ValidationProcess
                    ADD RequestChannel TINYINT NOT NULL
                    CONSTRAINT DF_ValidationProcess_RequestChannel DEFAULT (0);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE dbo.ValidationProcess
                DROP COLUMN RequestChannel;
            ");
        }
    }
}
