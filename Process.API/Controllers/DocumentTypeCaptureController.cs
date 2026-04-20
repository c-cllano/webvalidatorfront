using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.DocumentTypeCapture.GetByCode;
using System.Net;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentTypeCaptureController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var result = await _mediator.Send(new GetDocumentTypeCaptureByCodeQuery(code));
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }
    }
}