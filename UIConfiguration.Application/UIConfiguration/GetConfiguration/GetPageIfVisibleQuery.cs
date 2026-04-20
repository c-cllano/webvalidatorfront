using MediatR;

namespace UIConfiguration.Application.UIConfiguration.GetConfiguration
{
    public class GetPageIfVisibleQuery : IRequest<object>
    {
        public Guid ProcesoConvenioGuid { get; set; }
        public string PageName { get; set; } = string.Empty;
        public int WorkflowID { get; set; }
    }
}
