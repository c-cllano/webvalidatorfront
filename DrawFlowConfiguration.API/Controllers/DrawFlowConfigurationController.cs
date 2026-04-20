using DrawFlowConfiguration.API.Response;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.ArchiveWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.DeleteJsonWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.DesarchiveWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.DuplicateSQLWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetCountriesByRegion;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentosFromItem;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentTypeByCountry;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetExistByNameAndGUID;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetJsonWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetNationalitiesFromItem;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.GetUbicacionesFromItem;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.PauseWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.PostJsonWorkflows;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.PostTemplate;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.PostWorkflows;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.PublicarWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.PutJsonWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.DeleteWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflowsByFilter;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.PutWorkflow;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowNacionalidadesPermitidas;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowTipoDocumento;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowUbicacionesPermitidas;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DrawFlowConfiguration.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class DrawFlowConfigurationController(IMediator mediator) : ControllerBase
    {
        private const string MensajeErrorProceso = "No se pudo realizar el proceso con éxito";
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Guardar Workflows
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [HttpPost("Workflows")]
        public async Task<IActionResult> SaveTransaction([FromBody] WorkflowsEntry request)
        {
            var command = new SaveWorflowCommand(request);
            var result = await _mediator.Send(command);
            return result ? Ok() : BadRequest(MensajeErrorProceso);
        }


        /// <summary>
        /// Duplicar Workflows
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [HttpPost("duplicateSQLWorkflow")]
        public async Task<IActionResult> duplicateSQLWorkflow([FromBody] WorkflowsEntry request)
        {
            var command = new DuplicateSQLWorkflowCommand(request);
            var result = await _mediator.Send(command);
            return result ? Ok() : BadRequest(MensajeErrorProceso);
        }

        /// <summary>
        /// Guardar Template
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [HttpPost("Template")]
        public async Task<IActionResult> Template([FromBody] TemplateEntry request)
        {
            var command = new SaveTemplateCommand(request);
            var result = await _mediator.Send(command);
            return result ? Ok() : BadRequest(MensajeErrorProceso);

        }

        /// <summary>
        /// Consulta una configuracion por clientId
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("ConsultarWorkflow")]
        public async Task<IActionResult> ConsultarWorkflow(int WorkFlowID)
        {
            var result = await _mediator.Send(new GetWorkflowQuery()
            {
                ProcesoConvenioGuid = WorkFlowID
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consulta una configuracion por clientId
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("ExistByNameAndGUID")]
        public async Task<IActionResult> ExistByNameAndGUID(
         Guid _AgreementId,
         string _name,
         int? _workflowId = null)  
        {
            var result = await _mediator.Send(new GetExistByNameAndGUIDQuery()
            {
                AgreementID = _AgreementId,
                Name = _name,
                WorkflowID = _workflowId  
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
        [HttpPut("UpdateWorkflow")]
        public async Task<IActionResult> UpdateWorkflow([FromBody] SaveWorflowRequest request)
        {
            var result = await _mediator.Send(new UpdateWorkflowCommand(request));
            return result ? Ok() : BadRequest("No se pudo actualizar");
        }


        /// <summary>
        /// Guardar SaveWorkflowJson
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("SaveWorkflowJson")]
        public async Task<IActionResult> SaveWorkflowJson([FromBody] object drawflow, int WorkFlowID, Guid? AgreementID = null)
        {
            var result = await _mediator.Send(new SaveJsonWorkflowQuery()
            {
                drawflow = drawflow,
                WorkFlowID = WorkFlowID,
                AgreementID = AgreementID,
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);

        }



        /// <summary>
        /// Actualizar UpdateWorkflowJson
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("UpdateWorkflowJson")]
        public async Task<IActionResult> UpdateWorkflowJson([FromBody] object drawflow, int WorkFlowID, Guid AgreementID)
        {
            var result = await _mediator.Send(new PutJsonWorkflowQuery()
            {
                AgreementID = AgreementID,
                WorkFlowID = WorkFlowID,
                drawflow = drawflow
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);

        }

        /// <summary>
        /// Consulta GetWorkflowJson
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("GetWorkflowJson")]
        public async Task<IActionResult> GetWorkflowJson(Guid? AgreementID = null, int? WorkFlowID = null)
        {
            var result = await _mediator.Send(new GetJsonWorkflowQuery()
            {
                AgreementID = AgreementID,
                WorkFlowID = WorkFlowID
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }



        /// <summary>
        /// Eliminar DeleteWorkflowJson
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code = "200" > Eliminado exitoso</response>
        /// <response code = "404" > No encontrado</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpDelete("DeleteWorkflowJson")]
        public async Task<IActionResult> DeleteWorkflowJson([FromQuery] int WorkFlowID, [FromQuery] Guid AgreementID)
        {
            var result = await _mediator.Send(new DeleteJsonWorkflowCommand
            {
                AgreementID = AgreementID,
                WorkFlowID = WorkFlowID
            });

            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }



        /// <summary>
        /// Eliminar DeleteWorkflow(
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code = "200" > Eliminado exitoso</response>
        /// <response code = "404" > No encontrado</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpDelete("DeleteWorkflow")]
        public async Task<IActionResult> DeleteWorkflow([FromQuery] DeleteWorflowNodeRequest request)
        {
            var result = await _mediator.Send(new DeleteWorkflowNodeCommand(request));
            return result ? Ok() : BadRequest("No se pudo actualizar");
        }


        /// <summary>
        /// Archive DeleteWorkflow(
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code = "200" > Eliminado exitoso</response>
        /// <response code = "404" > No encontrado</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("ArchiveWorkflow")]
        public async Task<IActionResult> ArchiveWorkflow(
            [FromBody] ArchiveWorkflowRequest request
        )
        {
            var result = await _mediator.Send(new ArchiveWorkflowCommand(request));
            return result ? Ok() : BadRequest("No se pudo actualizar");
        }



        /// <summary>
        /// PublicarWorkflow(
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code = "200" > Eliminado exitoso</response>
        /// <response code = "404" > No encontrado</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("PublicarWorkflow")]
        public async Task<IActionResult> PublicarWorkflow(
            [FromBody] PublicarWorkflowRequest request
        )
        {
            var result = await _mediator.Send(new PublicarWorkflowCommand(request));
            return result ? Ok() : BadRequest("No se pudo actualizar");
        }

        /// <summary>
        /// Archive DeleteWorkflow(
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code = "200" > Eliminado exitoso</response>
        /// <response code = "404" > No encontrado</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("DesarchiveWorkflow")]
        public async Task<IActionResult> DesarchiveWorkflow(
            [FromBody] DesarchiveWorkflowRequest request
        )
        {
            var result = await _mediator.Send(new DesarchiveWorkflowCommand(request));
            return result ? Ok() : BadRequest("No se pudo actualizar");
        }


        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("PauseWorkflow")]
        public async Task<IActionResult> PauseWorkflow(
    [FromBody] PauseWorkflowRequest request
)
        {
            var result = await _mediator.Send(new PauseWorkflowCommand(request));
            return result ? Ok() : BadRequest("No se pudo actualizar");
        }

        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkflowsEntry>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetWorkflowsByFilter")]
        public async Task<IActionResult> GetWorkflowsByFilter(
    Guid? agreementId = null,
    int? workFlowId = null,
    string? status = null)
        {
            var result = await _mediator.Send(new GetWorkflowsByFilterQuery
            {
                AgreementId = agreementId,
                WorkFlowId = workFlowId,
                Status = status?.Trim() 
            });

            return Ok(ApiResponse<IEnumerable<WorkflowsEntry>>.Success(result));
        }



        /// <summary>
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("SaveWorkflowNacionalidadesPermitidas")]
        public async Task<IActionResult> SaveWorkflowNacionalidadesPermitidas([FromBody] SaveWorkflowNacionalidadesPermitidasRequest request)
        {

            var command = new SaveWorkflowNacionalidadesPermitidasCommand(request);
            var result = await _mediator.Send(command);
            return result ? Ok() : BadRequest(MensajeErrorProceso);

        }


        /// <summary>
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("SaveWorkflowUbicacionesPermitidas")]
        public async Task<IActionResult> SaveWorkflowUbicacionesPermitidas([FromBody] SaveWorkflowUbicacionesPermitidasRequest request)
        {
            var command = new SaveWorkflowUbicacionesPermitidasCommand(request);
            var result = await _mediator.Send(command);
            return result ? Ok() : BadRequest(MensajeErrorProceso);
        }


        /// <summary>
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Guardado exitoso</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("SaveWorkflowTipoDocumento")]
        public async Task<IActionResult> SaveWorkflowTipoDocumento([FromBody] SaveWorkflowTipoDocumentoRequest request)
        {
            var command = new SaveWorkflowTipoDocumentoCommand(request);
            var result = await _mediator.Send(command);
            return result ? Ok() : BadRequest(MensajeErrorProceso);
        }


        /// <summary>
        /// Consulta getNationalitiesFromItem
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("GetNationalitiesFromItem")]
        public async Task<IActionResult> getNationalitiesFromItem(int WorkFlowID)
        {
            var result = await _mediator.Send(new GetNationalitiesFromItemQuery()
            {
                WorkFlowID = WorkFlowID
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consulta getDocumentosFromItem
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("GetDocumentosFromItem")]
        public async Task<IActionResult> getDocumentosFromItem(int WorkFlowID)
        {
            var result = await _mediator.Send(new GetDocumentosFromItemQuery()
            {
                WorkFlowID = WorkFlowID
            });

            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consulta getUbicacionesFromItem
        /// </summary>
        /// <remarks>04/08/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("GetUbicacionesFromItem")]
        public async Task<IActionResult> getUbicacionesFromItem(int WorkFlowID)
        {
            var result = await _mediator.Send(new GetUbicacionesFromItemQuery()
            {
                WorkFlowID = WorkFlowID
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }


        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetCountriesAndRegionsQueryResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetCountriesByRegion")]
        public async Task<IActionResult> GetCountriesByRegion()
        {
            var result = await mediator.Send(new GetCountriesAndRegionsQuery());
            var response = ApiResponse<IEnumerable<GetCountriesAndRegionsQueryResponse>>.Success(result);
            return Ok(response);
        }

        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetDocumentTypeByCountryQueryResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetDocumentTypeByCountry")]
        public async Task<IActionResult> GetDocumentTypeByCountry()
        {
            var result = await mediator.Send(new GetDocumentTypeByCountryQuery());
            var response = ApiResponse<IEnumerable<GetDocumentTypeByCountryQueryResponse>>.Success(result);
            return Ok(response);
        }
    }

}
