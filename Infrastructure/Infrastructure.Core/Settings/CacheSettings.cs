using System;
using Infrastructure.Core.Interfaces.ISettings;

namespace Infrastructure.Core.Settings
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
