using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePasswordRecoveryNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PasswordRecoveryNotifications' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[PasswordRecoveryNotifications](
	                    [Id] [int] IDENTITY(1,1) NOT NULL,
	                    [UserId] [int] NOT NULL,
	                    [Email] [nvarchar](256) NOT NULL,
	                    [NotificationCount] [int] NOT NULL,
	                    [LastSentAt] [datetime] NOT NULL,
	                    [CreatedAt] [datetime] NOT NULL,
	                    [UpdatedAt] [datetime] NOT NULL,
	                    [Token] [nvarchar](max) NULL,
                    PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                    ALTER TABLE [dbo].[PasswordRecoveryNotifications] ADD  DEFAULT ((1)) FOR [NotificationCount]

                    ALTER TABLE [dbo].[PasswordRecoveryNotifications] ADD  DEFAULT (getdate()) FOR [LastSentAt]

                    ALTER TABLE [dbo].[PasswordRecoveryNotifications] ADD  DEFAULT (getdate()) FOR [CreatedAt]

                    ALTER TABLE [dbo].[PasswordRecoveryNotifications] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.PasswordRecoveryNotifications', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.PasswordRecoveryNotifications;
                END
            ");
        }
    }
}
