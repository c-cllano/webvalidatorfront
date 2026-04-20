using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.Sso.Account.ChangePassword;
using Process.Application.Sso.Account.ForgotPassword;
using Process.Application.Sso.Account.GetTokenUsed;
using Process.Application.Sso.Account.ResetPassword;
using Process.Application.Sso.Authentications;
using Process.Application.Sso.Client;
using Process.Application.Sso.Configuration;
using Process.Domain.Parameters.Sso.Account;
using Process.Domain.Parameters.Sso.Authentications;
using Process.Domain.Parameters.Sso.Client;
using Process.Domain.Parameters.Sso.Configuration;
using System.Net;

namespace Process.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SsoController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Token")]
        public async Task<IActionResult> Token([FromBody] ClientCredential credential)
        {
            var result = await _mediator.Send(new GetTokenQuery(
                    credential.GrantType,
                    credential.ClientId,
                    credential.Username,
                    credential.Password,
                    credential.ClientSecret,
                    credential.RefreshToken,
                    credential.ClientType
                ));

            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 400, result);
            }

            var successResponse = ApiResponse<GetTokenResponse>.Success(result.Data!);
            return Ok(successResponse);
        }

        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPassword)
        {
            var result = await _mediator.Send(new ForgotPasswordCommand()
            {
                Email = forgotPassword.Email,
                Token = forgotPassword.Token
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPassword)
        {
            var result = await _mediator.Send(new ResetPasswordCommand
            {
                Email = resetPassword.Email,
                Token = resetPassword.Token,
                NewPassword = resetPassword.NewPassword
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var result = await _mediator.Send(new ChangePasswordCommand()
            {
                Email = changePasswordRequest.Email,
                OldPassword = changePasswordRequest.OldPassword,
                NewPassword = changePasswordRequest.NewPassword
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("CreateClient")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClienteRequest createCliente)
        {
            var result = await _mediator.Send(new CreateClientQuery() { ClientId = createCliente.ClientId, DisplayName = createCliente.DisplayName, Description = createCliente.Description, ClientType = createCliente.ClientType, ClientSecret = createCliente.ClientSecret });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("Application/Create")]
        public async Task<IActionResult> ApplicationCreate([FromBody] ApplicationCreateRequest applicationCreate)
        {
            var result = await _mediator.Send(new ApplicationCreateQuery() { ClientId = applicationCreate.ClientId, DisplayName = applicationCreate.DisplayName, Description = applicationCreate.Description, ClientType = applicationCreate.ClientType, ClientSecret = applicationCreate.ClientSecret });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [HttpGet("GetTokenUsed")]
        public async Task<IActionResult> GetTokenUsed([FromQuery] string token, [FromQuery] string email)
        {
            var result = await _mediator.Send(new GetTokenUsedQuery() { Token = token, Email = email });
            var response = ApiResponse<bool>.Success(result);
            return Ok(response);
        }
    }
}
