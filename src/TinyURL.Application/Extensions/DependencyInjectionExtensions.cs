using Microsoft.Extensions.DependencyInjection;
using TinyURL.Application.Urls;

namespace TinyURL.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ICodeGenerationService, CodeGenerationService>();
        
        return services;
    }
}
