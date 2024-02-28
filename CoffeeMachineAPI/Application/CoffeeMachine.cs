
using Microsoft.Extensions.Caching.Memory;

namespace CoffeeMachineAPI.Application
{
    public record struct CoffeeMachineCacheEntry(Guid ID, int ServiceRequestCounts);

    public class CoffeeMachine(IMemoryCache memoryCache, IDateTimeProvider dateTimeProvider, int maxServiceRequestsCount = 5) : ICoffeeMachine
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly int _maxServiceRequestsCount = maxServiceRequestsCount;
        private readonly Guid _id = Guid.NewGuid();
       
        private string CacheKey => $"CoffeeMachine_{_id}";
        
        public Guid ID => _id;

        public int MaxServiceRequestsCounts => _maxServiceRequestsCount;

        public async virtual Task<BrewResult> BrewAsync()
        {
            var now = _dateTimeProvider.UTCNow.Date;
            if (now == _dateTimeProvider.Create(now.Year, 4, 1))
            {
                return await Task.FromResult(new NotBrewing());
            }
            if (_memoryCache.TryGetValue(CacheKey, out CoffeeMachineCacheEntry entry))
            {
                if (entry.ServiceRequestCounts >= _maxServiceRequestsCount)
                {
                    return await Task.FromResult(new OutOfCoffee());
                }
            }
            else
            {
                entry = new CoffeeMachineCacheEntry(_id, 0);
            }
            entry.ServiceRequestCounts += 1;
            _memoryCache.Set(CacheKey, entry);
            return await Task.FromResult(new CoffeeIsReady());

        }
    }
}
