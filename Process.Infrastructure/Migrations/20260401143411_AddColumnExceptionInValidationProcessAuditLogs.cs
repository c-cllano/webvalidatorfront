using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnExceptionInValidationProcessAuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('dbo.ValidationProcessAuditLogs', 'Exception') IS NULL
                BEGIN
                    ALTER TABLE dbo.ValidationProcessAuditLogs
                    ADD Exception VARCHAR(MAX) NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE dbo.ValidationProcessAuditLogs
                DROP COLUMN Exception;
            ");
        }
    }
}
