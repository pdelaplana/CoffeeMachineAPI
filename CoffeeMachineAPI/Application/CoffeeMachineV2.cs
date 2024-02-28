using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CoffeeMachineAPI.Application
{
    public class CoffeeMachineV2(
        IWeatherService weatherService, 
        IMemoryCache memoryCache, 
        IDateTimeProvider dateTimeProvider, 
        IOptions<WeatherServiceOptions> options,
        int maxServiceRequestsCount = 5) 
        : CoffeeMachine(memoryCache, dateTimeProvider, maxServiceRequestsCount)
    {
        private readonly IWeatherService _weatherService = weatherService;
        private readonly IOptions<WeatherServiceOptions> _options = options;

        public async override Task<BrewResult> BrewAsync()
        {
            var result = await base.BrewAsync();
            if (result is CoffeeIsReady)
            {
                try
                {
                    double temperature = await _weatherService.GetTemperatureByCityAsync(_options.Value.City);
                    if (temperature > 30)
                    {
                        return new IcedCoffeeIsReady();
                    }
                }
                catch
                {
                    // log error
                }
            }
            return result;
        }
    }
}
