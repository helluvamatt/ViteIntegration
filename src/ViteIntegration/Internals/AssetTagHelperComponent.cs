using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ViteIntegration.Internals;

internal class AssetTagHelperComponent(IViteAssetService viteAssetService) : TagHelperComponent
{
    private const string ContextKey = "__ViteIntegration_Assets";
    private const string HtmlTagBody = "body";
    private const string HtmlTagHead = "head";

    private readonly IViteAssetService viteAssetService = viteAssetService ?? throw new ArgumentNullException(nameof(viteAssetService));

    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    #region ITagHelperComponent

    [ExcludeFromCodeCoverage]
    public override int Order => 1;

    public override void Init(TagHelperContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        // Validate assets and add them to the context
        HashSet<string> assetNames = [];
        assetNames.UnionWith(viteAssetService.DefaultAssets);
        assetNames.UnionWith(ViewContext.GetAssetNames());
        List<ViteAsset> assets = [];
        foreach (string assetName in assetNames)
        {
            if (!viteAssetService.TryGetAsset(assetName, out ViteAsset? asset)) throw new InvalidOperationException("Asset not found: " + assetName);
            assets.Add(asset);
        }
        context.Items[ContextKey] = assets;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);
        string tagName = context.TagName.ToLowerInvariant();
        if (tagName != HtmlTagHead && tagName != HtmlTagBody) return;
        if (!context.Items.TryGetValue(ContextKey, out object? assetsObj) || assetsObj is not List<ViteAsset> assets) throw new InvalidOperationException("Assets not found in context");
        switch (context.TagName.ToLowerInvariant())
        {
            case HtmlTagHead:
                ProcessHead(output, assets);
                break;
            case HtmlTagBody:
                ProcessBody(output, assets);
                break;
        }
    }

    #endregion

    internal void ProcessHead(TagHelperOutput output, IEnumerable<ViteAsset> assets)
    {
        if (viteAssetService.UseViteDevServer) return;
        foreach (ViteAsset asset in assets)
        {
            if (asset.Css is null) continue;
            foreach (string cssFile in asset.Css)
            {
                string cssUrl = cssFile.StartsWith('/') ? cssFile : $"/{cssFile}";
                output.PostContent.AppendHtml($"<link rel=\"stylesheet\" href=\"{cssUrl}\">");
            }
        }
    }

    internal void ProcessBody(TagHelperOutput output, IEnumerable<ViteAsset> assets)
    {
        if (viteAssetService.UseViteDevServer)
        {
            // Development mode: Use vite dev server
            // Write @vite/client
            output.PostContent.AppendHtml($"<script type=\"module\" src=\"{viteAssetService.ViteClientUri}\"></script>");
        }

        // Write required assets
        string scriptType = viteAssetService.ScriptMode switch
        {
            ScriptMode.Module => "module",
            ScriptMode.ModuleAsync => "module",
            ScriptMode.Defer => "text/javascript",
            ScriptMode.Async => "text/javascript",
            ScriptMode.Classic => "text/javascript",
            _ => throw new InvalidOperationException("Unknown ScriptMode: " + viteAssetService.ScriptMode)
        };
        string scriptAttrs = viteAssetService.ScriptMode switch
        {
            ScriptMode.ModuleAsync => " async",
            ScriptMode.Defer => " defer",
            ScriptMode.Async => " async",
            _ => ""
        };
        foreach (ViteAsset asset in assets)
        {
            output.PostContent.AppendHtml($"<script type=\"{scriptType}\"{scriptAttrs} src=\"{viteAssetService.GetAssetUrl(asset)}\"></script>");
        }
    }
}
