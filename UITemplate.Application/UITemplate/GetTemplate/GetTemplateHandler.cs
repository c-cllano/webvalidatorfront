using MediatR;
using UITemplate.Domain.Repositories;

namespace UITemplate.Application.UITemplate.GetTemplate
{
    public class GetTemplateHandler : IRequestHandler<GetTemplateQuery, object>
    {
        private readonly IUITemplateRepository _repository;

        public GetTemplateHandler(IUITemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(GetTemplateQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetUItemplate(requets.AgreementGuid.ToString("D").ToLower());
            return configuration;
        }
    }
}
