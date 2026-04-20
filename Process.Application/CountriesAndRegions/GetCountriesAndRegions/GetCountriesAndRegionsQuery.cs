using MediatR;
using Process.Application.Agreements.GetByClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Application.CountriesAndRegions.GetCountriesAndRegions
{
    public record GetCountriesAndRegionsQuery() : IRequest<IEnumerable<GetCountriesAndRegionsQueryResponse>>;
}
