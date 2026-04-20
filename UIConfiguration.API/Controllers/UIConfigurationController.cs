using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System;
using System.Net;
using UIConfiguration.API.Response;
using UIConfiguration.Application.UIConfiguration.GetConfiguration;
using UIConfiguration.Application.UIConfiguration.PostConfiguration;
using UIConfiguration.Application.UIConfiguration.PutConfiguration;

namespace UIConfiguration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UIConfigurationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Consulta una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("ConsultarUIConfiguration")]
        public async Task<IActionResult> ConsultarUIConfiguration(Guid agreementGuid, int workflowID, Guid? appFrontGuid = null)
        {
            Log.Information("Received request for ConsultarUIConfiguration with ProcesoConvenioGuid: {agreementGuid} and AppFrontGuid: {AppFrontGuid}", agreementGuid, appFrontGuid);


            var result = await _mediator.Send(new GetConfigurationQuery()
            {
                ProcesoConvenioGuid = agreementGuid,
                WorkflowID = workflowID
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guardar una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("SaveUIConfiguration")]
        public async Task<IActionResult> SaveUIConfiguration([FromBody] object uiConfigurationJson, Guid agreementGuid, Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SaveUIConfiguration with ProcesoConvenioGuid: {agreementGuid} and AppFrontGuid: {AppFrontGuid}", agreementGuid, appFrontGuid);

            var result = await _mediator.Send(new SaveConfigurationQuery()
            {
                UIConfigurationJson = uiConfigurationJson
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guardar una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("UpdateUIConfiguration")]
        public async Task<IActionResult> UpdateUIConfiguration([FromBody] object uiConfigurationJson, Guid agreementGuid, int workflowID, Guid? appFrontGuid = null)
        {
            Log.Information("Received request for SaveUIConfiguration with ProcesoConvenioGuid: {agreementGuid} and AppFrontGuid: {AppFrontGuid}", agreementGuid, appFrontGuid);

            var result = await _mediator.Send(new PutConfigurationQuery()
            {
                UIConfigurationJson = uiConfigurationJson,
                ProcesoConvenioGuid = agreementGuid,
                WorkflowID = workflowID
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consulta una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("ExisteUIConfiguration")]
        public async Task<IActionResult> ExisteUIConfiguration(Guid agreementGuid, int workflowID, Guid? appFrontGuid = null)
        {
            Log.Information("Received request for ConsultarUIConfiguration with ProcesoConvenioGuid: {agreementGuid} and AppFrontGuid: {AppFrontGuid}", agreementGuid, appFrontGuid);


            var result = await _mediator.Send(new GetConfigurationQuery()
            {
                ProcesoConvenioGuid = agreementGuid,
                WorkflowID = workflowID
            });
            var response = ApiResponse<object>.Success(result);

            if (response?.Data != null)
                response.Data = true;
            else
                response!.Data = false;

            return Ok(response);
        }


        /// <summary>
        /// Consulta una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("ConsultarListaPaginasActivas")]
        public async Task<IActionResult> ConsultarListaPaginasActivas(Guid agreementGuid, int workflowID)
        {
            Log.Information("Received request for ConsultarListaPaginasActivas with ProcesoConvenioGuid: {agreementGuid}", agreementGuid);


            var result = await _mediator.Send(new GetPagesWithDisplayTrueQuery()
            {
                ProcesoConvenioGuid = agreementGuid,
                WorkflowID = workflowID
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consulta una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("ConsultarPaginasActivas")]
        public async Task<IActionResult> ConsultarPaginasActivas(Guid agreementGuid, int workflowID, string PageName)
        {
            Log.Information("Received request for ConsultarPaginasActivas with ProcesoConvenioGuid: {agreementGuid} and PageName: {PageName}", agreementGuid, PageName);


            var result = await _mediator.Send(new GetPageIfVisibleQuery()
            {
                ProcesoConvenioGuid = agreementGuid,
                WorkflowID = workflowID,
                PageName = PageName
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consulta una configuracion por clientId
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("ConsultarConfiguracionGlobal")]
        public async Task<IActionResult> ConsultarConfiguracionGlobal(Guid agreementGuid, int workflowID)
        {
            Log.Information("Received request for ConsultarConfiguracionGlobal with ProcesoConvenioGuid: {agreementGuid}", agreementGuid);


            var result = await _mediator.Send(new GetGlobalConfigurationQuery()
            {
                ProcesoConvenioGuid = agreementGuid,
                WorkflowID = workflowID
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza campos dinámicos dentro de la configuración UI.
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Actualización exitosa</response>
        /// <response code="400">Solicitud inválida</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("UpdateDynamicFields")]
        public async Task<IActionResult> UpdateDynamicFields([FromBody] Dictionary<string, object> fieldsToUpdate, Guid agreementGuid, int workflowID,
            Guid? appFrontGuid = null)
        {
            Log.Information("Received request for UpdateDynamicFields for agreementGuid: {agreementGuid}", agreementGuid);

            var result = await _mediator.Send(new PutDynamicFieldQuery(
                agreementGuid,
                workflowID,
                fieldsToUpdate
            ));

            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Agregar campos dinámicos dentro de la configuración UI.
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Agregacion exitosa</response>
        /// <response code="400">Solicitud inválida</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("AddDynamicFields")]
        public async Task<IActionResult> AddDynamicFields([FromBody] Dictionary<string, object> fieldsToAdd, Guid agreementGuid, int workflowID)
        {
            var result = await _mediator.Send(new AddDynamicFieldsQuery(
                agreementGuid,
                workflowID,
                fieldsToAdd
            ));

            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Agregar campos dinámicos dentro de la configuración UI.
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Agregacion exitosa</response>
        /// <response code="400">Solicitud inválida</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("AddUnitProcess")]
        public async Task<IActionResult> AddUnitProcess([FromBody] Dictionary<string, object> fieldsToAdd, Guid agreementGuid, int workflowID)
        {
            var result = await _mediator.Send(new AddIfNotExistsQuery(
                agreementGuid,
                workflowID,
                fieldsToAdd
            ));

            var response = ApiResponse<object>.Success(result!);
            return Ok(response);
        }

        /// <summary>
        /// Agregar campos dinámicos dentro de la configuración UI para todos.
        /// </summary>
        /// <remarks>TEAM Facial PoC 08/05/2025</remarks>
        /// <response code="200">Agregacion exitosa</response>
        /// <response code="400">Solicitud inválida</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("AddAllProcess")]
        public async Task<IActionResult> AddAllProcess([FromBody] Dictionary<string, object> fieldsToAdd)
        {
            var result = await _mediator.Send(new AddIfNotExistsToAllQuery(
                fieldsToAdd
            ));

            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }


    }
}

