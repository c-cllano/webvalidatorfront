using MediatR;

namespace Process.Application.AgreementProcess.GetProcess
{
    public class GetProcessQuery : IRequest<GetProcessResponse>
    {

        public Guid ProcesoConvenioGuid { get; set; }

        public Guid? AppFrontGuid { get; set; }

    }
}
