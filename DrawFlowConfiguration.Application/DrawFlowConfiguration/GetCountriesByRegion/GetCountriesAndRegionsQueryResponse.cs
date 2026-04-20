using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetCountriesByRegion
{
    public record GetCountriesAndRegionsQueryResponse(
        int CountryId,
        string Flag,
        string NameESP,
        string Indicative,
        string RegionName,
        bool FrecuentCountry
    );
}
