using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_ADD_TABLE_ROLEAGREEMENTBYUSER : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- 1. Verificar si la tabla NO existe para crearla
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleAgreementByUser]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[RoleAgreementByUser](
        [RoleAgreementByUserId] [bigint] IDENTITY(1,1) NOT NULL,
        [UserId] [bigint] NOT NULL,
        [RoleId] [bigint] NOT NULL,
        [AgreementId] [bigint] NOT NULL,
        [CreatorUserId] [bigint] NOT NULL,
        [CreatedDate] [datetime] NOT NULL,
        [UpdatedDate] [datetime] NULL,
        [Active] [bit] NOT NULL,
        [IsDeleted] [bit] NOT NULL,
        CONSTRAINT [PK_RoleAgreementByUser] PRIMARY KEY CLUSTERED ([RoleAgreementByUserId] ASC)
    );
END

-- 2. Validaciones para los Valores por Defecto (Defaults)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'DF_RoleAgreementByUser_CreatedDate' AND type = 'D')
BEGIN
    ALTER TABLE [dbo].[RoleAgreementByUser] 
    ADD CONSTRAINT [DF_RoleAgreementByUser_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'DF_RoleAgreementByUser_Active' AND type = 'D')
BEGIN
    ALTER TABLE [dbo].[RoleAgreementByUser] 
    ADD CONSTRAINT [DF_RoleAgreementByUser_Active] DEFAULT ((1)) FOR [Active];
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'DF_RoleAgreementByUser_IsDeleted' AND type = 'D')
BEGIN
    ALTER TABLE [dbo].[RoleAgreementByUser] 
    ADD CONSTRAINT [DF_RoleAgreementByUser_IsDeleted] DEFAULT ((0)) FOR [IsDeleted];
END

-- 3. Validaciones para las Llaves Foráneas (Foreign Keys)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_RoleAgreementByUser_Agreement')
BEGIN
    ALTER TABLE [dbo].[RoleAgreementByUser] 
    ADD CONSTRAINT [FK_RoleAgreementByUser_Agreement] FOREIGN KEY([AgreementId]) 
    REFERENCES [dbo].[Agreement] ([AgreementId]);
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_RoleAgreementByUser_Role')
BEGIN
    ALTER TABLE [dbo].[RoleAgreementByUser] 
    ADD CONSTRAINT [FK_RoleAgreementByUser_Role] FOREIGN KEY([RoleId]) 
    REFERENCES [dbo].[Role] ([RoleId]);
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_RoleAgreementByUser_User')
BEGIN
    ALTER TABLE [dbo].[RoleAgreementByUser] 
    ADD CONSTRAINT [FK_RoleAgreementByUser_User] FOREIGN KEY([UserId]) 
    REFERENCES [dbo].[User] ([UserId]);
END
");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER TABLE [dbo].[RoleAgreementByUser] 
DROP CONSTRAINT [FK_RoleAgreementByUser_User];

ALTER TABLE [dbo].[RoleAgreementByUser] 
DROP CONSTRAINT [FK_RoleAgreementByUser_Role];

ALTER TABLE [dbo].[RoleAgreementByUser] 
DROP CONSTRAINT [FK_RoleAgreementByUser_Agreement];

ALTER TABLE [dbo].[RoleAgreementByUser] 
DROP CONSTRAINT [DF_RoleAgreementByUser_IsDeleted];

ALTER TABLE [dbo].[RoleAgreementByUser] 
DROP CONSTRAINT [DF_RoleAgreementByUser_Active];

ALTER TABLE [dbo].[RoleAgreementByUser] 
DROP CONSTRAINT [DF_RoleAgreementByUser_CreatedDate];

DROP TABLE [dbo].[RoleAgreementByUser];
");
        }

    }
}
