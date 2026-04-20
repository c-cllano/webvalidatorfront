using MediatR;

namespace UIConfiguration.Application.UIConfiguration.GetConfiguration
{
    public class GetPagesWithDisplayTrueQuery : IRequest<object>
    {
        public Guid ProcesoConvenioGuid { get; set; }
        public int WorkflowID { get; set; }
    }
}
