using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Attributes;
using Process.API.Response;
using Process.Application.AgreementProcess.GetConsultProcess;
using Process.Application.AgreementProcess.GetProcess;
using Serilog;
using System.Net;

namespace Process.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreementProcessController(IMediator mediator) : ControllerBase
    {

        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Consulta un proceo por id
        /// </summary>
        /// <remarks>TEAM Facial PoC 25/03/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<GetProcessResponse>), (int)HttpStatusCode.OK)]
        [HttpGet("ConsultarProcesoWebToken")]
        public async Task<IActionResult> ConsultarProcesoWebToken(Guid procesoConvenioGuid, Guid? appFrontGuid = null)
        {
            Log.Information("Received request for ConsultarProcesoWebToken with ProcesoConvenioGuid:" +
                " {ProcesoConvenioGuid} and AppFrontGuid: {AppFrontGuid}", procesoConvenioGuid, appFrontGuid);


            var result = await _mediator.Send(new GetProcessQuery()
            {
                ProcesoConvenioGuid = procesoConvenioGuid,
                AppFrontGuid = appFrontGuid
            });
            var response = ApiResponse<GetProcessResponse>.Success(result);
            return Ok(response);
        }



        [HttpPost("ConsultarProcesoConvenio")]
        [AuditLog]
        [AccessUriPermission]
        [ProducesResponseType(typeof(ApiResponse<GetConsultProcessResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ConsultProcessAgreement(Guid guid)
        {
            var result = await _mediator.Send(new GetConsultProcessQuery()
            {
                ProcessAgreement = guid
            });

            var response = ApiResponse<GetConsultProcessResponse>.Success(result);

            return Ok(response);
        }
    }
}
