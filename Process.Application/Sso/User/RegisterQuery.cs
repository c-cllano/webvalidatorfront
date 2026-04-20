using MediatR;
using Process.Domain.Parameters.Sso.User;

namespace Process.Application.Sso.User
{
    public class RegisterQuery : IRequest<object>
    {
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public int CreatedUserId { get; set; }
        public int ClientId { get; set; }
        public int RoleId { get; set; }
        public string? CellPhone { get; set; }
        public List<AssignmentRoleAgreements> AssignmentRoleAgreements { get; set; } = [];
    }
}
