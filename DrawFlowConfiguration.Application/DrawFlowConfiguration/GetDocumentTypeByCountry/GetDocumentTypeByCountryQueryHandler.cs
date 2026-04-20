using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentTypeByCountry
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
                x.Active,
                x.Length,
                x.RegularExpression,
                x.MinLength,
                x.MaxLength
            ));

            return response;
        }
    }
}
