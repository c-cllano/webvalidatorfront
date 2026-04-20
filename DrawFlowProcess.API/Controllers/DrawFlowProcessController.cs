using DrawFlowProcess.API.Response;
using DrawFlowProcess.Application.DrawFlowProcess.GetGlobalConfiguration;
using DrawFlowProcess.Application.DrawFlowProcess.GetGlobalConfigurationByFlow;
using DrawFlowProcess.Application.DrawFlowProcess.GetJsonConvert;
using DrawFlowProcess.Application.DrawFlowProcess.GetJsonPages;
using DrawFlowProcess.Application.DrawFlowProcess.GetProcessFlow;
using DrawFlowProcess.Application.DrawFlowProcess.GteJsonFilter;
using DrawFlowProcess.Application.DrawFlowProcess.PostGlobalConfiguration;
using DrawFlowProcess.Application.DrawFlowProcess.PostJsonConvert;
using DrawFlowProcess.Application.DrawFlowProcess.PutGlobalConfiguration;
using DrawFlowProcess.Domain.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Serilog;
using System.Net;
using System.Text.Json;

namespace DrawFlowProcess.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawFlowProcessController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Transforma un json drawflow por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 11/07/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost("TransformJsonDrwaFlow")]
        public async Task<IActionResult> TransformJsonDrwaFlow([FromBody] JsonDocument drawflowExport, 
            [FromQuery] Guid agreementId, 
            [FromQuery] Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SavedrawFlow with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);


            var result = await _mediator.Send(new GetJsonConvertQuery()
            {
                ProcesoConvenioGuid = agreementId,
                JsonDocument = drawflowExport
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guardar una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 08/05/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [HttpPost("SaveDrawFlow")]
        public async Task<IActionResult> SaveDrawFlow([FromBody] JsonDocument jsonDocument, Guid agreementId, Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SavedrawFlow with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);

            var result = await _mediator.Send(new SaveJsonConvertQuery()
            {
                Document = jsonDocument
            });
            var response = ApiResponse<bool>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Transforma un json drawflow por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 11/07/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet("GetJsonDrwaFlow")]
        public async Task<IActionResult> GetJsonDrwaFlow([FromQuery] int workFlowId,
            [FromQuery] Guid agreementId,
            [FromQuery] Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SavedrawFlow with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);


            var result = await _mediator.Send(new GetJsonFilterQuery()
            {
                AgreementId = agreementId,
                WorkflowId = workFlowId
            });
            var response = ApiResponse<ExportJson>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Transforma un json drawflow por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 11/07/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost("GetProcessFlow")]
        public async Task<IActionResult> GetProcessFlow([FromBody] JsonDocument conditional,
            [FromQuery] int workFlowId,
            [FromQuery] Guid agreementId,
            [FromQuery] string typeCurrent,
            [FromQuery] int typeProcess,
            [FromQuery] bool back,
            [FromQuery] Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SavedrawFlow with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);


            var result = await _mediator.Send(new GetProcessFlowQuery()
            {
                AgreeentId = agreementId,
                WorkFlowId = workFlowId,
                NameType = typeCurrent,
                Conditional = conditional,
                TypeProcess = typeProcess,
                Back = back
            });
            var response = ApiResponse<GetProcessFlowResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Transforma un json drawflow por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 11/07/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet("GetJsonPages")]
        public async Task<IActionResult> GetJsonPages([FromQuery] int workFlowId,
            [FromQuery] Guid agreementId,
            [FromQuery] Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SavedrawFlow with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);


            var result = await _mediator.Send(new GetJsonPagesQuery()
            {
                AgreementId = agreementId,
                WorkFlowId = workFlowId
            });
            var response = ApiResponse<GetJsonPagesResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Transforma un json drawflow por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 11/07/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet("GetGlobalConfiguration")]
        public async Task<IActionResult> GetGlobalConfiguration([FromQuery] int workFlowId,
            [FromQuery] Guid agreementId,
            [FromQuery] DateTime createDateTask,
            [FromQuery] string section,
            [FromQuery] Guid? appFrontGuid = null)
        {
            Log.Information("Received request for GetGlobalConfiguration with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);


            var result = await _mediator.Send(new GetGlobalConfigurationQuery()
            {
                AgreementId = agreementId,
                WorkFlowId = workFlowId,
                CreateDateTask = createDateTask,
                Section = section
            });
            var response = ApiResponse<List<GetGlobalConfigurationResponse>>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guardar una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 08/05/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [HttpPost("SaveGlobalConfiguration")]
        public async Task<IActionResult> SaveGlobalConfiguration([FromBody] JsonDocument jsonDocument, Guid agreementId, Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SaveGlobalConfiguration with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);

            var result = await _mediator.Send(new SaveGlobalConfigurationCommand()
            {
                Document = jsonDocument
            });
            var response = ApiResponse<bool>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guardar una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 08/05/2025</remarks>
        /// <response code="200">Actualizado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [HttpPut("UpdateGlobalConfiguration")]
        public async Task<IActionResult> UpdateGlobalConfiguration([FromBody] JsonDocument jsonDocument, [FromQuery] Guid agreementId, [FromQuery] int workFlowId ,Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SaveGlobalConfiguration with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);

            var result = await _mediator.Send(new UpdateGlobalConfigurationCommand()
            {
                Document = jsonDocument,
                AgreementId = agreementId,
                WorkFlowId = workFlowId,                
            });
            var response = ApiResponse<bool>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Transforma un json drawflow por clientId
        /// </summary>
        /// <remarks>TEAM DrawFlow PoC 11/07/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet("GetGlobalConfigurationByFlow")]
        public async Task<IActionResult> GetGlobalConfigurationByFlow([FromQuery] int workFlowId,
            [FromQuery] Guid agreementId,
            [FromQuery] Guid? appFrontGuid = null)
        {
            Log.Information("Received request for GetGlobalConfiguration with AgreementId: {AgreementId} and AppFrontGuid: {AppFrontGuid}", agreementId, appFrontGuid);


            var result = await _mediator.Send(new GetGlobalConfigurationByFlowQuery()
            {
                AgreementId = agreementId,
                WorkFlowId = workFlowId
            });
            var response = ApiResponse<JsonDocument>.Success(result);
            return Ok(response);
        }
    }
}
