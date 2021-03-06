﻿using System;

namespace Infrastructure.Core.Interfaces.ISettings
{
    public interface ICacheSettings
    {
        string KeyPrefix { get; set; }
        bool IsEnabled { get; set; }
        bool IsSerialized { get; set; }
        int CacheDuration { get; set; }
        DateTime ExpiryDate { get; set; }
    }
}
