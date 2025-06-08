using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CacheService
    {
        private IMemoryCache _cache;
        private const int SLIDEXPsec = 60;
        private const int ABSEXPsec = 3600;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool GetValue<T>(string cachename, out List<T> collection)
        {
            return _cache.TryGetValue(cachename, out collection);
        }

        public void SetValue<T>(string cachename, List<T> collection, int size)
        {
            var cacheentry = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(SLIDEXPsec))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(ABSEXPsec))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(size);

            _cache.Set(cachename, collection, cacheentry);
        }

        public void RemoveValue(string cachename)
        {
            _cache.Remove(cachename);
        }
    }
}
