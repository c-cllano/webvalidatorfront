using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableParameterAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sysobjects WHERE name='ParametersAgreement' AND xtype='U')
                BEGIN
                    IF COL_LENGTH('dbo.ParametersAgreement', 'ParameterCode') IS NULL
                    BEGIN
                        ALTER TABLE dbo.ParametersAgreement
                        ADD ParameterCode VARCHAR(20) NOT NULL
                        CONSTRAINT DF_ParametersAgreement_ParameterCode DEFAULT(''); 
                    END

                    IF COL_LENGTH('dbo.ParametersAgreement', 'ParameterDescription') IS NULL
                    BEGIN
                        ALTER TABLE dbo.ParametersAgreement
                        ADD ParameterDescription VARCHAR(255) NULL;
                    END
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sysobjects WHERE name='ParametersAgreement' AND xtype='U')
                BEGIN
                    IF COL_LENGTH('dbo.ParametersAgreement', 'ParameterCode') IS NOT NULL
                    BEGIN
                        ALTER TABLE dbo.ParametersAgreement
                        DROP COLUMN ParameterCode;
                    END

                    IF COL_LENGTH('dbo.ParametersAgreement', 'ParameterDescription') IS NOT NULL
                    BEGIN
                        ALTER TABLE dbo.ParametersAgreement
                        DROP COLUMN ParameterDescription;
                    END
                END
            ");
        }
    }
}
