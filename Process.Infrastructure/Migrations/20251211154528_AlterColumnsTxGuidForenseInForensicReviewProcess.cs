using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnsTxGuidForenseInForensicReviewProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StatusForensic' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[StatusForensic](
	                    [StatusForensicId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [Description] [varchar](50) NOT NULL,
	                    [CreatedDate] [datetime2](7) NULL,
	                    [UpdatedDate] [datetime2](7) NULL,
	                    [Active] [bit] NULL,
	                    [IsDeleted] [bit] NULL,
                     CONSTRAINT [PK_StatusForensic] PRIMARY KEY CLUSTERED 
                    (
	                    [StatusForensicId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY]
                END

                IF NOT EXISTS (SELECT * FROM dbo.StatusForensic WHERE Description='En revisión')
                BEGIN
                    INSERT INTO [dbo].[StatusForensic](
                        [Description]
                        ,[CreatedDate]
                        ,[Active]
                        ,[IsDeleted]
                    )
                    VALUES(
                        'En revisión'
                        ,'2025-02-12 00:00:00.0000000'
                        ,1
                        ,0
                    )
                END

                IF NOT EXISTS (SELECT * FROM dbo.StatusForensic WHERE Description='Revisado')
                BEGIN
                    INSERT INTO [dbo].[StatusForensic](
                        [Description]
                        ,[CreatedDate]
                        ,[Active]
                        ,[IsDeleted]
                    )
                    VALUES(
                        'Revisado'
                        ,'2025-02-12 00:00:00.0000000'
                        ,1
                        ,0
                    )
                END

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ForensicReviewProcess' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[ForensicReviewProcess](
	                    [ForensicReviewProcessId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [ValidationProcessId] [bigint] NOT NULL,
	                    [TxGuidForense] [uniqueidentifier] NULL,
	                    [StatusForensicId] [bigint] NOT NULL,
	                    [VerificationDate] [datetime2](7) NULL,
	                    [Approved] [bit] NULL,
	                    [Score] [decimal](18, 2) NULL,
	                    [MainReason] [varchar](100) NULL,
	                    [OptionalReason] [varchar](100) NULL,
	                    [Description] [varchar](100) NULL,
	                    [Observation] [varchar](100) NULL,
	                    [CreatedDate] [datetime2](7) NULL,
	                    [UpdatedDate] [datetime2](7) NULL,
	                    [Active] [bit] NULL,
	                    [IsDeleted] [bit] NULL,
                     CONSTRAINT [PK_ForensicReviewProcess] PRIMARY KEY CLUSTERED 
                    (
	                    [ForensicReviewProcessId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY]

                    ALTER TABLE [dbo].[ForensicReviewProcess]  WITH CHECK ADD  CONSTRAINT [FK_ForensicReviewProcess_StatusForensic] FOREIGN KEY([StatusForensicId])
                    REFERENCES [dbo].[StatusForensic] ([StatusForensicId])

                    ALTER TABLE [dbo].[ForensicReviewProcess] CHECK CONSTRAINT [FK_ForensicReviewProcess_StatusForensic]

                    ALTER TABLE [dbo].[ForensicReviewProcess]  WITH CHECK ADD  CONSTRAINT [FK_ForensicReviewProcess_ValidationProcess] FOREIGN KEY([ValidationProcessId])
                    REFERENCES [dbo].[ValidationProcess] ([ValidationProcessId])

                    ALTER TABLE [dbo].[ForensicReviewProcess] CHECK CONSTRAINT [FK_ForensicReviewProcess_ValidationProcess]
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.ForensicReviewProcess', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.ForensicReviewProcess;
                END

                IF OBJECT_ID('dbo.StatusForensic', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.StatusForensic;
                END
            ");
        }
    }
}
