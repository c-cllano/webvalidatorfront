using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_TokenCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TokenCache' AND xtype='U')
                                    BEGIN
                                         CREATE TABLE [dbo].[TokenCache](
                                        	[Id] [nvarchar](449) NOT NULL,
                                        	[Value] [varbinary](max) NOT NULL,
                                        	[ExpiresAtTime] [datetimeoffset](7) NOT NULL,
                                        	[SlidingExpirationInSeconds] [bigint] NULL,
                                        	[AbsoluteExpiration] [datetimeoffset](7) NULL,
                                         CONSTRAINT [PK_TokenCache] PRIMARY KEY CLUSTERED 
                                        (
                                        	[Id] ASC
                                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                              IF OBJECT_ID('dbo.TokenCache', 'U') IS NOT NULL
                              BEGIN
                                DROP TABLE dbo.DocumentTypeCapture;
                              END");
        }
    }
}
