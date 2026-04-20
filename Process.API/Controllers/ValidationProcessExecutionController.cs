using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.ValidationProcessExecutions.UpdateLastStepAndTrazability;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationProcessExecutionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Actualiza el lastStep y trazability de la tabla ValidationProcessExecution 
        /// </summary>
        [HttpPatch("UpdateLastStepAndTrazability")]
        public async Task<IActionResult> UpdateLastStepAndTrazability(
            [FromBody] UpdateLastStepAndTrazabilityCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<UpdateLastStepAndTrazabilityResponse>.Success(result);
            return Ok(response);
        }
    }
}
