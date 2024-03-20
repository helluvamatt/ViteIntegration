namespace ViteIntegration.Internals;

internal class DefaultViteConfigurationBuilder : IViteConfigurationBuilder
{
    private readonly Dictionary<string, ViteAsset> assets = new();
    private readonly List<string> defaultAssets = [];
    private Uri? viteDevServerUrl;
    private ScriptMode scriptMode;
    private ServeFrom assetServeFrom = ServeFrom.Default;
    private ServeFrom? tagHelperServeFrom;

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
        viteDevServerUrl = new Uri(url, UriKind.Absolute);
        return this;
    }

    public IViteConfigurationBuilder WithScriptMode(ScriptMode configureScriptMode)
    {
        scriptMode = configureScriptMode;
        return this;
    }

    public IViteConfigurationBuilder WithAssetsServedFrom(ServeFrom serveFrom)
    {
        assetServeFrom = serveFrom;
        return this;
    }

    public IViteConfigurationBuilder WithTagHelperAssetsServedFrom(ServeFrom serveFrom)
    {
        tagHelperServeFrom = serveFrom;
        return this;
    }

    public void Build(ViteConfiguration configuration)
    {
        configuration.Assets = assets;
        configuration.DefaultAssets = defaultAssets.ToArray();
        configuration.ViteDevServerUrl = viteDevServerUrl;
        configuration.ScriptMode = scriptMode;
        configuration.AssetServeFrom = assetServeFrom;
        configuration.TagHelperServeFrom = tagHelperServeFrom;
    }
}
