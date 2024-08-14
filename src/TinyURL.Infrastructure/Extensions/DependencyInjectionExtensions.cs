using Microsoft.Extensions.DependencyInjection;
using TinyURL.Domain.Urls;
using TinyURL.Infrastructure.Persistence;
using TinyURL.Infrastructure.Persistence.Repositories;

namespace TinyURL.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDataStorage<Guid, ShortenedUrl>, SemaphoreSlimInMemoryDataStorage<Guid, ShortenedUrl>>();
        services.AddTransient<IUrlRepository, UrlInMemoryRepository>();

        return services;
    }
}
