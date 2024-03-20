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
    /// <param name="scriptMode"><see cref="ScriptMode"/></param>
    /// <returns>This configuration, for chaining</returns>
    IViteConfigurationBuilder WithScriptMode(ScriptMode scriptMode);

    /// <summary>
    /// How to serve assets referenced by <see cref="ViewContextExtensions.RequireViteAssets"/> and <see cref="IViteConfigurationBuilder.WithDefaultAssets"/>
    /// </summary>
    /// <param name="serveFrom"><see cref="ServeFrom"/></param>
    /// <returns>This configuration, for chaining</returns>
    IViteConfigurationBuilder WithAssetsServedFrom(ServeFrom serveFrom);

    /// <summary>
    /// How to serve assets referenced by tag helpers
    /// </summary>
    /// <param name="serveFrom"><see cref="ServeFrom"/></param>
    /// <returns>This configuration, for chaining</returns>
    IViteConfigurationBuilder WithTagHelperAssetsServedFrom(ServeFrom serveFrom);
}
