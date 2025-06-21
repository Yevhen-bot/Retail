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
    public class CachedOwnerRepository : IUserRepository<Owner>
    {
        private readonly IUserRepository<Owner> _decorated;
        private readonly IDistributedCache _cache;
        private static string cachekey = "Owner-";

        public CachedOwnerRepository(IUserRepository<Owner> repository, IDistributedCache cache)
        {
            _cache = cache;
            _decorated = repository;
        }

        public async Task<Owner> GetByEmail(string email)
        {
            var cached = await _cache.GetStringAsync(cachekey + email);
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetByEmail(email);
                await _cache.SetStAsync(cachekey + email, obj);
                await _cache.SetStAsync(cachekey + obj.Id, obj);
                return obj;
            }

            return JsonSerializer.Deserialize<Owner>(cached);
        }

        public async Task<Owner> GetByIdWithTrack(int id)
        {
            return await _decorated.GetByIdWithTrack(id);
        }

        public async Task<List<Owner>> GetAll()
        {
            var cached = await _cache.GetStringAsync(cachekey + "s");
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetAll();
                await _cache.SetStAsync(cachekey + "s", obj);
                return obj;
            }

            return JsonSerializer.Deserialize<List<Owner>>(cached);
        }

        public async Task<Owner> GetById(int id)
        {
            var cached = await _cache.GetStringAsync(cachekey + id);
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetById(id);
                await _cache.SetStAsync(cachekey + id, obj);
                await _cache.SetStAsync(cachekey + obj.Email.EmailAddress, obj);
                return obj;
            }

            return JsonSerializer.Deserialize<Owner>(cached);
        }

        public async Task Add(Owner entity)
        {
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            await _cache.SetStAsync(cachekey + entity.Email.EmailAddress, entity);
            await _decorated.Add(entity);
        }

        public async Task Update(Owner entity)
        {
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            await _cache.SetStAsync(cachekey + entity.Email.EmailAddress, entity);
            foreach (var building in entity.Buildings)
            {
                await _cache.SetStringAsync(CacheOptions.GetCacheKey(building) + building.Id, JsonSerializer.Serialize(building));
            }
            await _decorated.Update(entity);
        }

        public async Task Delete(int id)
        {
            var obj = await _cache.GetStringAsync(cachekey + id);
            if (!string.IsNullOrEmpty(obj))
            {
                var entity = JsonSerializer.Deserialize<Owner>(obj);
                await _cache.RemoveAsync(cachekey + entity?.Email.EmailAddress);
                foreach (var building in entity.Buildings)
                {
                    await _cache.RemoveAsync(CacheOptions.GetCacheKey(building) + building.Id);
                }
            }
            await _cache.RemoveAsync(cachekey + id);
            await _decorated.Delete(id);
        }
    }
}
