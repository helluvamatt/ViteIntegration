using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ViteIntegration.TagHelpers;

/// <summary>
/// Tag helper for vite-src
/// </summary>
[HtmlTargetElement(Attributes = AttributeName)]
public class ViteSrcTagHelper : ViteBaseTagHelper
{
    private const string AttributeName = "vite-src";

    /// <inheritdoc />
    public ViteSrcTagHelper(IOptions<ViteConfiguration> configuration, IHostEnvironment hostEnvironment) : base(configuration, hostEnvironment) { }

    /// <summary>
    /// Vite asset URL
    /// </summary>
    [HtmlAttributeName(AttributeName), PathReference]
    public string ViteSrc { get; set; } = default!;

    /// <inheritdoc />
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(AttributeName);
        if (!TryGetViteUrl(ViteSrc, out string? url)) throw new InvalidOperationException("Asset not found: " + ViteSrc);
        output.Attributes.SetAttribute("src", url);
    }
}
