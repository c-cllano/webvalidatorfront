using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentosFromItem;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetUbicacionesFromItem
{

    public class GetUbicacionesFromItemHandler : IRequestHandler<GetUbicacionesFromItemQuery, object>
    {
        private readonly IValidationTransaction _repository;

        public GetUbicacionesFromItemHandler(IValidationTransaction repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(GetUbicacionesFromItemQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetUbicacionesFromItem(requets.WorkFlowID);
            return configuration;
        }

    }
}
