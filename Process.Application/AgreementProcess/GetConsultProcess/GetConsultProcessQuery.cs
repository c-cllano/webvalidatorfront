using MediatR;

namespace Process.Application.AgreementProcess.GetConsultProcess
{
    public class GetConsultProcessQuery : IRequest<GetConsultProcessResponse>
    {
        public Guid ProcessAgreement { get; set; }
    }
}
