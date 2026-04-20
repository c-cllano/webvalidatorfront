using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ValidationProcessAuditLogs' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[ValidationProcessAuditLogs](
	                    [ValidationProcessAuditLogsId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [ValidationProcessId] [bigint] NULL,
	                    [Url] [varchar](500) NOT NULL,
	                    [Method] [varchar](20) NOT NULL,
	                    [RequestBody] [varchar](max) NULL,
	                    [ResponseBody] [varchar](max) NULL,
	                    [StatusCode] [int] NOT NULL,
	                    [DurationMs] [decimal](18, 2) NOT NULL,
	                    [CreatedAt] [datetime2](7) NOT NULL,
                     CONSTRAINT [PK_ValidationProcessAuditLogs] PRIMARY KEY CLUSTERED 
                    (
	                    [ValidationProcessAuditLogsId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                    ALTER TABLE [dbo].[ValidationProcessAuditLogs] ADD  CONSTRAINT [DF_ValidationProcessAuditLogs_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]

                    ALTER TABLE [dbo].[ValidationProcessAuditLogs]  WITH CHECK ADD  CONSTRAINT [FK_ValidationProcessAuditLogs_ValidationProcess] FOREIGN KEY([ValidationProcessId])
                    REFERENCES [dbo].[ValidationProcess] ([ValidationProcessId])

                    ALTER TABLE [dbo].[ValidationProcessAuditLogs] CHECK CONSTRAINT [FK_ValidationProcessAuditLogs_ValidationProcess]
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.ValidationProcessAuditLogs', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.ValidationProcessAuditLogs;
                END
            ");
        }
    }
}
