using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsUrlReconoserInAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('dbo.Agreement', 'ChangeUrl') IS NULL
                BEGIN
                    ALTER TABLE dbo.Agreement
                    ADD ChangeUrl BIT NULL;
                END

                IF COL_LENGTH('dbo.Agreement', 'BaseUrlReconoser1') IS NULL
                BEGIN
                    ALTER TABLE dbo.Agreement
                    ADD BaseUrlReconoser1 VARCHAR(MAX) NULL;
                END

                IF COL_LENGTH('dbo.Agreement', 'BaseUrlReconoser2') IS NULL
                BEGIN
                    ALTER TABLE dbo.Agreement
                    ADD BaseUrlReconoser2 VARCHAR(MAX) NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE dbo.Agreement
                DROP COLUMN ChangeUrl;

                ALTER TABLE dbo.Agreement
                DROP COLUMN BaseUrlReconoser1;

                ALTER TABLE dbo.Agreement
                DROP COLUMN BaseUrlReconoser2;
            ");
        }
    }
}
