using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Response;
using MediatR;

namespace DrawFlowConfiguration.Application.ScreenFlow.PutScreenFlow
{

    public record UpdateScreenFlowCommand(ScreenFlowResponse Request) : IRequest<bool>;

}
