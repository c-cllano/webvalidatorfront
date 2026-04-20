using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.RoleByAgreement.Create;
using Process.Application.RoleByAgreement.Delete;
using Process.Application.RoleByAgreement.Dto;
using Process.Application.RoleByAgreement.GetAll;
using Process.Application.RoleByAgreement.GetByRole;
using Process.Application.RoleByAgreement.Update;
using Process.Application.RoleByAgreement.UpdateBatch;
using Process.Domain.Entities;
using System.Net;

namespace Process.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleByAgreementController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Obtiene todas las relaciones rol-convenio
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<List<RoleByAgreementDto>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllRoleByAgreementQuery());
            
            if (!result.Success)
                return BadRequest(ApiResponse<List<RoleByAgreementDto>>.Error(
                    result.Data ?? [], 
                    result.StatusCode ?? 400, 
                    result.Error ?? "Error"));

            return Ok(ApiResponse<List<RoleByAgreementDto>>.Success(result.Data!));
        }

        /// <summary>
        /// Obtiene los convenios asignados a un rol
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<List<RoleByAgreementDto>>), (int)HttpStatusCode.OK)]
        [HttpGet("role/{roleId:long}")]
        public async Task<IActionResult> GetByRole(long roleId)
        {
            var result = await _mediator.Send(new GetRoleByAgreementByRoleQuery(roleId));
            
            if (!result.Success)
                return BadRequest(ApiResponse<List<RoleByAgreementDto>>.Error(
                    result.Data ?? [], 
                    result.StatusCode ?? 400, 
                    result.Error ?? "Error"));

            return Ok(ApiResponse<List<RoleByAgreementDto>>.Success(result.Data!));
        }

        /// <summary>
        /// Crea una nueva relación rol-convenio
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleByAgreementRequest request)
        {
            var result = await _mediator.Send(new CreateRoleByAgreementCommand(
                request.RoleId, 
                request.AgreementId, 
                request.CreatorUserId));
            
            if (!result.Success)
                return BadRequest(ApiResponse<long>.Error(
                    0, 
                    result.StatusCode ?? 400, 
                    result.Error ?? "Error"));

            return CreatedAtAction(nameof(GetAll), ApiResponse<long>.Success(result.Data));
        }

        /// <summary>
        /// Actualiza el estado de una relación rol-convenio
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.NotFound)]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateRoleByAgreementRequest request)
        {
            var result = await _mediator.Send(new UpdateRoleByAgreementCommand(id, request.Active));
            
            if (!result.Success)
            {
                var statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, ApiResponse<long>.Error(
                    0, 
                    statusCode, 
                    result.Error ?? "Error"));
            }

            return Ok(ApiResponse<long>.Success(result.Data));
        }

        /// <summary>
        /// Elimina una relación rol-convenio (soft delete)
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.NotFound)]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _mediator.Send(new DeleteRoleByAgreementCommand(id));
            
            if (!result.Success)
            {
                var statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, ApiResponse<long>.Error(
                    0, 
                    statusCode, 
                    result.Error ?? "Error"));
            }

            return Ok(ApiResponse<long>.Success(result.Data));
        }

        /// <summary>
        /// Actualiza todos los convenios de un rol (batch update)
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("role/{roleId:long}/agreements")]
        public async Task<IActionResult> UpdateRoleAgreements(
            long roleId, 
            [FromBody] UpdateRoleAgreementsRequest request)
        {
            var result = await _mediator.Send(new UpdateRoleAgreementsCommand(
                roleId, 
                request.AgreementIds, 
                request.UpdaterUserId));
            
            if (!result.Success)
            {
                var statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, ApiResponse<bool>.Error(
                    false, 
                    statusCode, 
                    result.Error ?? "Error"));
            }

            return Ok(ApiResponse<bool>.Success(result.Data));
        }
    }
}
