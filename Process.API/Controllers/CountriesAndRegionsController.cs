using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.Agreements.GetByClient;
using Process.Application.CountriesAndRegions.GetCountriesAndRegions;
using Process.Application.DocumentTypeByCountry.GetDocumentTypeByCountry;
using System.Net;

namespace Process.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CountriesAndRegionsController(IMediator mediator) : ControllerBase
    {
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetCountriesAndRegionsQueryResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetCountriesByRegion")]
        public async Task<IActionResult> GetCountriesByRegion()
        {
            var result = await mediator.Send(new GetCountriesAndRegionsQuery());
            var response = ApiResponse<IEnumerable<GetCountriesAndRegionsQueryResponse>>.Success(result);

            return Ok(response);
        }


        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetDocumentTypeByCountryQueryResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetDocumentTypeByCountry")]
        public async Task<IActionResult> GetDocumentTypeByCountry()
        {
            var result = await mediator.Send(new GetDocumentTypeByCountryQuery());
            var response = ApiResponse<IEnumerable<GetDocumentTypeByCountryQueryResponse>>.Success(result);

            return Ok(response);
        }

    }



}
