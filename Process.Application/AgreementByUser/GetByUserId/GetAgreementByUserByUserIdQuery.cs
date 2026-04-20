using MediatR;

namespace Process.Application.AgreementByUser.GetByUserId
{
    public class GetAgreementByUserByUserIdQuery : IRequest<IEnumerable<GetByUserIdQueryResponse>>
    {
        public int UserId { get; set; }
    }
}