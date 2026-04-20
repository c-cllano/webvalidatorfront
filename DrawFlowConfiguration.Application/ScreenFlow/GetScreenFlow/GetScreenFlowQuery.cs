using MediatR;

namespace DrawFlowConfiguration.Application.ScreenFlow.GetScreenFlow
{

    public class GetScreenFlowQuery : IRequest<object>
    {
        public int ProcesoConvenioGuid { get; set; }
    }

}
