using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableValidationProcessDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ValidationProcessDocuments' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[ValidationProcessDocuments](
	                    [ValidationProcessDocumentsId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [ValidationProcessId] [bigint] NOT NULL,
	                    [UrlFile] [varchar](max) NOT NULL,
                        [ProcessName] [varchar](255) NULL,
	                    [ServiceType] [int] NOT NULL,
	                    [ServiceSubType] [int] NULL,
	                    [CreatedDate] [datetime] NOT NULL,
	                    [UpdatedDate] [datetime] NULL,
	                    [Active] [bit] NOT NULL,
	                    [IsDeleted] [bit] NOT NULL,
                     CONSTRAINT [PK_ValidationProcessDocuments] PRIMARY KEY CLUSTERED 
                    (
	                    [ValidationProcessDocumentsId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                    ALTER TABLE [dbo].[ValidationProcessDocuments]  WITH CHECK ADD  CONSTRAINT [FK_ValidationProcessDocuments_ValidationProcess] FOREIGN KEY([ValidationProcessId])
                    REFERENCES [dbo].[ValidationProcess] ([ValidationProcessId])

                    ALTER TABLE [dbo].[ValidationProcessDocuments] CHECK CONSTRAINT [FK_ValidationProcessDocuments_ValidationProcess]

                    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla en donde se almacenan las imagenes de facial y de documentos de todo el flujo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ValidationProcessDocuments'
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.ValidationProcessDocuments', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.ValidationProcessDocuments;
                END
            ");
        }
    }
}
