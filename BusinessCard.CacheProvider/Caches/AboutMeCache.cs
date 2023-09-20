using BusinessCard.CacheProvider.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessCard.CacheProvider.Caches
{
    /// <summary>
    /// 
    /// </summary>
    public class AboutMeCache : IAboutMeCache
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache _cache;

        public AboutMeCache(IMemoryCache cache) => _cache = cache;
    }
}