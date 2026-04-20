using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableValidationProcessDeviceInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ValidationProcessDeviceInfo' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[ValidationProcessDeviceInfo](
	                    [ValidationProcessDeviceInfoId] [bigint] IDENTITY(1,1) NOT NULL,
	                    [ValidationProcessId] [bigint] NOT NULL,
	                    [MobileDeviceInfoId] [bigint] NOT NULL,
	                    [BestCameraAutomatic] [varchar](50) NULL,
	                    [CreatedDate] [datetime] NOT NULL,
	                    [UpdatedDate] [datetime] NULL,
	                    [Active] [bit] NOT NULL,
	                    [IsDeleted] [bit] NOT NULL,
                        CONSTRAINT [PK_ValidationProcessDeviceInfo] PRIMARY KEY CLUSTERED 
                    (
	                    [ValidationProcessDeviceInfoId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY]

                    ALTER TABLE [dbo].[ValidationProcessDeviceInfo]  WITH CHECK ADD  CONSTRAINT [FK_ValidationProcessDeviceInfo_MobileDeviceInfo] FOREIGN KEY([MobileDeviceInfoId])
                    REFERENCES [dbo].[MobileDeviceInfo] ([MobileDeviceInfoId])

                    ALTER TABLE [dbo].[ValidationProcessDeviceInfo] CHECK CONSTRAINT [FK_ValidationProcessDeviceInfo_MobileDeviceInfo]

                    ALTER TABLE [dbo].[ValidationProcessDeviceInfo]  WITH CHECK ADD  CONSTRAINT [FK_ValidationProcessDeviceInfo_ValidationProcess] FOREIGN KEY([ValidationProcessId])
                    REFERENCES [dbo].[ValidationProcess] ([ValidationProcessId])

                    ALTER TABLE [dbo].[ValidationProcessDeviceInfo] CHECK CONSTRAINT [FK_ValidationProcessDeviceInfo_ValidationProcess]
                END

                IF EXISTS (SELECT * FROM sysobjects WHERE name='MobileDeviceInfo' AND xtype='U')
                BEGIN
                    IF COL_LENGTH('dbo.MobileDeviceInfo', 'BestCameraAutomatic') IS NOT NULL
                    BEGIN
                        ALTER TABLE dbo.MobileDeviceInfo
                        DROP COLUMN BestCameraAutomatic;
                    END

                    IF COL_LENGTH('dbo.MobileDeviceInfo', 'OS') IS NULL
                    BEGIN
                        ALTER TABLE dbo.MobileDeviceInfo
                        ADD OS VARCHAR(50) NOT NULL
                        CONSTRAINT DF_MobileDeviceInfo_OS DEFAULT('');
                    END

                    IF COL_LENGTH('dbo.MobileDeviceInfo', 'OSVersion') IS NULL
                    BEGIN
                        ALTER TABLE dbo.MobileDeviceInfo
                        ADD OSVersion VARCHAR(50) NOT NULL
                        CONSTRAINT DF_MobileDeviceInfo_OSVersion DEFAULT('');
                    END

                    IF COL_LENGTH('dbo.MobileDeviceInfo', 'Browser') IS NULL
                    BEGIN
                        ALTER TABLE dbo.MobileDeviceInfo
                        ADD Browser VARCHAR(50) NOT NULL
                        CONSTRAINT DF_MobileDeviceInfo_Browser DEFAULT('');
                    END
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.ValidationProcessDeviceInfo', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.ValidationProcessDeviceInfo;
                END

                IF COL_LENGTH('dbo.MobileDeviceInfo', 'BestCameraAutomatic') IS NOT NULL
                BEGIN
                    ALTER TABLE dbo.MobileDeviceInfo
                    ADD BestCameraAutomatic VARCHAR(50) NULL
                END

                IF COL_LENGTH('dbo.MobileDeviceInfo', 'OS') IS NOT NULL
                BEGIN
                    ALTER TABLE dbo.MobileDeviceInfo
                    DROP COLUMN OS;
                END

                IF COL_LENGTH('dbo.MobileDeviceInfo', 'OSVersion') IS NOT NULL
                BEGIN
                    ALTER TABLE dbo.MobileDeviceInfo
                    DROP COLUMN OSVersion;
                END

                IF COL_LENGTH('dbo.MobileDeviceInfo', 'Browser') IS NOT NULL
                BEGIN
                    ALTER TABLE dbo.MobileDeviceInfo
                    DROP COLUMN Browser;
                END
            ");
        }
    }
}
