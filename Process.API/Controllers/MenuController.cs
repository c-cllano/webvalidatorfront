using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Attributes;
using Process.Application.Menus;
using Process.Application.Menus.GetActualRoleBySideMenu;
using Process.Application.Menus.GetAll;
using Process.Application.Menus.GetByAgreementAndUser;
using Process.Application.Menus.GetByRole;
using Process.Application.Menus.GetNamesByRole;
using Process.Application.Menus.GetWithPermissions;
using Process.Domain.Entities;
using System.Net;

namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        /// <summary>
        /// Obtiene todos los menús
        /// </summary>
        [ProducesResponseType(typeof(SsoServiceResult<List<MenuResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {
            var result = await _mediator.Send(new GetAllMenusQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtiene los menús de un rol específico
        /// </summary>
        [ProducesResponseType(typeof(SsoServiceResult<List<MenuResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("ByRole/{roleId:long}")]
        public async Task<IActionResult> GetMenuByRole(long roleId)
        {
            var result = await _mediator.Send(new GetMenusByRoleQuery(roleId));
            return Ok(result);
        }


        /// <summary>
        /// Obtiene los menús del usuario con su rol seleccionado
        /// </summary>
        [ProducesResponseType(typeof(SsoServiceResult<List<MenuResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("ByAgreementAndUser/{agreementId:long}/{userId:long}")]
        public async Task<IActionResult> GetMenuByAgreementAndUser(long agreementId, long userId)
        {
            var result = await _mediator.Send(new GetMenusByAgreementAndUserQuery(agreementId, userId));
            return Ok(result);
        }


        /// <summary>
        /// Obtiene menús con sus permisos, marcando los seleccionados para un rol (opcional)
        /// </summary>
        [ProducesResponseType(typeof(SsoServiceResult<List<Process.Application.Menus.GetWithPermissions.MenuWithPermissionsResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("Permissions")]
        public async Task<IActionResult> GetMenusWithPermissions([FromQuery] long? roleId = null)
        {
            var result = await _mediator.Send(new GetMenusWithPermissionsQuery(roleId));
            return Ok(result);
        }

        /// <summary>
        /// Obtiene solo los nombres (titles) de los menús asignados a un rol
        /// </summary>
        [ProducesResponseType(typeof(SsoServiceResult<List<string>>), (int)HttpStatusCode.OK)]
        [HttpGet("NamesByRole/{roleId:long}")]
        public async Task<IActionResult> GetMenuNamesByRole(long roleId)
        {
            var result = await _mediator.Send(new GetMenuNamesByRoleQuery(roleId));
            return Ok(result);
        }

        /// <summary>
        /// Obtiene solo los nombres (titles) de los menús asignados a un rol
        /// </summary>
        [ProducesResponseType(typeof(SsoServiceResult<List<string>>), (int)HttpStatusCode.OK)]
        [HttpGet("ActualRoleBySideMenu/{userId:long}/{AgreementId:long}")]
        public async Task<IActionResult> GetActualRoleBySideMenu(long userId , long AgreementId)
        {
            var result = await _mediator.Send(new GetActualRoleBySideMenuQuery(userId, AgreementId));
            return Ok(result);
        }
    }
}
