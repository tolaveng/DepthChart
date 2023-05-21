using Client.Models;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Client.Models.ChartRequest;

namespace Client
{
    public class DepthChart : IDepthChart, IDisposable
    {
        private readonly HttpClient _httpClient;

        public DepthChart(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddPlayerToDepthChart(string position, Player player, int? depth)
        {
            var request = new ChartRequest(position, player, depth);
            new ChartRequestValidator().ValidateAndThrow(request);

            //var json = JsonConvert.SerializeObject(request);
            //var content = new StringContent(json);
            var response = await _httpClient.PostAsJsonAsync("/api/addPlayerToDepthChart", request);
            response.EnsureSuccessStatusCode();
            return true;
        }


        public async Task<string> GetBackups(string position, Player player)
        {
            new PlayerValidatorWithoutName().ValidateAndThrow(player);
            var response = await _httpClient.GetAsync($"/api/getBackups/{position}/{player.Number}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetFullDepthChart()
        {
            var response = await _httpClient.GetAsync("/api/getFullDepthChart");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RemovePlayerFromDepthChart(string position, Player player)
        {
            var request = new ChartRemoveRequest(position, player.Number);
            new ChartRemoveValidator().ValidateAndThrow(request);

            var response = await _httpClient.DeleteAsync($"/api/removePlayerFromDepthChart/{position}/{player.Number}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            if (_httpClient != null) _httpClient.Dispose();
        }

    }
}
