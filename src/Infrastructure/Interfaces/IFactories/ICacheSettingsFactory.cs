using Infrastructure.Interfaces.ISettings;
using Infrastructure.Models.Constants;

namespace Infrastructure.Interfaces.IFactories
{
    public interface ICacheSettingsFactory
    {
        ICacheSettings GetCacheSettings(CacheTypes cacheType, string cacheEntry);
    }
}
