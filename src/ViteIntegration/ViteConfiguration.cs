namespace ViteIntegration;

/// <summary>
/// Represents Vite configuration.
/// </summary>
public class ViteConfiguration
{
    /// <summary>
    /// Vite assets from the manifest
    /// </summary>
    public IReadOnlyDictionary<string, ViteAsset> Assets { get; set; } = new Dictionary<string, ViteAsset>();

    /// <summary>
    /// Default assets to include on every page
    /// </summary>
    public string[] DefaultAssets { get; set; } = Array.Empty<string>();

    /// <summary>
    /// If configured, in Development mode, use this Vite dev server
    /// </summary>
    public string? ViteDevServerUrl { get; set; }

    /// <summary>
    /// Should script tags be type module?
    /// </summary>
    public bool IsScriptModule { get; set; } = true;

    /// <summary>
    /// Should script tags include the defer attribute?
    /// </summary>
    public bool IsScriptDefer { get; set; } = true;
}
