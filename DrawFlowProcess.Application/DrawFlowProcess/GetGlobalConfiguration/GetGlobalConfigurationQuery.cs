using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetGlobalConfiguration
{
    public class GetGlobalConfigurationQuery : IRequest<List<GetGlobalConfigurationResponse>>
    {
        public Guid AgreementId { get; set; }
        public int WorkFlowId { get; set; }
        public DateTime CreateDateTask { get; set; }
        public string Section { get; set; } = string.Empty;

    }
}
