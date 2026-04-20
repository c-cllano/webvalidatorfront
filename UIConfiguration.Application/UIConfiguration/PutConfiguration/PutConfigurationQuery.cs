using MediatR;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public class PutConfigurationQuery : IRequest<object>
    {
        public object UIConfigurationJson { get; set; } = default!;
        public Guid ProcesoConvenioGuid { get; set; }
        public int WorkflowID { get; set; }
    }
}
