using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class INSERT_DocumentTypeCapture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[DocumentTypeCapture]
           ([DocumentTypeId]
           ,[Sides]
           ,[InstantFeedback]
           ,[CreatedDate]
           ,[UpdatedDate]
           ,[Active]
           ,[IsDeleted])
       VALUES
    (NULL,2, 0, GETDATE(), NULL, 1, 0),
    (1,2,1, GETDATE(), NULL, 1, 0),
    (2,2,0, GETDATE(), NULL, 1, 0),
    (5,1,0, GETDATE(), NULL, 1, 0);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM [DocumentTypeCapture]");
        }
    }
}
