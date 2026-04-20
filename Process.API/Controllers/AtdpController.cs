using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.Application.Atdpt.Version;
using Process.Application.Atdpt.Transactions;
using Process.Domain.Parameters.Atdpt;
using Process.Application.Atdpt.Transactions.Querys;
using Process.Domain.ValueObjects;
using Process.API.Response;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtdpController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AtdpController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene la versión del archivo por ID
        /// </summary>
        [HttpGet("version/{atdpId}")]
        public async Task<IActionResult> GetFileVersionById(int atdpId, [FromQuery] Guid AgreementGuid)
        {
            var result = await _mediator.Send(new GetFileVersionByIdQuery(atdpId, AgreementGuid));
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

            /// <summary>
            /// Obtiene la transacción del archivo por ID
            /// </summary>
        [HttpGet("transaction/{atdpTransactionId}")]
        public async Task<IActionResult> GetFileTransactionById(int atdpTransactionId, [FromQuery] Guid AgreementGuid)
        {
            var result = await _mediator.Send(new GetFileTransactionByIdQuery(atdpTransactionId, AgreementGuid));
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guarda una transacción
        /// </summary>
        [HttpPost("SaveTransaction")]
        public async Task<IActionResult> SaveTransaction([FromBody] SaveTransactionRequest request, [FromQuery] Guid AgreementGuid)
        {
            var command = new SaveTransactionCommand(request, AgreementGuid);
            var result = await _mediator.Send(command);
            var response = ApiResponse<object>.Success(result);
            return result != null ? Ok(response) : BadRequest("No se pudo realizar el proceso con exito");
        }
    }
}
