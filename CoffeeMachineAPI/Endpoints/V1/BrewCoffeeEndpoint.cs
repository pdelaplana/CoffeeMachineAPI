using CoffeeMachineAPI.Application;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CoffeeMachineAPI.Endpoints.V1;

public class BrewCoffeeEndpoint
{
    public static async Task<Results<Ok<BrewCoffeeResponse>, StatusCodeHttpResult>> Handle(ICoffeeMachine coffeeMachine)
    {

        var brewResult = await coffeeMachine.BrewAsync();

        switch (brewResult)
        {
            case CoffeeIsReady:
                var prepared = DateTime.Now;
                return TypedResults.Ok(
                    new BrewCoffeeResponse(
                        brewResult.Message, 
                        prepared.ToString("yyyy-MM-ddTHH:mm:ss") + prepared.ToString("zzz").Replace(":","")));
            case OutOfCoffee:
                return TypedResults.StatusCode(StatusCodes.Status503ServiceUnavailable);
            case NotBrewing:
                return TypedResults.StatusCode((StatusCodes.Status418ImATeapot));
            default: throw new NotImplementedException();
        }
            
    }
}

public record BrewCoffeeResponse(string Message, string Prepared);


