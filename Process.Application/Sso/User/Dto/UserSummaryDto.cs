using System.ComponentModel.Design;

namespace Process.Application.Sso.User.Dto
{
    /// <summary>
    /// DTO que representa un resumen de usuario con sus roles y agreements asignados
    /// </summary>
    public class UserSummaryDto
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string UserStatus { get; set; } = string.Empty;
        public string? CellPhone { get; set; }
        public List<AssignRoleAgreementDto> AssignRoleAgreement { get; set; } = [];
        public string DocumentType { get; internal set; } = string.Empty;
        public string DocumentNumber { get; internal set; } = string.Empty;
        public string FirstName { get; internal set; } = string.Empty;
        public string SecondName { get; internal set; } = string.Empty;
        public string FirstLastName { get; internal set; } = string.Empty;
        public string SecondLastName { get; internal set; } = string.Empty;
        public bool Active { get; internal set; }
    }

    /// <summary>
    /// DTO que representa la asignación de un rol y agreement a un usuario
    /// </summary>
    public class AssignRoleAgreementDto
    {
        public long RoleAgreementByUserId { get; set; }
        public long RoleByAgreementId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int AgreementId { get; set; }
        public string AgreementName { get; set; } = string.Empty;
    }
}
