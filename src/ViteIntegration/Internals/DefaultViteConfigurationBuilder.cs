using System.Text.Json;

namespace ViteIntegration.Internals;

internal class DefaultViteConfigurationBuilder : IViteConfigurationBuilder
{
    private readonly Dictionary<string, ViteAsset> assets = new();
    private readonly List<string> defaultAssets = new();
    private string? viteDevServerUrl;
    private bool isScriptModule = true;
    private bool isScriptDefer = true;

    public IViteConfigurationBuilder AddAsset(string name, ViteAsset asset)
    {
        assets.Add(name, asset);
        return this;
    }

    public IViteConfigurationBuilder WithDefaultAssets(params string[] defaultAssetNames)
    {
        defaultAssets.AddRange(defaultAssetNames);
        return this;
    }

    public IViteConfigurationBuilder WithDevServer(string url)
    {
        viteDevServerUrl = url;
        return this;
    }

    public IViteConfigurationBuilder WithScriptOptions(bool isModule = true, bool isDefer = true)
    {
        isScriptModule = isModule;
        isScriptDefer = isDefer;
        return this;
    }

    public void Build(ViteConfiguration configuration)
    {
        configuration.Assets = assets;
        configuration.DefaultAssets = defaultAssets.ToArray();
        configuration.ViteDevServerUrl = viteDevServerUrl;
        configuration.IsScriptModule = isScriptModule;
        configuration.IsScriptDefer = isScriptDefer;
    }
}
