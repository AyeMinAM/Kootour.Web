using System;

namespace DevTrends.MvcDonutCaching
{
    public interface IExtendedOutputCacheManager : IOutputCacheManager
    {
        void AddItem(string key, CacheItem cacheItem, DateTime utcExpiry);
        CacheItem GetItem(string key);
    }
}
