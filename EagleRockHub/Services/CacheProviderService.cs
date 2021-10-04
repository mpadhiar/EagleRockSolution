using EagleRockHub.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Services
{
    public class CacheProviderService : ICacheProviderService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions CacheEntryOptions;

        public CacheProviderService(IDistributedCache cache)
        {
            _cache = cache;
            CacheEntryOptions = new DistributedCacheEntryOptions();
        }

        public async Task<T> GetFromCache<T>(string key) where T : class
        {
            var cachedResponse = await _cache.GetStringAsync(key);
            return cachedResponse == null ? null : JsonConvert.DeserializeObject<T>(cachedResponse);
        }

        public async Task SetCache<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
        {
            var response = JsonConvert.SerializeObject(value);
            await _cache.SetStringAsync(key, response, options);
        }

        public async Task AddItem<T>(string key, T value) where T : class
        {
            List<T> cacheItems = await GetFromCache<List<T>>(key);

            if(cacheItems == null)
            {
                cacheItems = new List<T>();
            }

            cacheItems.Add(value);
            await SetCache(key, cacheItems, CacheEntryOptions);
        }

        public async Task ClearCache(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
