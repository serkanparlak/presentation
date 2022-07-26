using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

public static class RedisCacheManager
{
    private static RedisCacheOptions _options = new RedisCacheOptions { Configuration = "localhost:6379" };

    public static T Get<T>(string cacheKey)
    {
        var redisCache = new RedisCache(_options);
        var valueString = redisCache.GetString(cacheKey);
        if (!string.IsNullOrEmpty(valueString))
        {
            var valueObject = JsonSerializer.Deserialize<T>(valueString);
            return (T)valueObject;
        }
        return default(T);
    }

    public static void Set<T>(string cacheKey, T model)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(90)
        };

        using (var redisCache = new RedisCache(_options))
        {
            var valueString = JsonSerializer.Serialize(model);
            redisCache.SetString(cacheKey, valueString);
        }
    }

    public static void Remove(string key)
    {
        using (var redisCache = new RedisCache(_options))
        {
            redisCache.Remove(key);
        }
    }
}