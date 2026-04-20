using MediatR;
using Process.Application.CountriesAndRegions.GetCountriesAndRegions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Application.DocumentTypeByCountry.GetDocumentTypeByCountry
{

    public record GetDocumentTypeByCountryQuery() : IRequest<IEnumerable<GetDocumentTypeByCountryQueryResponse>>;
}
