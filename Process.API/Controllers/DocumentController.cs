using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Attributes;
using Process.API.Response;
using Process.Application.UploadFileBlob;
using Process.Application.ValidationProcesses.SaveDocumentBothSides;

namespace Process.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Guarda el documento por ambas caras
        /// </summary>
        [HttpPost("SaveDocumentBothSides")]
        [AuditLog]
        [AccessUriPermission]
        public async Task<IActionResult> SaveDocumentBothSides(
            [FromBody] SaveDocumentBothSidesCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<SaveDocumentBothSidesResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guarda las imagenes en Azure Blob Storage
        /// </summary>
        [HttpPost("UploadFileBlob")]
        public async Task<IActionResult> UploadFileBlob(
            [FromBody] UploadFileBlobCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<UploadFileBlobResponse>.Success(result);
            return Ok(response);
        }
    }
}
