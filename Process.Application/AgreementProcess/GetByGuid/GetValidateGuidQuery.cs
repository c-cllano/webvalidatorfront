using MediatR;

namespace Process.Application.AgreementProcess.GetByGuid
{
    public class GetValidateGuidQuery : IRequest<bool>
    {
        public Guid ProcessAgreementGuid { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
