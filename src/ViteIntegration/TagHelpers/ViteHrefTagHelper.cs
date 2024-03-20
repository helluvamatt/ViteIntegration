using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ViteIntegration.TagHelpers;

/// <summary>
/// Tag helper for vite-href
/// </summary>
[HtmlTargetElement(Attributes = AttributeName)]
public class ViteHrefTagHelper : TagHelper
{
    private const string AttributeName = "vite-href";

    private readonly IViteAssetService viteAssetService;

    /// <inheritdoc />
    public ViteHrefTagHelper(IViteAssetService viteAssetService)
    {
        this.viteAssetService = viteAssetService ?? throw new ArgumentNullException(nameof(viteAssetService));
    }

    /// <summary>
    /// Vite asset URL
    /// </summary>
    [HtmlAttributeName(AttributeName), PathReference]
    public string ViteHref { get; set; } = default!;

    /// <inheritdoc />
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(AttributeName);
        if (!viteAssetService.TryGetAsset(ViteHref, out ViteAsset? asset)) throw new InvalidOperationException("Asset not found: " + ViteHref);
        output.Attributes.SetAttribute("href", viteAssetService.GetAssetUrlForTagHelper(asset));
    }
}
