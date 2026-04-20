using MediatR;
using Process.Domain.Repositories;

namespace Process.Application.CountriesAndRegions.GetCountriesAndRegions
{

    public class GetCountriesAndRegionsQueryHandler(ICountriesAndRegionsRepository repository) :
        IRequestHandler<GetCountriesAndRegionsQuery,
            IEnumerable<GetCountriesAndRegionsQueryResponse>>
    {

        public async Task<IEnumerable<GetCountriesAndRegionsQueryResponse>> Handle(
         GetCountriesAndRegionsQuery request, CancellationToken cancellationToken)
        {

            var countriesRegions = await repository.GetCountriesAndRegions();

            var response = countriesRegions.Select(x => new GetCountriesAndRegionsQueryResponse(
                x!.CountryId,
                x.Flag,
                x.NameESP,
                x.Indicative,
                x.RegionName,
                x.FrecuentCountry
            ));

            return response;

        }
    }
}
