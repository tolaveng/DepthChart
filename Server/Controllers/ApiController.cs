using Application.Models;
using Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IDepthChartService _depthChartService;
        private readonly IValidator<ChartRequest> _chartRequestValidator;
        private readonly IValidator<PlayerRequest> _playerValidator;
        private readonly IValidator<ChartRemoveRequest> _chartRemoveValidator;
        public ApiController(IDepthChartService depthChartService,
            IValidator<ChartRequest> chartRequestValidator,
            IValidator<PlayerRequest> playerValidator,
            IValidator<ChartRemoveRequest> chartRemoveValidator
            )
        {
            _depthChartService = depthChartService;
            _chartRequestValidator = chartRequestValidator;
            _playerValidator = playerValidator;
            _chartRemoveValidator = chartRemoveValidator;
        }

        
        /// <summary>
        /// Get Full List of Depth Chart in Plain Text
        /// </summary>
        /// <returns>Plain Text of Depth Chart</returns>
        [HttpGet("getFullDepthChart")]
        [Produces("text/plain")]
        public async Task<IActionResult> GetFullDepthChart()
        {
            var results = await _depthChartService.GetFullDepthChartAsync();
            return Ok(formatChart(results));
        }


        /// <summary>
        /// Get Full List of Depth Chart in JSON
        /// </summary>
        /// <returns>Json data of Depth Chart</returns>
        [HttpGet("getFullDepthChart/json")]
        public async Task<IActionResult> GetFullDepthChartJson()
        {
            var results = await _depthChartService.GetFullDepthChartAsync();
            return Ok(results.Select(ChartResponse.FromData));
        }

        [HttpGet("getBackups/{position}/{playerNumber:int}")]
        [Produces("text/plain")]
        public async Task<IActionResult> GetBackups(string position, int playerNumber)
        {
            if (string.IsNullOrWhiteSpace(position) || playerNumber <= 0)
            {
                return BadRequest("Position and Player Number are invalid");
            }
            var playerDto = new PlayerDto { Number = playerNumber };
            var results = await _depthChartService.GetBackupsAsync(position, playerDto);
            var text = new StringBuilder();
            foreach (var result in results)
            {
                text.AppendLine($"#{result.PlayerNumber} - {result.Player.Name}");
            }
            return Ok(text.ToString());
        }

        [HttpPost("getBackups")]
        public async Task<IActionResult> GetBackups([FromBody] ChartRequest request)
        {
            var playerDto = new PlayerDto { Number = request.Player.Number, Name = request.Player.Name };
            var results = await _depthChartService.GetBackupsAsync(request.Position, playerDto);
            return Ok(results.Select(ChartResponse.FromData));
        }

        [HttpPost("addPlayerToDepthChart")]
        public async Task<IActionResult> addPlayerToDepthChart([FromBody] ChartRequest request)
        {
            var validate = _chartRequestValidator.Validate(request);
            if (!validate.IsValid)
            {
                var errors = validate.Errors.Select(x => x.ErrorMessage);
                return BadRequest(errors);
            }
            var playerDto = new PlayerDto { Number = request.Player.Number, Name = request.Player.Name };
            var result = await _depthChartService.AddPlayerToDepthChartAsync(request.Position, playerDto, request.Depth);
            if (result) {
                return Ok("Player added");
            } else
            {
                return BadRequest("Cannot add player");
            }
        }


        [HttpDelete("removePlayerFromDepthChart/{position}/{playerNumber:int}")]
        public async Task<IActionResult> removePlayerFromDepthChart(string position, int playerNumber)
        {
            var request = new ChartRemoveRequest(position, playerNumber);
            var validate = _chartRemoveValidator.Validate(request);
            if (!validate.IsValid)
            {
                var errors = validate.Errors.Select(x => x.ErrorMessage);
                return BadRequest(errors);
            }
            var playerDto = new PlayerDto { Number = request.PlayerNumber };
            var result = await _depthChartService.RemovePlayerFromDepthChartAsync(request.Position, playerDto);
            if (result == null) return Ok("");
            return Ok($"#{result.Number} - {result.Name}");
        }


        private string formatChart(IEnumerable<ChartDto> results)
        {
            var groups = results.GroupBy(x => x.PositionId)
                .ToDictionary(x => x.Key, x => x.ToArray());

            var text = new StringBuilder();
            var groupName = "";
            foreach (var group in groups)
            {
                if (group.Key != groupName)
                {
                    text.AppendLine();
                    text.Append($"{group.Key.ToUpper()} -");
                    groupName = group.Key;
                }
                for (int i = 0; i < group.Value.Length; i++)
                {
                    var chart = group.Value[i];
                    text.Append($" (#{chart.PlayerNumber}, {chart.Player.Name})");
                    if (i < group.Value.Length - 1) text.Append(",");
                }
            }
            return text.ToString();
        }
    }
}
