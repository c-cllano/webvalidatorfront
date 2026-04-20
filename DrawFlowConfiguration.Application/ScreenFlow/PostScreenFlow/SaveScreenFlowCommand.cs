using DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Request;
using MediatR;

namespace DrawFlowConfiguration.Application.ScreenFlow.PostScreenFlow
{
 
    public record SaveScreenFlowCommand(ScreenFlowRequest request) : IRequest<bool>;
}
