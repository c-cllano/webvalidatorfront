using MediatR;
using Process.Domain.Parameters.Sso.User;

namespace Process.Application.Sso.User
{
    public class UpdateUserAssignmentsQuery : IRequest<object>
    {
        public int UserId { get; set; }
        public List<AssignmentRoleAgreements> AssignmentRoleAgreements { get; set; } = new List<AssignmentRoleAgreements>();
        public long UpdaterUserId { get; set; }
    }
}
