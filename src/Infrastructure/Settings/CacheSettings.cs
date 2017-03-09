using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces.ISettings;

namespace Infrastructure.Settings
{
    public class CacheSettings:ICacheSettings
    {
        public string KeyPrefix { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSerialized { get; set; }
        public int CacheDuration { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
