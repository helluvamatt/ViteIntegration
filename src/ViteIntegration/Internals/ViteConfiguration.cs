namespace ViteIntegration.Internals;

internal class ViteConfiguration
{
    public IReadOnlyDictionary<string, ViteAsset> Assets { get; set; } = new Dictionary<string, ViteAsset>();
    public string[] DefaultAssets { get; set; } = Array.Empty<string>();
    public Uri? ViteDevServerUrl { get; set; }
    public ScriptMode ScriptMode { get; set; } = ScriptMode.Module;
    public ServeFrom AssetServeFrom { get; set; } = ServeFrom.Default;
    public ServeFrom? TagHelperServeFrom { get; set; }
}
