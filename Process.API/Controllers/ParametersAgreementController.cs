using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.ParametersAgreement.GetAll;
using Process.Application.ParametersAgreement.GetById;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParametersAgreementController(
        IMediator mediator
    ) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetParametersAgreementQuery());
            var response = ApiResponse<IEnumerable<GetParametersAgreementQueryResponse>>.Success(result);
            return Ok(response);
        }

        [HttpGet("{agreementGuid:Guid}")]
        public async Task<IActionResult> GetByAgreementGuid(
            Guid agreementGuid,
            [FromQuery] IEnumerable<string>? parameterCode = null
        )
        {
            var result = await _mediator.Send(new GetParametersAgreementByIdQuery(agreementGuid, parameterCode));
            var response = ApiResponse<IEnumerable<GetParametersAgreementByIdQueryResponse>>.Success(result);
            return Ok(response);
        }
    }
}
