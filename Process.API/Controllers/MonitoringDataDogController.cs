using MediatR;
using Microsoft.AspNetCore.Mvc;
using Process.API.Response;
using Process.Application.MonitoringDataDog;


namespace Process.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitoringDataDogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet("GetMonitoringDataDogImg/{Type}")]
        public async Task<IActionResult> GetMonitoringDataDogImg(string Type)
        {
            var result = await _mediator.Send(new GetMonitoringDataDogImgQuery(Type));
            var response = ApiResponse<GetMonitoringDataDogImgResponse>.Success(result);
            return Ok(response);
        }
    }
}
