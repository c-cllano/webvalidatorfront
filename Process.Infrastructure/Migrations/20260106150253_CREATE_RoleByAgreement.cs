using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_RoleByAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                   IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='RoleByAgreement' AND xtype='U')
                                     BEGIN
                                    CREATE TABLE [dbo].[RoleByAgreement](
                                    	[RoleByAgreementId] [bigint] IDENTITY(1,1) NOT NULL,
                                    	[RoleId] [bigint] NOT NULL,
                                    	[AgreementId] [bigint] NOT NULL,
                                    	[CreatorUserId] [bigint] NOT NULL,
                                    	[CreatedDate] [datetime] NOT NULL,
                                    	[UpdatedDate] [datetime] NULL,
                                    	[Active] [bit] NOT NULL,
                                    	[IsDeleted] [bit] NOT NULL,
                                     CONSTRAINT [PK_RoleByAgreement] PRIMARY KEY CLUSTERED 
                                    (
                                    	[RoleByAgreementId] ASC
                                    )WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                                    ) ON [PRIMARY]
                                    
                                    ALTER TABLE [dbo].[RoleByAgreement] ADD  CONSTRAINT [DF_RoleByAgreement_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
                                    
                                    ALTER TABLE [dbo].[RoleByAgreement] ADD  CONSTRAINT [DF_RoleByAgreement_Active]  DEFAULT ((1)) FOR [Active]
                                    
                                    ALTER TABLE [dbo].[RoleByAgreement] ADD  CONSTRAINT [DF_RoleByAgreement_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
                                    
                                    ALTER TABLE [dbo].[RoleByAgreement]  WITH CHECK ADD  CONSTRAINT [FK_RoleByAgreement_Agreement] FOREIGN KEY([AgreementId])
                                    REFERENCES [dbo].[Agreement] ([AgreementId])
                                    
                                    ALTER TABLE [dbo].[RoleByAgreement] CHECK CONSTRAINT [FK_RoleByAgreement_Agreement]
                                    
                                    ALTER TABLE [dbo].[RoleByAgreement]  WITH CHECK ADD  CONSTRAINT [FK_RoleByAgreement_Role] FOREIGN KEY([RoleId])
                                    REFERENCES [dbo].[Role] ([RoleId])
                                    
                                    ALTER TABLE [dbo].[RoleByAgreement] CHECK CONSTRAINT [FK_RoleByAgreement_Role]
                                    
                                    END  
                                  ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                   IF OBJECT_ID('dbo.RoleByAgreement', 'U') IS NOT NULL
                                    BEGIN
                                      DROP TABLE dbo.RoleByAgreement;
                                    END
                                 ");
        }
    }
}
