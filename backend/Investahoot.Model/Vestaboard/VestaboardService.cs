using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Investahoot.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Investahoot.Model.Vestaboard
{
    public class VestaboardService
    {
        private readonly HttpClient _httpClient;
        private readonly string _subscriptionId;

        public VestaboardService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://platform.vestaboard.com/");
            _httpClient.DefaultRequestHeaders.Add("X-Vestaboard-Api-Key", configuration["VestaboardApi:Key"]);
            _httpClient.DefaultRequestHeaders.Add("X-Vestaboard-Api-Secret", configuration["VestaboardApi:Secret"]);

            _subscriptionId = configuration["VestaboardApi:SubscriptionId"];
        }

        public async Task SendTextMessage(VestaboardTextMessage textMessage)
        {
            var json = JsonSerializer.Serialize(textMessage, GetJsonOptions());
            var jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            using var httpResponseMessage = await _httpClient.PostAsync($"subscriptions/{_subscriptionId}/message", jsonContent);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task SendImageMessage(VestaboardCharacterMessage characterMessage)
        {
            var json = JsonSerializer.Serialize(characterMessage.Characters, GetJsonOptions());
            var jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            using var httpResponseMessage = await _httpClient.PostAsync($"subscriptions/{_subscriptionId}/message", jsonContent);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        private static JsonSerializerOptions GetJsonOptions() => 
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
                WriteIndented = true,
                Converters = { new Array2DConverter() }
            };
    }

    public record VestaboardTextMessage(string Text);
    public record VestaboardCharacterMessage(Image Characters);
}