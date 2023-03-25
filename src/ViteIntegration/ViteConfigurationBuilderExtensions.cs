using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ViteIntegration;

/// <summary>
/// Extension methods for <see cref="IViteConfigurationBuilder"/>.
/// </summary>
public static class ViteConfigurationBuilderExtensions
{
    /// <summary>
    /// Add assets from a manifest file
    /// </summary>
    /// <param name="builder"><see cref="IViteConfigurationBuilder"/></param>
    /// <param name="manifestPath">Path to Vite manifest.json</param>
    /// <returns>The <see cref="IViteConfigurationBuilder"/>, for chaining</returns>
    [ExcludeFromCodeCoverage] // Excluded from code coverage because this is just a convenience method
    public static IViteConfigurationBuilder AddAssetsFromManifest(this IViteConfigurationBuilder builder, string manifestPath)
    {
        string manifestJson = File.ReadAllText(manifestPath);
        return builder.AddAssetsFromManifestJson(manifestJson);
    }

    internal static IViteConfigurationBuilder AddAssetsFromManifestJson(this IViteConfigurationBuilder builder, string manifestJson, string? manifestPath = null)
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        ViteManifest manifest = JsonSerializer.Deserialize<ViteManifest>(manifestJson, options) ?? throw new InvalidOperationException($"Failed to deserialize manifest \"{manifestPath ?? "__TEST_DATA__"}\"");
        return builder.AddAssetsFromManifest(manifest);
    }

    /// <summary>
    /// Add assets from a manifest
    /// </summary>
    /// <param name="builder"><see cref="IViteConfigurationBuilder"/></param>
    /// <param name="manifest">Manifest</param>
    /// <returns>The <see cref="IViteConfigurationBuilder"/>, for chaining</returns>
    public static IViteConfigurationBuilder AddAssetsFromManifest(this IViteConfigurationBuilder builder, ViteManifest manifest)
    {
        foreach ((string name, ViteAsset asset) in manifest)
        {
            builder.AddAsset(name, asset);
        }
        return builder;
    }
}
