using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_DocumentTypeCapture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
                             IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DocumentTypeCapture' AND xtype='U')
                                  BEGIN
                                 CREATE TABLE DocumentTypeCapture (
                                 DocumentTypeCaptureId INT IDENTITY(1,1) PRIMARY KEY,
                                 DocumentTypeId INT NULL,
                                 Sides INT NULL,
                                 InstantFeedback BIT NOT NULL DEFAULT 0,
                                 CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                                 UpdatedDate DATETIME NULL,
                                 Active BIT NOT NULL DEFAULT 1,
                                 IsDeleted BIT NOT NULL DEFAULT 0,
                                 CONSTRAINT FK_DocumentTypeCapture_DocumentType
                                  FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(DocumentTypeId)
                             
                             );
                             END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF OBJECT_ID('dbo.DocumentTypeCapture', 'U') IS NOT NULL
                                 BEGIN
                                   DROP TABLE dbo.DocumentTypeCapture;
                                 END");
        }
    }
}
