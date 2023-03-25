namespace ViteIntegration;

/// <summary>
/// Vite configuration builder
/// </summary>
public interface IViteConfigurationBuilder
{
    /// <summary>
    /// Add an asset to the configuration
    /// </summary>
    /// <param name="name">Name of the asset</param>
    /// <param name="asset">Vite asset</param>
    /// <returns>This configuration, for chaining</returns>
    IViteConfigurationBuilder AddAsset(string name, ViteAsset asset);

    /// <summary>
    /// Include the specified assets by default on every page
    /// </summary>
    /// <param name="defaultAssetNames">Names of assets to include by default</param>
    /// <returns>This configuration, for chaining</returns>
    IViteConfigurationBuilder WithDefaultAssets(params string[] defaultAssetNames);

    /// <summary>
    /// In Development mode, use the Vite dev server
    /// </summary>
    /// <param name="url">Vite URL</param>
    /// <returns>This configuration, for chaining</returns>
    IViteConfigurationBuilder WithDevServer(string url);

    /// <summary>
    /// Configure options for &lt;script&gt; tags
    /// </summary>
    /// <param name="isModule">True to set type="module", false to set type="text/javascript"</param>
    /// <param name="isDefer">True to include the "defer" attribute</param>
    /// <returns>This configuration, for chaining</returns>
    IViteConfigurationBuilder WithScriptOptions(bool isModule = true, bool isDefer = true);
}
