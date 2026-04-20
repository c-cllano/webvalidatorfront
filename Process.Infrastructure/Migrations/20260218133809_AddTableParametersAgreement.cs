using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableParametersAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ParametersAgreement' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[ParametersAgreement](
	                    [ParameterAgreementId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [AgreementId] [bigint] NOT NULL,
	                    [ParameterAgreementGuid] [uniqueidentifier] NOT NULL,
	                    [ParameterName] [varchar](100) NOT NULL,
	                    [ParameterValue] [varchar](max) NOT NULL,
	                    [CreatedDate] [datetime] NOT NULL,
	                    [UpdatedDate] [datetime] NULL,
	                    [Active] [bit] NULL,
	                    [IsDeleted] [bit] NULL
                     CONSTRAINT [PK_ParameterAgreement] PRIMARY KEY CLUSTERED 
                    (
	                    [ParameterAgreementId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                    ALTER TABLE [dbo].[ParametersAgreement]  WITH CHECK ADD  CONSTRAINT [FK_ParameterAgreement_Agreement] FOREIGN KEY([AgreementId])
                    REFERENCES [dbo].[Agreement] ([AgreementId])

                    ALTER TABLE [dbo].[ParametersAgreement] CHECK CONSTRAINT [FK_ParameterAgreement_Agreement]
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.ParametersAgreement', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.ParametersAgreement;
                END
            ");
        }
    }
}
