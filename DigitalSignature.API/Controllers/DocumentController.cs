using DigitalSignature.API.Response;
using DigitalSignature.Application.GenerateDocument;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Genera el documento
        /// </summary>
        [HttpPost("GenerateDocument")]
        public async Task<IActionResult> GenerateDocumentAsync(
            [FromBody] GenerateDocumentCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<GenerateDocumentResponse>.Success(result);
            return Ok(response);
        }
    }
}
