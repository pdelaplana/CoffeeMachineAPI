using CoffeeMachineAPI.Application;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace CoffeeMachineAPI.Tests
{
    public class CoffeeMachineTests
    {
        
        private readonly Mock<IMemoryCache> _mockMemoryCache = new();
        private readonly Mock<ICacheEntry> _mockCacheEntry = new();
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();

        public CoffeeMachineTests() 
        {
            object? value = new CoffeeMachineCacheEntry(It.IsAny<Guid>(), 0);
            
            _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out value)).Returns(true);
            _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>())).Returns(_mockCacheEntry.Object);

            _mockDateTimeProvider.SetupGet(provider => provider.UTCNow).Returns(DateTime.UtcNow);
            _mockDateTimeProvider.Setup(provider => provider.Create(DateTime.UtcNow.Year, 4, 1)).Returns(new DateTime(DateTime.UtcNow.Year, 4, 1));

        }

        [Fact]
        public async void CoffeeMachine_Brew_ShouldReturnCoffeeIsReady_WhenInvoked()
        {
            // arrange
            

            // act
            ICoffeeMachine coffeeMachine = new CoffeeMachine(_mockMemoryCache.Object, _mockDateTimeProvider.Object, 5);
            var result = await coffeeMachine.BrewAsync();

            // assert
            Assert.IsType<CoffeeIsReady>(result);
        }

        [Fact]
        public async void CoffeeMachine_Brew_ShouldReturnOutOfCoffee_WhenInvokedMoreThanMaxServiceRequestCount()
        {
            // arrange
            var maxServiceRequestCount = 5;
            var currentServiceRequestCount = 5;

            object? value = new CoffeeMachineCacheEntry(It.IsAny<Guid>(), currentServiceRequestCount);
            _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out value)).Returns(true);

            // act
            ICoffeeMachine coffeeMachine = new CoffeeMachine(_mockMemoryCache.Object, _mockDateTimeProvider.Object, maxServiceRequestCount);
            var result = await coffeeMachine.BrewAsync();

            // assert
            Assert.IsType<OutOfCoffee>(result);
        }

        [Fact]
        public async void CoffeeMachine_Brew_ShouldReturnNotBreweing_WhenInvokedOnApril1()
        {
            // arrange
            _mockDateTimeProvider.SetupGet(provider => provider.UTCNow).Returns(new DateTime(DateTime.UtcNow.Year, 4, 1));

            // act
            ICoffeeMachine coffeeMachine = new CoffeeMachine(_mockMemoryCache.Object, _mockDateTimeProvider.Object);
            var result = await coffeeMachine.BrewAsync();

            // assert
            Assert.IsType<NotBrewing>(result);
        }

        
    }
}