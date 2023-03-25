using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ViteIntegration.Internals;

internal class AssetTagHelperComponent : TagHelperComponent
{
    private const string ContextKey = "__ViteIntegration_Assets";
    private const string HtmlTagBody = "body";
    private const string HtmlTagHead = "head";

    private readonly IOptions<ViteConfiguration> configuration;
    private readonly IHostEnvironment hostEnvironment;

    public AssetTagHelperComponent(IOptions<ViteConfiguration> configuration, IHostEnvironment hostEnvironment)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    #region ITagHelperComponent

    [ExcludeFromCodeCoverage]
    public override int Order => 1;

    public override void Init(TagHelperContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        // Validate assets and add them to the context
        HashSet<string> assetNames = new();
        assetNames.UnionWith(configuration.Value.DefaultAssets);
        assetNames.UnionWith(ViewContext.GetAssetNames());
        List<ViteAsset> assets = new();
        foreach (string assetName in assetNames)
        {
            if (!configuration.Value.Assets.TryGetValue(assetName, out ViteAsset? asset)) throw new InvalidOperationException("Asset not found: " + assetName);
            assets.Add(asset);
        }
        context.Items[ContextKey] = assets;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (output is null) throw new ArgumentNullException(nameof(output));
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
        if (hostEnvironment.IsDevelopment() && !string.IsNullOrEmpty(configuration.Value.ViteDevServerUrl)) return;
        foreach (ViteAsset asset in assets)
        {
            if (asset.Css is null) continue;
            foreach (string cssFile in asset.Css)
            {
                string cssUrl = cssFile.StartsWith("/") ? cssFile : $"/{cssFile}";
                output.PostContent.AppendHtml($"<link rel=\"stylesheet\" href=\"{cssUrl}\">");
            }
        }
    }

    internal void ProcessBody(TagHelperOutput output, IEnumerable<ViteAsset> assets)
    {
        if (hostEnvironment.IsDevelopment() && !string.IsNullOrEmpty(configuration.Value.ViteDevServerUrl))
        {
            // Development mode: Use vite dev server
            Uri baseUri = new(configuration.Value.ViteDevServerUrl, UriKind.Absolute);

            // Write @vite/client
            Uri viteClientUri = new(baseUri, "/@vite/client");
            output.PostContent.AppendHtml($"<script type=\"module\" src=\"{viteClientUri}\"></script>");

            // Write required assets
            foreach (ViteAsset asset in assets)
            {
                Uri assetUri = new(baseUri, asset.Src);
                output.PostContent.AppendHtml($"<script type=\"module\" src=\"{assetUri}\"></script>");
            }

            return;
        }

        // Production mode: Use bundled assets from manifest
        foreach (ViteAsset asset in assets)
        {
            string assetUrl = asset.File;
            if (!assetUrl.StartsWith("/")) assetUrl = $"/{assetUrl}";
            output.PostContent.AppendHtml($"<script type=\"text/javascript\" src=\"{assetUrl}\"></script>");
        }
    }
}
