using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CREAR_HIJOS_MENU_CONFIGUI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DECLARE @_IdMenuPadre INT;
        DECLARE @_LinkPadre NVARCHAR(200) = 'configuration';

        SELECT @_IdMenuPadre = MenuId FROM MENU WHERE Link = @_LinkPadre;

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/global-params')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 1, 'Configuration UI global-params', 'Configuration UI gobal-params', '', 'configuration/global-params', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/generate-qr')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 2, 'Configuration UI generate-qr', 'Configuration UI generate-qr', '', 'configuration/generate-qr', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/welcome')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 3, 'Configuration UI welcome', 'Configuration UI welcome', '', 'configuration/welcome', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/permit')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 4, 'Configuration UI permit', 'Configuration UI permit', '', 'configuration/permit', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/permit-denied')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 5, 'Configuration UI permit-denied', 'Configuration UI permit-denied', '', 'configuration/permit-denied', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/document-validation')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 6, 'Configuration UI document-validation', 'Configuration UI document-validation', '', 'configuration/document-validation', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/capture-id')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 7, 'Configuration UI capture-id', 'Configuration UI capture-id', '', 'configuration/capture-id', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/doc-confirmation')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 8, 'Configuration UI doc-confirmation', 'Configuration UI doc-confirmation', '', 'configuration/doc-confirmation', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/retry-capture-doc')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 9, 'Configuration UI retry-capture-doc', 'Configuration UI retry-capture-doc', '', 'configuration/retry-capture-doc', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/upload-document')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 10, 'Configuration UI upload-document', 'Configuration UI upload-document', '', 'configuration/upload-document', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/retry-upload-doc')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 11, 'Configuration UI retry-upload-doc', 'Configuration UI retry-upload-doc', '', 'configuration/retry-upload-doc', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/processing-doc')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 12, 'Configuration UI processing-doc', 'Configuration UI processing-doc', '', 'configuration/processing-doc', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/liveness-guide')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 13, 'Configuration UI liveness-guide', 'Configuration UI liveness-guide', '', 'configuration/liveness-guide', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/liveness')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 14, 'Configuration UI liveness', 'Configuration UI liveness', '', 'configuration/liveness', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/liveness-confirmation')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 15, 'Configuration UI liveness-confirmation', 'Configuration UI liveness-confirmation', '', 'configuration/liveness-confirmation', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/processing-liveness')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 16, 'Configuration UI processing-liveness', 'Configuration UI processing-liveness', '', 'configuration/processing-liveness', 0, GETDATE(), NULL, 1, 0, 0);
        END

        IF @_IdMenuPadre IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM MENU WHERE Link = 'configuration/result')
        BEGIN
            INSERT INTO MENU (ParentId, [Order], Title, [Description], Icon, Link, Selected, CreatedDate, UpdatedDate, Active, IsDeleted, Visible)
            VALUES (@_IdMenuPadre, 17, 'Configuration UI result', 'Configuration UI result', '', 'configuration/result', 0, GETDATE(), NULL, 1, 0, 0);
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM MENU
        WHERE Link IN (
            'configuration/global-params',
            'configuration/generate-qr',
            'configuration/welcome',
            'configuration/permit',
            'configuration/permit-denied',
            'configuration/document-validation',
            'configuration/capture-id',
            'configuration/doc-confirmation',
            'configuration/retry-capture-doc',
            'configuration/upload-document',
            'configuration/retry-upload-doc',
            'configuration/processing-doc',
            'configuration/liveness-guide',
            'configuration/liveness',
            'configuration/liveness-confirmation',
            'configuration/processing-liveness',
            'configuration/result'
        );
    ");
        }

    }
}
