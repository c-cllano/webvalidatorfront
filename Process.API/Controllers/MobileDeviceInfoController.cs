using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.DeviceInfo.CreateUpdateMobileDeviceInfo;
using Process.Application.DeviceInfo.GetMobileDeviceInfo;

namespace Process.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileDeviceInfoController(
        IMediator mediator
    ) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{model}/{os}/{osVersion}/{browser}")]
        public async Task<IActionResult> GetMobileDeviceInfoByModelAsync(
            string model,
            string os,
            string osVersion,
            string browser
        )
        {
            var result = await _mediator.Send(new GetMobileDeviceInfoQuery(model, os, osVersion, browser));
            var response = ApiResponse<GetMobileDeviceInfoResponse>.Success(result);
            return Ok(response);
        }

        [HttpPost("create-update")]
        public async Task<IActionResult> CreateUpdateMobileDeviceInfoAsync(
            [FromBody] CreateUpdateMobileDeviceInfoCommand command
        )
        {
            var result = await _mediator.Send(command);
            var response = ApiResponse<GetMobileDeviceInfoResponse>.Success(result);
            return Ok(response);
        }
    }
}
