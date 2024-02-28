using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoffeeMachineAPI.Application
{
    public struct TemperatureStruct
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }
    }

    public record WeatherData([property: JsonPropertyName("main")]TemperatureStruct Main);

    public class WeatherService(IHttpClientFactory httpClientFactory, IOptions<WeatherServiceOptions> options) : IWeatherService
    {
        private const string baseWeatherServiceUrl = "https://api.openweathermap.org/data/2.5/weather";
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IOptions<WeatherServiceOptions> _options = options;
        public async Task<double> GetTemperatureByCityAsync(string city)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{baseWeatherServiceUrl}?q={city}&units=metric&APPID={_options.Value.ApiKey}");

                response.EnsureSuccessStatusCode();
                var weatherData = JsonSerializer.Deserialize<WeatherData>(await response.Content.ReadAsStringAsync());

                return weatherData != null ? weatherData.Main.Temp : 0;
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Weather Service Unavailable");
            }
        }
    }
}
