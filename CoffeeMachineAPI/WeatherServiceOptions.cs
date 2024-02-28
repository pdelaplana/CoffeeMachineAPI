namespace CoffeeMachineAPI
{
    public record WeatherServiceOptions()
    {
        public string ApiKey { get; set; } = null!;
        public string City { get; set; } = null!;

    }
}
