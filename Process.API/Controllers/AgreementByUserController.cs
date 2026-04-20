using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.AgreementByUser.Create;
using Process.Application.AgreementByUser.GetByGuid;
using Process.Application.AgreementByUser.GetByUserId;
using System.Net;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgreementByUserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        /// <summary>
        /// Get AgreementByUser by UserId
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of AgreementByUser</returns>
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _mediator.Send(new GetAgreementByUserByUserIdQuery { UserId = userId });
             return Ok(result);
        }

        /// <summary>
        /// Create a new AgreementByUser record
        /// </summary>
        /// <param name="request">AgreementByUser creation request</param>
        /// <returns>Created AgreementByUser record</returns>
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAgreementByUserCommand request)
        {
            var result = await _mediator.Send(request);
            var response = ApiResponse<long>.Success(result);
            return Ok(response);
        }


        [HttpGet("guid/{Guid}")]
        public async Task<IActionResult> GetAgreementByGuid(Guid Guid)
        {
            var result = await _mediator.Send(new GetAgreementByUserByGuidQuery { Guid = Guid });
            return Ok(result);
        }

    }
}
