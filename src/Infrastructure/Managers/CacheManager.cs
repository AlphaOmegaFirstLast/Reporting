using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces.IManagers;
using Infrastructure.Interfaces.IServices;
using Infrastructure.Interfaces.ISettings;
using Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Managers
{
    /// <summary>
    /// Caching Service + Serialization 
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private readonly ILogger<CacheManager> _logger;
        private readonly ICacheService _cacheService;
        private readonly ICacheSettings _cacheSettings;
        private readonly ISerializationService _serializationService;

        public CacheManager(ILogger<CacheManager> logger,ICacheService cacheService,  ISerializationService serializationService)
        {
            _cacheService = cacheService;
            _cacheSettings = new CacheSettings();
            _serializationService = serializationService;
            _logger = logger;
        }

        public async Task<T> Get<T>(string key)
        {
            var result = default(T);
            try
            {
                dynamic cachedValue =  _cacheService.Get(key);

                if (cachedValue != null)
                    result = _cacheSettings.IsSerialized ? await _serializationService.Deserialize<T>(cachedValue) : cachedValue;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error CacheManager:{e.Message} : key: {key}: objectType: {typeof(T).ToString()}");
            }

            return result;
        }

        public async Task Set<T>(string key, T value)
        {
            if (value != null)
            {
                if (_cacheSettings.IsSerialized)
                {
                    var serializedValue = await _serializationService.Serialize(value);
                     _cacheService.Set(key, serializedValue);
                }
                else
                     _cacheService.Set(key, value.ToString());
            }
        }

        public void Remove(string key)
        {
             _cacheService.Remove(key);
        }
    }
}