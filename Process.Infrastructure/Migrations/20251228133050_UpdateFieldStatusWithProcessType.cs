using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldStatusWithProcessType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('dbo.ValidationProcess', 'Status') IS NOT NULL
                BEGIN
                    EXEC sp_rename 
                        'ValidationProcess.Status',
                        'ProcessType',
                        'COLUMN';
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('dbo.ValidationProcess', 'ProcessType') IS NOT NULL
                BEGIN
                    EXEC sp_rename 
                        'ValidationProcess.ProcessType',
                        'Status',
                        'COLUMN';
                END
            ");
        }
    }
}
