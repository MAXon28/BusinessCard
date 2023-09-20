using BusinessCard.CacheProvider.Caches;
using BusinessCard.CacheProvider.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessCard.CacheProvider
{
    /// <summary>
    /// Расширение для добавления сервисов работы с кэшем приложения
    /// </summary>
    public static class CacheProviderExtensions
    {
        /// <summary>
        /// Добавить сервисы для работы с кэшем приложения
        /// </summary>
        /// <param name="services"> Сервисы </param>
        /// <returns> Сервисы </returns>
        public static IServiceCollection AddCacheProviderServices(this IServiceCollection services)
            => services
                .AddMemoryCache()
                .AddScoped<IAboutMeCache, AboutMeCache>()
                .AddScoped<ITaskCache, TaskCache>();
    }
}