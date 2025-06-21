using Core.Interfaces;
using Data_Access.Entities;
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
    public class CachedBuildingRepository : IRepository<Building>
    {
        private readonly IRepository<Building> _decorated;
        private readonly IDistributedCache _cache;
        private static string cachekey = "Building-";

        public CachedBuildingRepository(IRepository<Building> repository, IDistributedCache cache)
        {
            _decorated = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<List<Building>> GetAll()
        {
            var cachedBuildings = await _cache.GetStringAsync(cachekey + "s");
            if(string.IsNullOrEmpty(cachedBuildings))
            {
                var obj = await _decorated.GetAll();
                await _cache.SetStAsync(cachekey + "s", obj);
                return obj;
            }

            return JsonSerializer.Deserialize<List<Building>>(cachedBuildings);
        }

        public async Task<Building> GetById(int id)
        {
            var cachedBuildings = await _cache.GetStringAsync(cachekey + id);
            if (string.IsNullOrEmpty(cachedBuildings))
            {
                var obj = await _decorated.GetById(id);
                await _cache.SetStAsync(cachekey + id, obj);
                return obj;
            }

            return JsonSerializer.Deserialize<Building>(cachedBuildings);
        }

        public async Task Add(Building entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            foreach (var worker in entity.Workers)
            {
                await _cache.SetStAsync(cachekey + worker.Id, worker);
            }
            if(entity.Owner != null)
            {
                await _cache.SetStAsync(cachekey + entity.Owner.Id, entity.Owner);
            }
            await _decorated.Add(entity);
        }

        public async Task Update(Building entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            foreach (var worker in entity.Workers)
            {
                await _cache.SetStAsync(cachekey + worker.Id, worker);
            }
            if (entity.Owner != null)
            {
                await _cache.SetStAsync(cachekey + entity.Owner.Id, entity.Owner);
            }
            await _decorated.Update(entity);
        }

        public async Task Delete(int id)
        {
            await _cache.RemoveAsync(cachekey + id);
            var entity = await GetById(id);
            foreach (var worker in entity.Workers)
            {
                await _cache.SetStAsync(cachekey + worker.Id, worker);
            }
            if (entity.Owner != null)
            {
                await _cache.SetStAsync(cachekey + entity.Owner.Id, entity.Owner);
            }
            await _decorated.Delete(id);
        }
    }
}
