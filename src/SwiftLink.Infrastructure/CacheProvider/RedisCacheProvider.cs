using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;

namespace SwiftLink.Infrastructure.CacheProvider;

public class RedisCacheService(IDistributedCache cache,
                               IOptions<AppSettings> options,
                               ResiliencePipelineProvider<string> resiliencePipeline)
    : ICacheProvider
{
    private readonly IDistributedCache _cache = cache;
    private readonly ResiliencePipelineProvider<string> _resiliencePipeline = resiliencePipeline;
    private readonly AppSettings _options = options.Value;

    #region Not Important
    public async Task<bool> Remove(string key)
    {
        var pipeline = _resiliencePipeline.GetPipeline<bool>(nameof(RedisCashServiceResiliencyKey.SetOrRemoveCircuitBreaker));

        var outcome = await pipeline.ExecuteOutcomeAsync(async (ctx, state) =>
        {
            await _cache.RemoveAsync(key);
            return Outcome.FromResult(true);

        }, ResilienceContextPool.Shared.Get(), "state");

        return outcome.Exception is not BrokenCircuitException && outcome.Result;
    }

    public async Task<bool> Set(string key, string value)
        => await Set(key, value, DateTime.Now.AddDays(_options.DefaultExpirationTimeInDays));

    public async Task<bool> Set(string key, string value, DateTime expirationDate)
    {
        var pipeline = _resiliencePipeline.GetPipeline<bool>(
          nameof(RedisCashServiceResiliencyKey.SetOrRemoveCircuitBreaker));

        var outcome = await pipeline.ExecuteOutcomeAsync(async (ctx, state) =>
        {
            DistributedCacheEntryOptions cacheEntryOptions = new()
            {
                SlidingExpiration = TimeSpan.FromHours(_options.Redis.SlidingExpirationHour),
                AbsoluteExpiration = expirationDate,
            };
            var dataToCache = Encoding.UTF8.GetBytes(value);
            await _cache.SetAsync(key, dataToCache, cacheEntryOptions);
            return Outcome.FromResult(true);

        }, ResilienceContextPool.Shared.Get(), "state");

        return outcome.Exception is not BrokenCircuitException && outcome.Result;
    }

    #endregion

    public async Task<string> Get(string key)
    {

        // return await _cache.GetStringAsync(key);
        var pipeline = _resiliencePipeline.GetPipeline<string>(
            nameof(RedisCashServiceResiliencyKey.GetCircuitBreaker));

        var outcome = await pipeline.ExecuteOutcomeAsync(async (ctx, state) =>
        {
            return Outcome.FromResult(await _cache.GetStringAsync(key));
        }, ResilienceContextPool.Shared.Get(), "state");

        return outcome.Exception is BrokenCircuitException ? null : outcome.Result;

    }
}

public enum RedisCashServiceResiliencyKey
{
    SetOrRemoveCircuitBreaker,
    GetCircuitBreaker
}
