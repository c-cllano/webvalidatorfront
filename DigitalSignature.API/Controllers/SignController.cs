using DigitalSignature.API.Response;
using DigitalSignature.Application.Sign;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Genera el documento
        /// </summary>
        [HttpPost("Sign")]
        public async Task<IActionResult> SignAsync(
            [FromBody] SignCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<SignResponse>.Success(result);
            return Ok(response);
        }
    }
}
