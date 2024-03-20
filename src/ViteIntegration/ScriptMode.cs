namespace ViteIntegration;

/// <summary>
/// How should default script tags be rendered
/// </summary>
public enum ScriptMode
{
    /// <summary>
    /// Script tags will be rendered with the `type="module"` attribute
    /// </summary>
    /// <remarks>
    /// &lt;script type="module" src="..."&gt;&lt;/script&gt;
    /// </remarks>
    Module,
    /// <summary>
    /// Script tags will be rendered with the `type="module"` attribute with the `async` attribute
    /// </summary>
    /// <remarks>
    /// &lt;script type="module" async src="..."&gt;&lt;/script&gt;
    /// </remarks>
    ModuleAsync,
    /// <summary>
    /// Script tags will be rendered with the `type="text/javascript"` attribute and with the `defer` attribute
    /// </summary>
    /// <remarks>
    /// &lt;script type="text/javascript" defer src="..."&gt;&lt;/script&gt;
    /// </remarks>
    Defer,
    /// <summary>
    /// Script tags will be rendered with the `type="text/javascript"` attribute and with the `async` attribute
    /// </summary>
    /// <remarks>
    /// &lt;script type="text/javascript" async src="..."&gt;&lt;/script&gt;
    /// </remarks>
    Async,
    /// <summary>
    /// Script tags will be rendered with the `type="text/javascript"` attribute
    /// </summary>
    /// <remarks>
    /// &lt;script type="text/javascript" src="..."&gt;&lt;/script&gt;
    /// </remarks>
    Classic
}
