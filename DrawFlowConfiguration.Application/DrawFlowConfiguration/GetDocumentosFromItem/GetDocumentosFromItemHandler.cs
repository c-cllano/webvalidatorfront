using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetExistByNameAndGUID;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentosFromItem
{

    public class GetDocumentosFromItemHandler : IRequestHandler<GetDocumentosFromItemQuery, object>
    {
        private readonly IValidationTransaction _repository;

        public GetDocumentosFromItemHandler(IValidationTransaction repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(GetDocumentosFromItemQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetDocumentosFromItem(requets.WorkFlowID);
            return configuration;
        }

    }
}
