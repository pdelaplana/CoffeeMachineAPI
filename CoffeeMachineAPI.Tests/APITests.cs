using CoffeeMachineAPI.Application;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace CoffeeMachineAPI.Tests
{

    public class APITests
    {
        private readonly Mock<ICoffeeMachine> _mockCoffeeMachine = new();

        public async Task<WebApplicationFactory<Program>> SetupWebApplication()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton<ICoffeeMachine>(sp => _mockCoffeeMachine.Object);
                    })
                );
            return application;
        }

        [Fact]
        public async void API_BrewCoffeeEndpoint_ShouldReturn200_WhenInvoked() 
        {
            // arrange
            _mockCoffeeMachine.Setup(m => m.BrewAsync()).ReturnsAsync(new CoffeeIsReady());

            var application = await SetupWebApplication();
            using var client = application.CreateClient();
            
            // act
            var response = await client.GetAsync("api/v1/brew-coffee");

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async void API_BrewCoffeeEndpoint_ShouldReturn503_WhenInvokedAndCoffeeMachineIsOutOfCoffee()
        {
            // arrange
            _mockCoffeeMachine.Setup(m => m.BrewAsync()).ReturnsAsync(new OutOfCoffee());

            var application = await SetupWebApplication();
            using var client = application.CreateClient();

            // act
            var response = await client.GetAsync("api/v1/brew-coffee");

            // assert
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);

        }

        [Fact]
        public async void API_BrewCoffeeEndpoint_ShouldReturn418_WhenInvokedAndCoffeeMachineIsNotBrewing()
        {
            // arrange
            _mockCoffeeMachine.Setup(m => m.BrewAsync()).ReturnsAsync(new NotBrewing());

            var application = await SetupWebApplication();
            using var client = application.CreateClient();

            // act
            var response = await client.GetAsync("api/v1/brew-coffee");

            // assert
            Assert.Equal(StatusCodes.Status418ImATeapot.ToString(), response.StatusCode.ToString());


        }

    }
}
