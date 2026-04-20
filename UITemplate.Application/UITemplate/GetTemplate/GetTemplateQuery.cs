using MediatR;

namespace UITemplate.Application.UITemplate.GetTemplate
{
    public class GetTemplateQuery : IRequest<object>
    {
        public Guid AgreementGuid { get; set; }
    }
}
