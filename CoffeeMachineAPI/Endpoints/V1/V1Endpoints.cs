namespace CoffeeMachineAPI.Endpoints.V1
{
    public static class V2Endpoints
    {
        public static void MapV1Endpoints(this WebApplication app)
        {
            app.MapGet("api/v1/brew-coffee", BrewCoffeeEndpoint.Handle)
                .WithTags("Brew Coffee v1")
                .WithName("BrewCoffeeV1")
                .Produces<BrewCoffeeResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status503ServiceUnavailable)
                .WithOpenApi();

        }
    }
}
