using System.Diagnostics.CodeAnalysis;

namespace ViteIntegration;

/// <summary>
/// Represents a Vite manifest.json
/// </summary>
[ExcludeFromCodeCoverage]
public class ViteManifest : Dictionary<string, ViteAsset> { }
