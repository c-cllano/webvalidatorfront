using MediatR;
using Process.Application.Sso.User.Dto;

namespace Process.Application.Sso.User
{
    public class GetUserQuery : IRequest<UserSummaryDto>
    {
        public int UserId { get; set; }
    }
}
