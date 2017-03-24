using Infrastructure.Core.Interfaces.IFactories;
using Infrastructure.Core.Interfaces.ISettings;
using Infrastructure.Core.Settings;
using Infrastructure.Models.Constants;

namespace Infrastructure.Core.Factories
{
    public class CacheSettingsFactory:ICacheSettingsFactory
    {
        public ICacheSettings GetCacheSettings(CacheTypes cacheType, string cacheEntry)
        {
           return new CacheSettings();
        }
    }
}
