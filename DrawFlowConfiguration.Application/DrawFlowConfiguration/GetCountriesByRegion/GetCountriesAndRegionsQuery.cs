using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetCountriesByRegion
{
    public record GetCountriesAndRegionsQuery() : IRequest<IEnumerable<GetCountriesAndRegionsQueryResponse>>;
}
