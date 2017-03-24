using System;
using Infrastructure.Core.Interfaces.IServices;
using Infrastructure.Core.Interfaces.ISettings;
using Infrastructure.Core.Settings;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Core.Services
{
    public class CacheService: ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheSettings _cacheSettings;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheSettings = new CacheSettings();
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public void Set(string key, string value)
        {
            var cacheOptions = new MemoryCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_cacheSettings.CacheDuration) };
            _memoryCache.Set(key, value, cacheOptions);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
