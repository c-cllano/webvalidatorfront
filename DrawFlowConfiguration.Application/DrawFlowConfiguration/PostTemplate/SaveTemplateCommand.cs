using MediatR;
using DrawFlowConfiguration.Domain.Parameters.Transaction;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow;


namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PostTemplate
{
    public record SaveTemplateCommand(TemplateEntry request) : IRequest<bool>;
 
}



