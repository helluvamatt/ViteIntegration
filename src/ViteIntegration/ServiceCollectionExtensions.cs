using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using ViteIntegration.Internals;

namespace ViteIntegration;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Vite integration services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configurationAction">Optional configuration callback</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining</returns>
    public static IServiceCollection AddViteIntegration(this IServiceCollection services, Action<IViteConfigurationBuilder>? configurationAction = null)
    {
        DefaultViteConfigurationBuilder configurationBuilder = new();
        configurationAction?.Invoke(configurationBuilder);
        services.AddOptions<ViteConfiguration>().Configure(configurationBuilder.Build);
        services.AddTransient<ITagHelperComponent, AssetTagHelperComponent>();
        return services;
    }
}
