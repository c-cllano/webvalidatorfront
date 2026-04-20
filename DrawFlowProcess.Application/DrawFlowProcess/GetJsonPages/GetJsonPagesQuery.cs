using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetJsonPages
{
    public class GetJsonPagesQuery : IRequest<GetJsonPagesResponse>
    {
        public Guid AgreementId { get; set; }
        public int WorkFlowId { get; set; }
    }
}
