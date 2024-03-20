using System.Diagnostics.CodeAnalysis;

namespace ViteIntegration;

/// <summary>
/// Represents a service which can resolve Vite assets
/// </summary>
public interface IViteAssetService
{
    /// <summary>
    /// Default assets to include in the page
    /// </summary>
    IEnumerable<string> DefaultAssets { get; }

    /// <summary>
    /// Whether or not to use the Vite dev server for assets required through <see cref="ViewContextExtensions.RequireViteAssets"/> and <see cref="IViteConfigurationBuilder.WithDefaultAssets"/>
    /// </summary>
    bool UseViteDevServer { get; }

    /// <summary>
    /// Configured <see cref="ViteIntegration.ScriptMode"/>
    /// </summary>
    ScriptMode ScriptMode { get; }

    /// <summary>
    /// URI for the @vite/client module on the vite dev server
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this is called and <see cref="UseViteDevServer"/> is false</exception>
    Uri ViteClientUri { get; }

    /// <summary>
    /// Lookup the Vite asset for the specified asset name.
    /// </summary>
    /// <param name="assetName">Asset source</param>
    /// <param name="viteAsset"><see cref="ViteAsset"/></param>
    /// <returns>True if the asset was found, false otherwise</returns>
    bool TryGetAsset(string assetName, [NotNullWhen(true)] out ViteAsset? viteAsset);

    /// <summary>
    /// Get the URL to serve the specified asset.
    /// </summary>
    /// <param name="viteAsset">The <see cref="ViteAsset"/></param>
    /// <returns>The URL to use to serve the asset</returns>
    string GetAssetUrl(ViteAsset viteAsset);

    /// <summary>
    /// Get the URL to serve the specified asset for use in the tag helpers.
    /// </summary>
    /// <param name="viteAsset">The <see cref="ViteAsset"/></param>
    /// <returns>The URL to use to serve the asset in a tag helper</returns>
    string GetAssetUrlForTagHelper(ViteAsset viteAsset);
}
