using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.Recapcha;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecaptchaController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost("GetValidateRecaptcha")]
        public async Task<IActionResult> GetValidateRecaptcha([FromBody] GetValidateRecaptchaCommand command)
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<GetValidateRecaptchaResponse>.Success(result);
            return Ok(response);
        }

    }



}
