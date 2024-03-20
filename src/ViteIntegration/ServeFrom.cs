namespace ViteIntegration;

/// <summary>
/// Where to serve assets
/// </summary>
public enum ServeFrom
{
    /// <summary>
    /// The default, which is to serve assets from the Vite dev server in Development mode, and from the Vite build output in Production mode
    /// </summary>
    Default,
    /// <summary>
    /// Always serve assets from the Vite build output (wwwroot)
    /// </summary>
    StaticFiles,
    /// <summary>
    /// Always serve assets from the Vite dev server
    /// </summary>
    ViteDevServer,
}
