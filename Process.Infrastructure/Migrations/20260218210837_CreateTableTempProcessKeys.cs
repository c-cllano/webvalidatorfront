using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableTempProcessKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TempProcessKeys' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[TempProcessKeys](
	                    [TempProcessKeysId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [ValidationProcessGuid] [uniqueidentifier] NOT NULL,
	                    [PublicKey] [varchar](max) NOT NULL,
	                    [PrivateKey] [varchar](max) NOT NULL,
	                    [AlgorithmPublicKey] [varchar](max) NOT NULL,
	                    [AlgorithmPrivateKey] [varchar](max) NOT NULL,
	                    [CreatedAt] [datetime2](7) NOT NULL,
	                    [UpdatedAt] [datetime2](7) NULL,
                     CONSTRAINT [PK_TempProcessKeys] PRIMARY KEY CLUSTERED 
                    (
	                    [TempProcessKeysId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                    ALTER TABLE [dbo].[TempProcessKeys] ADD  CONSTRAINT [DF_TempProcessKeys_AlgorithmPublicKey]  DEFAULT (N'') FOR [AlgorithmPublicKey]

                    ALTER TABLE [dbo].[TempProcessKeys] ADD  CONSTRAINT [DF_TempProcessKeys_AlgorithmPrivateKey]  DEFAULT (N'') FOR [AlgorithmPrivateKey]
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.TempProcessKeys', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.TempProcessKeys;
                END
            ");
        }
    }
}
