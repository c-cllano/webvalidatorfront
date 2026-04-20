using MediatR;
using Process.Domain.Repositories;

namespace Process.Application.DocumentTypeByCountry.GetDocumentTypeByCountry
{

    public class GetDocumentTypeByCountryQueryHandler(IDocumentTypeByCountryRepository repository) :
    IRequestHandler<GetDocumentTypeByCountryQuery,
        IEnumerable<GetDocumentTypeByCountryQueryResponse>>
    {

        public async Task<IEnumerable<GetDocumentTypeByCountryQueryResponse>> Handle(
         GetDocumentTypeByCountryQuery request, CancellationToken cancellationToken)
        {

            var countriesRegions = await repository.GetDocumentTypeByCountry();

            var response = countriesRegions.Select(x => new GetDocumentTypeByCountryQueryResponse(
                x.DocumentTypeByCountryId,
                x.CountryId,
                x.Flag,
                x.NameCountry,
                x.NameESP,
                x.Indicative,
                x.FrecuentCountry,
                x.DocumentTypeId,
                x.CodeDocumentType,
                x.NameDocumentType,
                x.RegionId,
                x.NameRegion,
                x.Length,
                x.RegularExpression
            ));

            return response;

        }
    }
}
