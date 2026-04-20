using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.Agreements.GetByClient;
using System.Net;

namespace Process.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AgreementController(IMediator mediator) : ControllerBase
    {
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetAgreementsByClientGuidQueryResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetByClientGuid/{guid}")]
        public async Task<IActionResult> GetByClientId(Guid guid)
        {
            var result = await mediator.Send(new GetAgreementsByClientGuidQuery(guid));
            var response = ApiResponse<IReadOnlyList<GetAgreementsByClientGuidQueryResponse>>.Success(result);

            return Ok(response);
        }

    }
}
