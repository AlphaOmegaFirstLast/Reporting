using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces.IFactories;
using Infrastructure.Interfaces.ISettings;
using Infrastructure.Models.Constants;
using Infrastructure.Settings;

namespace Infrastructure.Factories
{
    public class CacheSettingsFactory:ICacheSettingsFactory
    {
        public ICacheSettings GetCacheSettings(CacheTypes cacheType, string cacheEntry)
        {
           return new CacheSettings();
        }
    }
}
