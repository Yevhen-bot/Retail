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
    public class CachedClientRepository : IUserRepository<Client>
    {
        private readonly IUserRepository<Client> _decorated;
        private readonly IDistributedCache _cache;
        private static string cachekey = "Client-";

        public CachedClientRepository(IUserRepository<Client> userRepository, IDistributedCache cache)
        {
            _cache = cache;
            _decorated = userRepository;
        }

        public async Task<Client> GetByEmail(string email)
        {
            var cached = await _cache.GetStringAsync(cachekey + email);
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetByEmail(email);
                await _cache.SetStringAsync(cachekey + email, JsonSerializer.Serialize(obj));
                await _cache.SetStringAsync(cachekey + obj.Id, JsonSerializer.Serialize(obj));
                return obj;
            }

            return JsonSerializer.Deserialize<Client>(cached);
        }

        public async Task<Client> GetByIdWithTrack(int id)
        {
            return await _decorated.GetByIdWithTrack(id);
        }

        public async Task<List<Client>> GetAll()
        {
            var cached = await _cache.GetStringAsync(cachekey + "s");
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetAll();
                await _cache.SetStringAsync(cachekey + "s", JsonSerializer.Serialize(obj));
                return obj;
            }

            return JsonSerializer.Deserialize<List<Client>>(cached);
        }

        public async Task<Client> GetById(int id)
        {
            var cached = await _cache.GetStringAsync(cachekey + id);
            if (string.IsNullOrEmpty(cached))
            {
                var obj = await _decorated.GetById(id);
                await _cache.SetStringAsync(cachekey + id, JsonSerializer.Serialize(obj));
                await _cache.SetStringAsync(cachekey + obj.Email.EmailAddress, JsonSerializer.Serialize(obj));
                return obj;
            }

            return JsonSerializer.Deserialize<Client>(cached);
        }

        public async Task Add(Client entity)
        {
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            await _cache.SetStAsync(cachekey + entity.Email.EmailAddress, entity);
            await _decorated.Add(entity);
        }

        public async Task Update(Client entity)
        {
            await _cache.SetStAsync(cachekey + entity.Id, entity);
            await _cache.SetStAsync(cachekey + entity.Email.EmailAddress, entity);
            foreach(var order in entity.Orders)
            {
                await _cache.SetStAsync(CacheOptions.GetCacheKey(order) + order.Id, order);
            }
            await _decorated.Update(entity);
        }

        public async Task Delete(int id)
        {
            var obj = await _cache.GetStringAsync(cachekey + id);
            if (!string.IsNullOrEmpty(obj))
            {
                var entity = JsonSerializer.Deserialize<Client>(obj);
                await _cache.RemoveAsync(cachekey + entity?.Email.EmailAddress);
                foreach (var order in entity.Orders)
                {
                    await _cache.RemoveAsync(CacheOptions.GetCacheKey(order) + order.Id);
                }
            }
            await _cache.RemoveAsync(cachekey + id);
            await _decorated.Delete(id);
        }
    }
}
