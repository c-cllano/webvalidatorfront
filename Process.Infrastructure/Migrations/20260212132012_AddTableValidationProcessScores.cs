using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableValidationProcessScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ValidationProcessScores' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[ValidationProcessScores](
	                    [ValidationProcessScoresId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [ValidationProcessId] [bigint] NOT NULL,
	                    [Scores] [varchar](max) NOT NULL,
	                    [CreatedDate] [datetime] NOT NULL,
	                    [UpdatedDate] [datetime] NULL,
	                    [Active] [bit] NOT NULL,
	                    [IsDeleted] [bit] NOT NULL,
                     CONSTRAINT [PK_ValidationProcessScores] PRIMARY KEY CLUSTERED 
                    (
	                    [ValidationProcessScoresId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                    ALTER TABLE [dbo].[ValidationProcessScores]  WITH CHECK ADD  CONSTRAINT [FK_ValidationProcessScores_ValidationProcess] FOREIGN KEY([ValidationProcessId])
                    REFERENCES [dbo].[ValidationProcess] ([ValidationProcessId])

                    ALTER TABLE [dbo].[ValidationProcessScores] CHECK CONSTRAINT [FK_ValidationProcessScores_ValidationProcess]
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.ValidationProcessScores', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.ValidationProcessScores;
                END
            ");
        }
    }
}
