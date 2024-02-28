namespace CoffeeMachineAPI.Application
{
    public interface IWeatherService
    {
        Task<double> GetTemperatureByCityAsync(string city);
    }
}
