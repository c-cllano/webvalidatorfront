using DrawFlowConfiguration.API.Response;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.WorkflowNode.GetWorkflowNode;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DrawFlowConfiguration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WorkflowNodeConfigurationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        /// <summary>
        /// Consulta 
        /// </summary>
        /// <remarks>31/07/2025</remarks>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="401">Error en la validación del token</response>
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpGet("GetWorkflowNode")]
        public async Task<IActionResult> GetWorkflowNode(int? workFlowNodeID = null, string? name = null)
        {
            var result = await _mediator.Send(new GetWorkflowNodeQuery()
            {
                workFlowNodeID = workFlowNodeID,
                name = name
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }


    }
}
