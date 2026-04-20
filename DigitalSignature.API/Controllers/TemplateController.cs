using DigitalSignature.API.Response;
using DigitalSignature.Application.CreateTemplate;
using DigitalSignature.Application.GetTemplate;
using DigitalSignature.Application.GetTemplateFields;
using DigitalSignature.Application.UpdateTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Obtiene el template
        /// </summary>
        [HttpGet("Get")]
        public async Task<IActionResult> GetAsync(
            long clientId,
            Guid templateSerial
        )
        {
            var result = await _mediator.Send(new GetTemplateQuery(clientId, templateSerial));
            var response = ApiResponse<GetTemplateResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Obtiene los campos del template
        /// </summary>
        [HttpGet("GetFields")]
        public async Task<IActionResult> GetFieldsAsync(
            long clientId,
            Guid templateSerial
        )
        {
            var result = await _mediator.Send(new GetTemplateFieldsQuery(clientId, templateSerial));
            var response = ApiResponse<GetTemplateFieldsResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Crea el template
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateTemplateCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<CreateTemplateResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza el template
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(
            [FromBody] UpdateTemplateCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<UpdateTemplateResponse>.Success(result);
            return Ok(response);
        }
    }
}
