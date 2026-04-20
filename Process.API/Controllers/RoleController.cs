using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.Application.Roles.Create;
using Process.Application.Roles.Delete;
using Process.Application.Roles.GetByClient;
using Process.Application.Roles.GetId;
using Process.Application.Roles.Update;
using Process.Application.Roles.GetPermissions;
using Process.Application.Roles.UpdatePermissions;
using Process.Domain.Entities;
using System.Net;
using Process.Application.Roles.GetByUser;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.OK)]
        [HttpGet("GetRoles/{clientGuid}")]
        public async Task<IActionResult> GetAll(
            Guid clientGuid,
            [FromQuery] string[]? rolName,
            [FromQuery] string? status,
            [FromQuery] int? pageSize,
            [FromQuery] int? pageNumber)
        {
            var result = await _mediator.Send(
                 new GetRoleByClientQuery(clientGuid, rolName, status, pageSize, pageNumber)
            );
            return Ok(result);
        }


        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.NotFound)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {
            var result = await _mediator.Send(new CreateRoleCommand(
                request.Name,
                request.Active,
                request.ClientGuid,
                request.CreatorUserId,
                request.Permissions
            ));

            return Ok(result);
        }


        [ProducesResponseType(typeof(SsoServiceResult<RoleByIdResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(SsoServiceResult<RoleByIdResponse>), (int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(long id)
        {
            var result = await _mediator.Send(new GetRoleByIdQuery(id));
            return Ok(result);
        }

        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateRoleRequest request)
        {
            var result = await _mediator.Send(new UpdateRoleCommand(id, request.Name, request.Active));
            return Ok(result);
        }

        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.Conflict)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _mediator.Send(new DeleteRoleCommand(id));
            return Ok(result);
        }

        [ProducesResponseType(typeof(SsoServiceResult<List<long>>), (int)HttpStatusCode.OK)]
        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetRolePermissions(long id)
        {
            var result = await _mediator.Send(new GetRolePermissionsQuery(id));
            return Ok(result);
        }

        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}/permissions")]
        public async Task<IActionResult> UpdateRolePermissions(long id, [FromBody] UpdateRolePermissionRequest request)
        {
            var result = await _mediator.Send(new UpdateRolePermissionsCommand(id, request.Permissions));
            return Ok(result);
        }

        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(SsoServiceResult<long>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("GetRoleByUser/{id}/{agreementId}")]
        public async Task<IActionResult> GetRoleByUser(long id, long? agreementId)
        {
            var result = await _mediator.Send(new GetRoleByUserQuery(id, agreementId));
            return Ok(result);
        }

    }
}
