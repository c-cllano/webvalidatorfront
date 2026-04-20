using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Application.DocumentTypeByCountry.GetDocumentTypeByCountry
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
        int Length,
        string RegularExpression
    );
}
