using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Attributes;
using Process.API.Response;
using Process.Application.ValidationProcesses.CancelProcess;
using Process.Application.ValidationProcesses.CompareFaces;
using Process.Application.ValidationProcesses.ConsultValidation;
using Process.Application.ValidationProcesses.ConsultValidationLegacy;
using Process.Application.ValidationProcesses.SaveBiometric;
using Process.Application.ValidationProcesses.SaveBiometricV3;
using Process.Application.ValidationProcesses.StatusValidationRequest;
using Process.Application.ValidationProcesses.UpdateStatusValidationProcess;
using Process.Application.ValidationProcesses.ValidateBiometric;
using Process.Application.ValidationProcesses.ValidateBiometricV3;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationProcessController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Actualiza los estados de la tabla ValidationProcess
        /// </summary>
        [HttpPatch("UpdateStatus")]
        public async Task<IActionResult> UpdateStatusValidationProcess(
            [FromBody] UpdateStatusValidationProcessCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<UpdateStatusValidationProcessResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Compara los rostros
        /// </summary>
        [HttpPost("CompareFaces")]
        [AuditLog]
        [AccessUriPermission]
        public async Task<IActionResult> CompareFaces(
            [FromBody] CompareFacesCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<CompareFacesResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guarda la biometría
        /// </summary>
        [HttpPost("SaveBiometric")]
        [AuditLog]
        [AccessUriPermission]
        public async Task<IActionResult> SaveBiometric(
            [FromBody] SaveBiometricCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<SaveBiometricResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Guarda la biometría V3
        /// </summary>
        [HttpPost("SaveBiometricV3")]
        [AuditLog]
        [AccessUriPermission]
        public async Task<IActionResult> SaveBiometricV3(
            [FromForm] SaveBiometricV3Command command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<SaveBiometricResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Realiza la validación de solicitud de estado
        /// </summary>
        [HttpPost("StatusValidationRequest")]
        [AuditLog]
        [AccessUriPermission]
        public async Task<IActionResult> StatusValidationRequest(
            [FromBody] StatusValidationRequestCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<StatusValidationRequestResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Valida la biometría
        /// </summary>
        [HttpPost("ValidateBiometric")]
        [AuditLog]
        [AccessUriPermission]
        public async Task<IActionResult> ValidateBiometric(
            [FromBody] ValidateBiometricCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<ValidateBiometricResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Valida la biometría V3
        /// </summary>
        [HttpPost("ValidateBiometricV3")]
        [AuditLog]
        [AccessUriPermission]
        public async Task<IActionResult> ValidateBiometricV3(
            [FromForm] ValidateBiometricV3Command command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<ValidateBiometricResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consulta la validación de biometría
        /// </summary>
        [HttpPost("ConsultValidation")]
        [AuditLog]
        [AccessUriPermission]
        public async Task<IActionResult> ConsultValidation(
            [FromBody] ConsultValidationCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<ConsultValidationResponse>.Success(result);
            return Ok(response);
        }

        /// <summary>
        /// Consulta la validación de biometría legacy
        /// </summary>
        [HttpPost("ConsultValidationLegacy")]
        [AccessUriPermission]
        public async Task<IActionResult> ConsultValidationLegacy(
            [FromBody] ConsultValidationLegacyCommand command
        )
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // Inicio código generado por GitHub Copilot
        /// <summary>
        /// Cancela el proceso de validación
        /// </summary>
        [HttpPost("CancelProcess")]
        [AccessUriPermission]
        public async Task<IActionResult> CancelProcess(
            [FromBody] CancelProcessCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<CancelProcessResponse>.Success(result);
            return Ok(response);
        }
        // Fin código generado por GitHub Copilot
    }
}
