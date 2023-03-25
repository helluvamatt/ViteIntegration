namespace ViteIntegration;

/// <summary>
/// Represents a Vite asset.
/// </summary>
public class ViteAsset
{
    /// <summary>
    /// Relative file path
    /// </summary>
    public string File { get; set; } = default!;

    /// <summary>
    /// Source file path
    /// </summary>
    public string Src { get; set; } = default!;

    /// <summary>
    /// Is the asset an entry point?
    /// </summary>
    public bool IsEntry { get; set; }

    /// <summary>
    /// Is the asset a dynamic entry point?
    /// </summary>
    public bool IsDynamicEntry { get; set; }

    /// <summary>
    /// Static chunk dependencies
    /// </summary>
    public string[]? Imports { get; set; }

    /// <summary>
    /// Dynamic chunk dependencies
    /// </summary>
    public string[]? DynamicImports { get; set; }

    /// <summary>
    /// Static asset dependencies
    /// </summary>
    public string[]? Assets { get; set; }

    /// <summary>
    /// CSS asset dependencies
    /// </summary>
    public string[]? Css { get; set; }
}
