using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableMobileDeviceInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MobileDeviceInfo' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[MobileDeviceInfo](
	                    [MobileDeviceInfoId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [Brand] [varchar](50) NOT NULL,
	                    [Model] [varchar](50) NOT NULL,
	                    [MediaDeviceInfo] [varchar](200) NOT NULL,
	                    [BestCameraAutomatic] [varchar](50) NULL,
	                    [BestCameraManual] [varchar](50) NULL,
	                    [CreatedDate] [datetime] NOT NULL,
	                    [UpdatedDate] [datetime] NULL,
	                    [Active] [bit] NOT NULL,
	                    [IsDeleted] [bit] NOT NULL,
                     CONSTRAINT [PK_MobileDeviceInfo] PRIMARY KEY CLUSTERED 
                    (
	                    [MobileDeviceInfoId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY]
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.MobileDeviceInfo', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.MobileDeviceInfo;
                END
            ");
        }
    }
}
