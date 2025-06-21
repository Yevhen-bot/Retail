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
    public class CachedWorkerRepository : IUserRepository<Worker>
    {
        private readonly IUserRepository<Worker> _decorated;
        private readonly IDistributedCache _cache;
        private static string cachekey = "Worker-";

        public CachedWorkerRepository(IUserRepository<Worker> repository, IDistributedCache cache)
        {
            _cache = cache;
            _decorated = repository;
        }

        public async Task<Worker> GetByEmail(string email)
        {
            var cached = await _cache.GetStringAsync(cachekey + email);
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetByEmail(email);
                await _cache.SetStAsync(cachekey + email, obj);
                await _cache.SetStAsync(cachekey + obj.Id, obj);
                return obj;
            }

            return JsonSerializer.Deserialize<Worker>(cached);
        }

        public async Task<Worker> GetByIdWithTrack(int id)
        {
            return await _decorated.GetByIdWithTrack(id);
        }

        public async Task<List<Worker>> GetAll()
        {
            var cached = await _cache.GetStringAsync(cachekey + "s");
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetAll();
                await _cache.SetStAsync(cachekey + "s", obj);
                return obj;
            }

            return JsonSerializer.Deserialize<List<Worker>>(cached);
        }

        public async Task<Worker> GetById(int id)
        {
            var cached = await _cache.GetStringAsync(cachekey + id);
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetById(id);
                await _cache.SetStAsync(cachekey + id, obj);
                await _cache.SetStAsync(cachekey + obj.Email.EmailAddress, obj);
                return obj;
            }

            return JsonSerializer.Deserialize<Worker>(cached);
        }

        public async Task Add(Worker entity)
        {
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            await _cache.SetStAsync(cachekey + entity.Email.EmailAddress, entity);
            if (entity.Building != null)
            {
                await _cache.RemoveAsync(CacheOptions.GetCacheKey(entity.Building) + entity.Building.Id);
            }
            await _decorated.Add(entity);
        }

        public async Task Update(Worker entity)
        {
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            await _cache.SetStAsync(cachekey + entity.Email.EmailAddress, entity);
            if (entity.Building != null)
            {
                await _cache.RemoveAsync(CacheOptions.GetCacheKey(entity.Building) + entity.Building.Id);
            }
            await _decorated.Update(entity);
        }

        public async Task Delete(int id)
        {
            var obj = await _cache.GetStringAsync(cachekey + id);
            if (!string.IsNullOrEmpty(obj))
            {
                var entity = JsonSerializer.Deserialize<Worker>(obj);
                await _cache.RemoveAsync(cachekey + entity?.Email.EmailAddress);
                if(entity.Building != null)
                {
                    await _cache.RemoveAsync(CacheOptions.GetCacheKey(entity.Building) + entity.Building.Id);
                }
            }
            await _cache.RemoveAsync(cachekey + id);
            await _decorated.Delete(id);
        }
    }
}
