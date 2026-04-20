using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Sso.User
{
    public class GetUsersQuery : IRequest<SsoServiceResult<List<UserSso>>>
    {
        public string? Email { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? SecondName { get; set; } = string.Empty;
        public string? SecondLastName { get; set; } = string.Empty;
        public long ClientId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
