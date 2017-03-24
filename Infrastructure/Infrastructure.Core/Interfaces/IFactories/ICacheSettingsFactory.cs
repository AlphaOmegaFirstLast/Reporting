using Infrastructure.Core.Interfaces.ISettings;
using Infrastructure.Models.Constants;

namespace Infrastructure.Core.Interfaces.IFactories
{
    public interface ICacheSettingsFactory
    {
        ICacheSettings GetCacheSettings(CacheTypes cacheType, string cacheEntry);
    }
}
