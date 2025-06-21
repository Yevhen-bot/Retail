using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data_Access.Repos
{
    public static class CacheOptions
    {
        public static string GetCacheKey<T>(T entity) where T : class
        {
            return nameof(entity) + "-";
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        private static DistributedCacheEntryOptions GetCacheEntryOptions()
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
        }

        public static async Task SetStAsync<T>(this IDistributedCache cache, string key, T obj)
        {
            await cache.SetStringAsync(key, JsonSerializer.Serialize(obj, GetJsonSerializerOptions()), GetCacheEntryOptions());
        }
    }
}
