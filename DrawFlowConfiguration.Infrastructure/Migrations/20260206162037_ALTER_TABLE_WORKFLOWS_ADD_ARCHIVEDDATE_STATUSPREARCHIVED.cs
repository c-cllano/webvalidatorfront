using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawFlowConfiguration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ALTER_TABLE_WORKFLOWS_ADD_ARCHIVEDDATE_STATUSPREARCHIVED : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF NOT EXISTS (
            SELECT 1
            FROM sys.columns
            WHERE Name = 'ArchivedDate'
              AND Object_ID = OBJECT_ID('Workflows')
        )
        BEGIN
            ALTER TABLE Workflows
            ADD ArchivedDate DATE NOT NULL
                CONSTRAINT DF_Workflows_ArchivedDate DEFAULT ('1900-01-01');
        END
    ");

            migrationBuilder.Sql(@"
        IF NOT EXISTS (
            SELECT 1
            FROM sys.columns
            WHERE Name = 'StatusPreArchived'
              AND Object_ID = OBJECT_ID('Workflows')
        )
        BEGIN
            ALTER TABLE Workflows
            ADD StatusPreArchived INT NOT NULL
                CONSTRAINT DF_Workflows_StatusPreArchived DEFAULT (0);
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT 1
            FROM sys.columns
            WHERE Name = 'ArchivedDate'
              AND Object_ID = OBJECT_ID('Workflows')
        )
        BEGIN
            ALTER TABLE Workflows
            DROP CONSTRAINT DF_Workflows_ArchivedDate;
            ALTER TABLE Workflows
            DROP COLUMN ArchivedDate;
        END
    ");

            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT 1
            FROM sys.columns
            WHERE Name = 'StatusPreArchived'
              AND Object_ID = OBJECT_ID('Workflows')
        )
        BEGIN
            ALTER TABLE Workflows
            DROP CONSTRAINT DF_Workflows_StatusPreArchived;
            ALTER TABLE Workflows
            DROP COLUMN StatusPreArchived;
        END
    ");
        }

    }
}
