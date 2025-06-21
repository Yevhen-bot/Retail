using Core.Interfaces;
using Data_Access.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data_Access.Repos
{
    public class CachedOrderRepository : IRepository<Order>
    {
        private readonly IRepository<Order> _decorated;
        private readonly IDistributedCache _cache;
        private static string cachekey = "Order-";

        public CachedOrderRepository(IRepository<Order> repository, IDistributedCache cache)
        {
            _decorated = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task Add(Order entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            await _decorated.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _cache.RemoveAsync(cachekey + id);
            await _decorated.Delete(id);
        }

        public async Task<List<Order>> GetAll()
        {
            var cached = await _cache.GetStringAsync(cachekey + "s");
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetAll();
                await _cache.SetStAsync(cachekey + "s", obj);
                return obj;
            }

            return JsonSerializer.Deserialize<List<Order>>(cached);
        }

        public async Task<Order> GetById(int id)
        {
            var cachedBuildings = await _cache.GetStringAsync(cachekey + id);
            if (string.IsNullOrEmpty(cachedBuildings))
            {
                var obj = await _decorated.GetById(id);
                await _cache.SetStAsync(cachekey + id, obj);
                return obj;
            }

            return JsonSerializer.Deserialize<Order>(cachedBuildings);
        }

        public async Task Update(Order entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            await _decorated.Update(entity);
        }
    }
}
