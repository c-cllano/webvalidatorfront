using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnJsonTrazabilityInValidationProcessDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sysobjects WHERE name='ValidationProcessDocuments' AND xtype='U')
                BEGIN
                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'Trazability') IS NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        ADD Trazability VARCHAR(MAX) NULL;
                    END

                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'UrlFile') IS NOT NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        DROP COLUMN UrlFile;
                    END

                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'ProcessName') IS NOT NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        DROP COLUMN ProcessName;
                    END

                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'ServiceType') IS NOT NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        DROP COLUMN ServiceType;
                    END

                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'ServiceSubType') IS NOT NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        DROP COLUMN ServiceSubType;
                    END
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sysobjects WHERE name='ValidationProcessDocuments' AND xtype='U')
                BEGIN
                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'Trazability') IS NOT NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        DROP COLUMN Trazability;
                    END

                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'UrlFile') IS NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        ADD UrlFile VARCHAR(MAX) NULL;
                    END

                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'ProcessName') IS NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        ADD ProcessName VARCHAR(255) NULL;
                    END

                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'ServiceType') IS NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        ADD ServiceType INT NULL;
                    END

                    IF COL_LENGTH('dbo.ValidationProcessDocuments', 'ServiceSubType') IS NULL
                    BEGIN
                        ALTER TABLE dbo.ValidationProcessDocuments
                        ADD ServiceSubType INT NULL;
                    END
                END
            ");
        }
    }
}
