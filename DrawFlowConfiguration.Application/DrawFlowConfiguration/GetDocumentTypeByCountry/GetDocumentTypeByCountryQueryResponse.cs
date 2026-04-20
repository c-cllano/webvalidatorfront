using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentTypeByCountry
{
    public record GetDocumentTypeByCountryQueryResponse(
            int DocumentTypeByCountryId,
            int CountryId,
            string Flag,
            string NameCountry,
            string NameESP,
            string Indicative,
            bool FrecuentCountry,
            int DocumentTypeId,
            string CodeDocumentType,
            string NameDocumentType,
            int RegionId,
            string NameRegion,
            bool Active,
            int Length,
            string RegularExpression,
            int MinLength,
            int MaxLength
        );
}
