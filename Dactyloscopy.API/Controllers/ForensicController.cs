using Dactyloscopy.API.Response;
using Dactyloscopy.Application.GetForensicStatus;
using Dactyloscopy.Application.RequestForensicReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dactyloscopy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForensicController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Solicitar revisión forense
        /// </summary>
        [HttpPost("RequestForensicReview")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] RequestForensicReviewCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<RequestForensicReviewResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consultar estado forense
        /// </summary>
        [HttpGet("GetForensicStatus")]
        public async Task<IActionResult> GetAsync(
            Guid txGuid
        )
        {
            var result = await _mediator.Send(new GetForensicStatusQuery(txGuid));
            var response = ApiResponse<GetForensicStatusResponse>.Success(result);
            return Ok(response);
        }
    }
}
