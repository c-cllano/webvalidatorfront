using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UITemplate.API.Response;
using UITemplate.Application.UITemplate.GetTemplate;

namespace UITemplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UITemplateController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Consulta una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("ConsultarUITemplate")]
        public async Task<IActionResult> ConsultarUITemplate(Guid agreementGuid, Guid? appFrontGuid = null)
        {
            Log.Information("Received request for ConsultarUITemplate with agreementGuid: {agreementGuid} and AppFrontGuid: {AppFrontGuid}", agreementGuid, appFrontGuid);


            var result = await _mediator.Send(new GetTemplateQuery()
            {
                AgreementGuid = agreementGuid
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }
    }
}
