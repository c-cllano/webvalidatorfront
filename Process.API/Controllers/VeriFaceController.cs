using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.VeriFace;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeriFaceController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Obtiene el token del proceso de VeriFace
        /// </summary>
        [HttpGet("GetTokenVeriFace")]
        public async Task<IActionResult> GetTokenVeriFaceAsync()
        {
            var result = await _mediator.Send(new GetTokenVeriFaceQuery());
            var response = ApiResponse<GetTokenVeriFaceResponse>.Success(result);
            return Ok(response);
        }
    }
}
