using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RedisService
    {
        private readonly IDistributedCache _cache;

        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetValue<T>(string cachename)
        {
            var obj = await _cache.GetStringAsync(cachename);
            if (string.IsNullOrEmpty(obj))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(obj);
        }

        public async Task SetValue<T>(string cachename, T obj)
        {
            if(GetValue<T>(cachename) != null)
            {
                RemoveValue(cachename);
            }

            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(60),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            await _cache.SetStringAsync(cachename, JsonSerializer.Serialize(obj), options);
        }

        public async Task RemoveValue(string cachename)
        {
            await _cache.RemoveAsync(cachename);
        }
    }
}
