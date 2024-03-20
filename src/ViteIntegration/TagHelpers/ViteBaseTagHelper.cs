using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ViteIntegration.TagHelpers;

/// <summary>
/// Base class for Vite tag helpers
/// </summary>
public abstract class ViteBaseTagHelper : TagHelper
{
    private readonly IOptions<ViteConfiguration> configuration;
    private readonly IHostEnvironment hostEnvironment;

    /// <summary>
    /// DI constructor
    /// </summary>
    protected ViteBaseTagHelper(IOptions<ViteConfiguration> configuration, IHostEnvironment hostEnvironment)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    /// <summary>
    /// Lookup the Vite URL for the specified asset name.
    /// </summary>
    /// <param name="assetName">Asset name</param>
    /// <param name="url">Set to the correct URL to serve the asset</param>
    /// <returns>True if the asset was found and the URL was constructed, false otherwise</returns>
    protected bool TryGetViteUrl(string assetName, out string? url)
    {
        url = default;

        if (!configuration.Value.Assets.TryGetValue(assetName, out ViteAsset? asset)) return false;

        // Development mode: Use vite dev server
        if (hostEnvironment.IsDevelopment() && !string.IsNullOrEmpty(configuration.Value.ViteDevServerUrl))
        {
            Uri baseUri = new(configuration.Value.ViteDevServerUrl, UriKind.Absolute);
            Uri assetUri = new(baseUri, asset.Src);
            url = assetUri.ToString();
            return true;
        }

        url = asset.File;
        if (!url.StartsWith("/")) url = "/" + url;
        return true;
    }
}
