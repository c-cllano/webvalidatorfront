using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentosFromItem;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetNationalitiesFromItem
{



    public class GetNationalitiesFromItemHandler : IRequestHandler<GetNationalitiesFromItemQuery, object>
    {
        private readonly IValidationTransaction _repository;

        public GetNationalitiesFromItemHandler(IValidationTransaction repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(GetNationalitiesFromItemQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetNationalitiesFromItem(requets.WorkFlowID);
            return configuration;
        }

    }
}
