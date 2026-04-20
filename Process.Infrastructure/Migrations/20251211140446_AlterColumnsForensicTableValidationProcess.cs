using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnsForensicTableValidationProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('dbo.ValidationProcess', 'ForensicState') IS NULL
                BEGIN
                    ALTER TABLE dbo.ValidationProcess
                    ADD ForensicState INT NULL;
                END

                IF COL_LENGTH('dbo.ValidationProcess', 'ForensicReason') IS NULL
                BEGIN
                    ALTER TABLE dbo.ValidationProcess
                    ADD ForensicReason VARCHAR(300) NULL;
                END

                IF COL_LENGTH('dbo.ValidationProcess', 'ForensicOptionalReason') IS NULL
                BEGIN
                    ALTER TABLE dbo.ValidationProcess
                    ADD ForensicOptionalReason VARCHAR(300) NULL;
                END

                IF COL_LENGTH('dbo.ValidationProcess', 'ForensicObservations') IS NULL
                BEGIN
                    ALTER TABLE dbo.ValidationProcess
                    ADD ForensicObservations VARCHAR(300) NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE dbo.ValidationProcess
                DROP COLUMN ForensicState;

                ALTER TABLE dbo.ValidationProcess
                DROP COLUMN ForensicReason;

                ALTER TABLE dbo.ValidationProcess
                DROP COLUMN ForensicOptionalReason;

                ALTER TABLE dbo.ValidationProcess
                DROP COLUMN ForensicObservations;
            ");
        }
    }
}
