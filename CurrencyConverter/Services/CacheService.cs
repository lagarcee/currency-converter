using System.Text.Json;

namespace CurrencyConverter.Services;

public interface ICacheService
{
    Task<decimal?> GetCachedRate(string from, string to);
    Task SaveRates(string baseCurrency, Dictionary<string, decimal> rates);
}

public class MemoryCacheService : ICacheService
{
    private readonly Dictionary<string, (DateTime Expiry, Dictionary<string, decimal> Rates)> _cache = new();
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

    public Task<decimal?> GetCachedRate(string from, string to)
    {
        var cacheKey = $"{from}_rates";
        if (_cache.TryGetValue(cacheKey, out var cachedData) && cachedData.Expiry > DateTime.Now)
        {
            if (cachedData.Rates.TryGetValue(to, out var rate))
                return Task.FromResult<decimal?>(rate);
        }
        return Task.FromResult<decimal?>(null);
    }

    public Task SaveRates(string baseCurrency, Dictionary<string, decimal> rates)
    {
        var cacheKey = $"{baseCurrency}_rates";
        _cache[cacheKey] = (DateTime.Now.Add(_cacheDuration), rates);
        return Task.CompletedTask;
    }
}