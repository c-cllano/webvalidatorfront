using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableStatusValidationAndAlterColumnsStatusInValidationProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StatusValidation' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[StatusValidation](
	                    [StatusValidationId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [StatusCode] [tinyint] NOT NULL,
	                    [Description] [varchar](50) NOT NULL,
	                    [CreatedDate] [datetime2](7) NOT NULL,
	                    [UpdatedDate] [datetime2](7) NULL,
	                    [Active] [bit] NULL,
	                    [IsDeleted] [bit] NULL,
                     CONSTRAINT [PK_StatusValidation] PRIMARY KEY CLUSTERED 
                    (
	                    [StatusValidationId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY]
                END

                IF NOT EXISTS (SELECT * FROM dbo.StatusValidation WHERE StatusCode=1)
                BEGIN
                    INSERT dbo.[StatusValidation](
                        [StatusCode], 
                        [Description], 
                        [CreatedDate], 
                        [UpdatedDate], 
                        [Active], 
                        [IsDeleted]
                    ) VALUES (
                        1, 
                        N'En proceso', 
                        CAST(N'2025-12-16T00:00:00.0000000' AS DateTime2), 
                        NULL, 
                        1, 
                        NULL
                    )
                END

                IF NOT EXISTS (SELECT * FROM dbo.StatusValidation WHERE StatusCode=2)
                BEGIN
                    INSERT dbo.[StatusValidation](
                        [StatusCode], 
                        [Description], 
                        [CreatedDate], 
                        [UpdatedDate], 
                        [Active], 
                        [IsDeleted]
                    ) VALUES (
                        2, 
                        N'Finalizado', 
                        CAST(N'2025-12-16T00:00:00.0000000' AS DateTime2), 
                        NULL, 
                        1, 
                        NULL
                    )
                END

                IF NOT EXISTS (SELECT * FROM dbo.StatusValidation WHERE StatusCode=3)
                BEGIN
                    INSERT dbo.[StatusValidation](
                        [StatusCode], 
                        [Description], 
                        [CreatedDate], 
                        [UpdatedDate], 
                        [Active], 
                        [IsDeleted]
                    ) VALUES (
                        3, 
                        N'Cancelado', 
                        CAST(N'2025-12-16T00:00:00.0000000' AS DateTime2), 
                        NULL, 
                        1, 
                        NULL
                    )
                END

                IF NOT EXISTS (SELECT * FROM dbo.StatusValidation WHERE StatusCode=4)
                BEGIN
                    INSERT dbo.[StatusValidation](
                        [StatusCode], 
                        [Description], 
                        [CreatedDate], 
                        [UpdatedDate], 
                        [Active], 
                        [IsDeleted]
                    ) VALUES (
                        4, 
                        N'Validacion', 
                        CAST(N'2025-12-16T00:00:00.0000000' AS DateTime2), 
                        NULL, 
                        1, 
                        NULL
                    )
                END

                IF NOT EXISTS (SELECT * FROM dbo.StatusValidation WHERE StatusCode=5)
                BEGIN
                    INSERT dbo.[StatusValidation](
                        [StatusCode], 
                        [Description], 
                        [CreatedDate], 
                        [UpdatedDate], 
                        [Active], 
                        [IsDeleted]
                    ) VALUES (
                        5, 
                        N'Error', 
                        CAST(N'2025-12-16T00:00:00.0000000' AS DateTime2), 
                        NULL, 
                        1, 
                        NULL
                    )
                END

                IF COL_LENGTH('dbo.ValidationProcess', 'StatusValidationId') IS NULL
                BEGIN
                    ALTER TABLE [dbo].[ValidationProcess]
                    ADD StatusValidationId bigint NULL;

                    ALTER TABLE [dbo].[ValidationProcess]  WITH CHECK ADD  CONSTRAINT [FK_ValidationProcess_StatusValidation] FOREIGN KEY([StatusValidationId])
                    REFERENCES [dbo].[StatusValidation] ([StatusValidationId]);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('dbo.ValidationProcess', 'StatusValidationId') IS NOT NULL
                BEGIN
                    ALTER TABLE dbo.ValidationProcess
                    DROP COLUMN StatusValidationId;
                END

                IF OBJECT_ID('dbo.StatusValidation', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.StatusValidation;
                END
            ");
        }
    }
}
