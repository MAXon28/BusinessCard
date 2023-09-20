using BusinessCard.CacheProvider.Interfaces;
using BusinessCard.Entities.DTO.Service;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessCard.CacheProvider.Caches
{
    /// <inheritdoc cref="ITaskCache"/>
    public class TaskCache : ITaskCache
    {
        /// <summary>
        /// Кеш
        /// </summary>
        private readonly IMemoryCache _cache;

        public TaskCache(IMemoryCache cache) => _cache = cache;

        /// <inheritdoc/>
        public void SaveTaskDetail(string taskStringId, TaskDetail taskDetail)
        {
            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(30),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(7)
            };
            _cache.Set(taskStringId, taskDetail);
        }

        /// <inheritdoc/>
        public TaskDetail? GetTaskDetail(string taskStringId)
            => _cache.TryGetValue(taskStringId, out TaskDetail taskDetail) 
            ? taskDetail 
            : null;

        /// <inheritdoc/>
        public void DeleteTaskDetail(string taskStringId)
            => _cache?.Remove(taskStringId);
    }
}