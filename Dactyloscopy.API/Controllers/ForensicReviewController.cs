using Dactyloscopy.API.Response;
using Dactyloscopy.Application.GetForensicReviewProcessInReview;
using Dactyloscopy.Application.UpdateForensicReviewProcess;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dactyloscopy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForensicReviewController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Consulta todos los procesos forenses que esten en estado 'En revisión'
        /// </summary>
        [HttpGet("GetForensicReviewProcessInReview")]
        public async Task<IActionResult> GetForensicReviewProcessInReviewAsync()
        {
            var result = await _mediator.Send(new GetForensicReviewProcessInReviewQuery());
            var response = ApiResponse<IEnumerable<GetForensicReviewProcessResponse>>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza el proceso forense
        /// </summary>
        [HttpPut("UpdateForensicReviewProcess")]
        public async Task<IActionResult> UpdateForensicReviewProcessAsync(
            [FromBody] UpdateForensicReviewProcessCommand command
        )
        {
            await _mediator.Send(command);
            var response = ApiResponse.Success();
            return Ok(response);
        }
    }
}
