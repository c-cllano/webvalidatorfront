using MediatR;

namespace Process.Application.Sso.User
{
    public class DeleteUserQuery : IRequest<object>
    {
        public int userId { get; set; }
    }
}
