using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IDepthChartService _depthChartService;

        public ApiController(IDepthChartService depthChartService)
        {
            _depthChartService = depthChartService;
        }

        [HttpGet("healthcheck")]
        public IActionResult HealthCheck()
        {
            return Ok("Ok");
        }

        [HttpGet("getFullDepthChart")]
        public async Task<IActionResult> GetFullDepthChart()
        {
            var results = await _depthChartService.GetFullDepthChartAsync();
            return Ok(results);
        }

        [HttpGet("getBackups")]
        public async Task<IActionResult> GetBackups([FromQuery] string position, [FromQuery] int playerNumber)
        {
            if (string.IsNullOrWhiteSpace(position) || playerNumber == 0)
            {
                return BadRequest("Position and Player Number are required");
            }
            var playerDto = new PlayerDto { Number = playerNumber };
            var results = await _depthChartService.GetBackupsAsync(position, playerDto);
            return Ok(results);
        }

        [HttpPost("getBackups")]
        public async Task<IActionResult> GetBackups([FromBody] BackupRequest request)
        {
            var playerDto = new PlayerDto { Number = request.Player.Number, Name = request.Player.Name };
            var results = await _depthChartService.GetBackupsAsync(request.Position, playerDto);
            return Ok(results);
        }

        [HttpPost("addPlayerToDepthChart")]
        public async Task<IActionResult> addPlayerToDepthChart([FromBody] BackupRequest request)
        {
            var playerDto = new PlayerDto { Number = request.Player.Number, Name = request.Player.Name };
            var result = await _depthChartService.AddPlayerToDepthChartAsync(request.Position, playerDto, request.Depth);
            if (result) {
                return Ok("Player added");
            } else
            {
                return BadRequest("Cannot add player");
            }
        }
    }
}
