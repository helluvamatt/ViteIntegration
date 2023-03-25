using Microsoft.AspNetCore.Mvc.Rendering;

namespace ViteIntegration;

/// <summary>
/// Contains methods that extend <see cref="ViewContext"/>.
/// </summary>
public static class ViewContextExtensions
{
    internal const string ContextKey = "__ViteIntegration_AssetNames";

    /// <summary>
    /// Add the specified asset names to the <see cref="ViewContext"/> so that they are included in the page.
    /// </summary>
    /// <param name="viewContext"><see cref="ViewContext"/></param>
    /// <param name="assetNames">Asset names</param>
    public static void RequireViteAssets(this ViewContext viewContext, params string[] assetNames)
    {
        if (!viewContext.ViewData.ContainsKey(ContextKey)) viewContext.ViewData[ContextKey] = new List<string>();
        if (viewContext.ViewData[ContextKey] is List<string> assetNamesSet) assetNamesSet.AddRange(assetNames);
    }

    internal static IEnumerable<string> GetAssetNames(this ViewContext viewContext)
    {
        return viewContext.ViewData[ContextKey] as List<string> ?? Enumerable.Empty<string>();
    }
}
