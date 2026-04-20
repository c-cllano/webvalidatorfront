using MediatR;

namespace Process.Application.AgreementByUser.Create
{
    public class CreateAgreementByUserCommand : IRequest<long>
    {
        public int UserId { get; set; }
        public int AgreementId { get; set; }
        public long? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Active { get; set; }
    }
}