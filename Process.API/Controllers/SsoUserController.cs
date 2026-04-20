using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.Sso.User;
using Process.Application.Sso.User.Dto;
using Process.Domain.Entities;
using Process.Domain.Parameters.Sso.User;
using System.Net;

namespace Process.API.Controllers
{
    [Route("api/Sso/User")]
    [ApiController]
    public class SsoUserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest CreateUserRequest)
        {
            var result = await _mediator.Send(new RegisterQuery()
            {
                DocumentType = CreateUserRequest.DocumentType,
                DocumentNumber = CreateUserRequest.DocumentNumber,
                FirstName = CreateUserRequest.FirstName,
                SecondName = CreateUserRequest.SecondName,
                LastName = CreateUserRequest.LastName,
                SecondLastName = CreateUserRequest.SecondLastName,
                Email = CreateUserRequest.Email,
                UserName = CreateUserRequest.UserName,
                EmailConfirmed = CreateUserRequest.EmailConfirmed,
                CreatedUserId = CreateUserRequest.CreatedUserId, 
                ClientId = CreateUserRequest.ClientId,
                AssignmentRoleAgreements = CreateUserRequest.AssignmentRoleAgreements,
                CellPhone = CreateUserRequest.CellPhone,
                RoleId = CreateUserRequest.RoleId

            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }

        [ProducesResponseType(typeof(ApiResponse<UserSummaryDto>), (int)HttpStatusCode.OK)]
        [HttpGet("GetUser/{userId:int}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var result = await _mediator.Send(new GetUserQuery() { UserId = userId });
            var response = ApiResponse<UserSummaryDto>.Success(result);
            return Ok(response);
        }

        [HttpGet("GetUsersByClient/{clientId:long}")]
        public async Task<IActionResult> GetUsersByClient(
            long clientId,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] string? email,
            [FromQuery] string? firstName,
            [FromQuery] string? lastName,
            [FromQuery] string? secondName,
            [FromQuery] string? secondLastName)
        {
            var result = await _mediator.Send(new GetUsersQuery()
            {
                ClientId = clientId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                SecondName = secondName,
                SecondLastName = secondLastName
            });

            if (!result.Success)
            {
                var errorResponse = result.StatusCode.HasValue
                    ? ApiResponse<object>.Error(result.Data ?? new object(), result.StatusCode.Value, result.StatusCode.Value.ToString())
                    : ApiResponse<object>.Error(result.Data ?? new object(), 400, "400");
                return StatusCode(result.StatusCode ?? 400, errorResponse);
            } 
            return Ok(ApiResponse<List<UserSso>>.Success(result.Data!));
        }

        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("{userId:int}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest updateUserRequest, [FromQuery] long updaterUserId)
        {
            var result = await _mediator.Send(new UpdateUserQuery()
            {
                UserId = userId,
                FirstName = updateUserRequest.FirstName,
                SecondName = updateUserRequest.SecondName,
                LastName = updateUserRequest.LastName,
                SecondLastName = updateUserRequest.SecondLastName,
                Email = updateUserRequest.Email,
                UserName = updateUserRequest.UserName,
                DocumentType = updateUserRequest.DocumentType,
                DocumentNumber = updateUserRequest.DocumentNumber,
                ClientId = updateUserRequest.ClientId,
                RoleId = updateUserRequest.RoleId,
                CellPhone = updateUserRequest.CellPhone,
                AssignmentRoleAgreements = updateUserRequest.AssignmentRoleAgreements,
                UpdaterUserId = updaterUserId,
                Status = updateUserRequest.Status
            });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }
         
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpPut("Assignments")]
        public async Task<IActionResult> UpdateUserAssignments(
    [FromBody] UpdateUserAssignmentsQuery request)
        {
            var result = await _mediator.Send(
                new UpdateUserAssignmentsQuery () { AssignmentRoleAgreements = request.AssignmentRoleAgreements ,
                    UpdaterUserId = request.UpdaterUserId, 
                    UserId = request.UserId}
                   
                );

            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }


        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _mediator.Send(new DeleteUserQuery() { userId = userId });
            var response = ApiResponse<object>.Success(result);
            return Ok(response);
        }
    }
}