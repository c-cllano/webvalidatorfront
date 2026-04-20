using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePasswordRecoveryHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PasswordRecoveryHistory' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[PasswordRecoveryHistory](
	                    [Id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [Email] [nvarchar](256) NOT NULL,
	                    [CreateDate] [datetime] NOT NULL,
	                    [Token] [nvarchar](max) NOT NULL,
                    PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.PasswordRecoveryHistory', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.PasswordRecoveryHistory;
                END
            ");
        }
    }
}
