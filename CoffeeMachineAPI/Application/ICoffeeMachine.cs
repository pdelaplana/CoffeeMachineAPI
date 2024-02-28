
namespace CoffeeMachineAPI.Application
{
    public record BrewResult(string Message);
    public record CoffeeIsReady(): BrewResult("Your piping hot coffee is ready");
    public record IcedCoffeeIsReady() : BrewResult("Your refreshing iced coffee is ready");
    public record OutOfCoffee(): BrewResult("Out of Coffee");
    public record NotBrewing() : BrewResult("Not Brewing");
    
    public interface ICoffeeMachine
    {
        Guid ID { get; }
        int MaxServiceRequestsCounts { get; }
        Task<BrewResult> BrewAsync();
    }
}
