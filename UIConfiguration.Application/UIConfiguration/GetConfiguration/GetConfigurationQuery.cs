using MediatR;
namespace UIConfiguration.Application.UIConfiguration.GetConfiguration
{
    public class GetConfigurationQuery : IRequest<object> 
    {
        public Guid ProcesoConvenioGuid { get; set; }
        public int WorkflowID { get; set; }
    }
}
