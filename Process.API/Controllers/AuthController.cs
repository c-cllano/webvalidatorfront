using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.Application.AgreementProcess.GetProcess;
using Process.Application.Auth.GetToken;
using Process.Domain.Parameters.Auth;
using System.Net;

namespace Process.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {

        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Genera un token
        /// </summary>      
        [ProducesResponseType(typeof(GetProcessResponse), (int)HttpStatusCode.OK)]
        [HttpPost]
        [Route("TraerToken")]
        //[Authorize(Roles = "Audit")]
        public async Task<IActionResult> GenerateToken([FromBody] ClientCredentialEntry credential)
        {
            var result = await _mediator.Send(new GetTokenQuery() { ClientId = credential.ClientId, ClientSecret = credential.ClientSecret });
            return Ok(result);
        }
    }
}
