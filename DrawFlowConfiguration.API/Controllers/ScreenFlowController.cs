using DrawFlowConfiguration.API.Response;
using DrawFlowConfiguration.Application.ScreenFlow.GetScreenFlow;
using DrawFlowConfiguration.Application.ScreenFlow.GetScreenFlowByFilter;
using DrawFlowConfiguration.Application.ScreenFlow.PostScreenFlow;
using DrawFlowConfiguration.Application.ScreenFlow.PutScreenFlow;
using DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Request;
using DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DrawFlowConfiguration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenFlowController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Guardar SaveScreenFlow
        /// </summary>
        /// <remarks>06/10/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [HttpPost("SaveScreenFlow")]
        public async Task<IActionResult> SaveScreenFlow([FromBody] ScreenFlowRequest request)
        {
            var command = new SaveScreenFlowCommand(request);
            var result = await _mediator.Send(command);
            return result ? Ok() : BadRequest("No se pudo realizar el proceso con exito");
        }



        /// <summary>
        /// Consulta GetAllScreenFlow
        /// </summary>
        /// <remarks>06/10/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("GetAllScreenFlow")]
        public async Task<IActionResult> GetAllScreenFlow()
        {
            var result = await _mediator.Send(new GetScreenFlowQuery()
            {

            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }


        /// <summary>
        /// Consulta GetScreenFlowByFilter
        /// </summary>
        /// <remarks>06/10/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("GetScreenFlowByFilter")]
        public async Task<IActionResult> GetScreenFlowByFilter(Guid? AgreementID = null, int? ScreenFlowID = null, int? SelectedIdWorkflow = null)
        {
            var result = await _mediator.Send(new GetScreenFlowByFilterQuery()
            {
                AgreementId = AgreementID,
                ScreenFlowID = ScreenFlowID,
                SelectedIdWorkflow = SelectedIdWorkflow
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }


        /// <summary>
        /// Actualizar UpdateWorkflow
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("UpdateScreenFlow")]
        public async Task<IActionResult> UpdateScreenFlow([FromBody] ScreenFlowResponse request)
        {
            var result = await _mediator.Send(new UpdateScreenFlowCommand(request));
            return result ? Ok() : BadRequest("No se pudo actualizar");
        }
    }
}
