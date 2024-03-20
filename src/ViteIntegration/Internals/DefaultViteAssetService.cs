using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ViteIntegration.Internals;

// This service reads configuration once at creation for performance reasons
internal sealed class DefaultViteAssetService : IViteAssetService
{
    private readonly IReadOnlyDictionary<string, ViteAsset> viteAssets;
    private readonly Func<ViteAsset, string> assetToUrl;
    private readonly Func<ViteAsset, string> assetToTagHelperUrl;
    private readonly Uri? viteClientUri;

    public DefaultViteAssetService(IOptions<ViteConfiguration> configuration, IHostEnvironment hostEnvironment)
    {
        viteAssets = configuration.Value.Assets;
        assetToUrl = BuildAssetToUrl(hostEnvironment, configuration.Value.AssetServeFrom, configuration.Value.ViteDevServerUrl);
        assetToTagHelperUrl = BuildAssetToUrl(hostEnvironment, configuration.Value.TagHelperServeFrom ?? configuration.Value.AssetServeFrom, configuration.Value.ViteDevServerUrl);
        viteClientUri = configuration.Value.ViteDevServerUrl is not null ? new Uri(configuration.Value.ViteDevServerUrl, "/@vite/client") : null;
        DefaultAssets = configuration.Value.DefaultAssets;
        UseViteDevServer = configuration.Value.ViteDevServerUrl is not null && IsUseViteDevServer(hostEnvironment, configuration.Value.AssetServeFrom);
        ScriptMode = configuration.Value.ScriptMode;
    }

    public IEnumerable<string> DefaultAssets { get; }
    public bool UseViteDevServer { get; }
    public ScriptMode ScriptMode { get; }
    public Uri ViteClientUri => viteClientUri ?? throw new InvalidOperationException("Vite dev server URL is not configured.");

    public bool TryGetAsset(string assetName, [NotNullWhen(true)] out ViteAsset? viteAsset)
        => viteAssets.TryGetValue(assetName, out viteAsset);

    public string GetAssetUrl(ViteAsset viteAsset)
        => assetToUrl(viteAsset);

    public string GetAssetUrlForTagHelper(ViteAsset viteAsset)
        => assetToTagHelperUrl(viteAsset);

    private static Func<ViteAsset, string> BuildAssetToUrl(IHostEnvironment hostEnvironment, ServeFrom serveFrom, Uri? viteDevServerBaseUrl)
    {
        // Development mode: Use vite dev server
        if (viteDevServerBaseUrl is not null && IsUseViteDevServer(hostEnvironment, serveFrom))
        {
            return asset =>
            {
                Uri assetUri = new(viteDevServerBaseUrl, asset.Src);
                return assetUri.ToString();
            };
        }

        // StaticFiles mode: Make sure URL path is rooted
        return asset =>
        {
            string url = asset.File;
            if (!url.StartsWith('/')) url = "/" + url;
            return url;
        };
    }

    private static bool IsUseViteDevServer(IHostEnvironment hostEnvironment, ServeFrom serveFrom)
    {
        return serveFrom switch
        {
            // Always use Vite dev server
            ServeFrom.ViteDevServer => true,
            // Default, only use Vite dev server in development environment
            ServeFrom.Default => hostEnvironment.IsDevelopment(),
            // Otherwise, do not use Vite dev server
            _ => false
        };
    }
}
