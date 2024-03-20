using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ViteIntegration.TagHelpers;

/// <summary>
/// Tag helper for vite-src
/// </summary>
[HtmlTargetElement(Attributes = AttributeName)]
public class ViteSrcTagHelper : TagHelper
{
    private const string AttributeName = "vite-src";

    private readonly IViteAssetService viteAssetService;

    /// <inheritdoc />
    public ViteSrcTagHelper(IViteAssetService viteAssetService)
    {
        this.viteAssetService = viteAssetService ?? throw new ArgumentNullException(nameof(viteAssetService));
    }

    /// <summary>
    /// Vite asset URL
    /// </summary>
    [HtmlAttributeName(AttributeName), PathReference]
    public string ViteSrc { get; set; } = default!;

    /// <inheritdoc />
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(AttributeName);
        if (!viteAssetService.TryGetAsset(ViteSrc, out ViteAsset? asset)) throw new InvalidOperationException("Asset not found: " + ViteSrc);
        output.Attributes.SetAttribute("src", viteAssetService.GetAssetUrlForTagHelper(asset));
    }
}
