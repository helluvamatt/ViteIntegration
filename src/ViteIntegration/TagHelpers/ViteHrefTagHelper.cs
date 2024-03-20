using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ViteIntegration.TagHelpers;

/// <summary>
/// Tag helper for vite-href
/// </summary>
[HtmlTargetElement(Attributes = AttributeName)]
public class ViteHrefTagHelper : ViteBaseTagHelper
{
    private const string AttributeName = "vite-href";

    /// <inheritdoc />
    public ViteHrefTagHelper(IOptions<ViteConfiguration> configuration, IHostEnvironment hostEnvironment) : base(configuration, hostEnvironment) { }

    /// <summary>
    /// Vite asset URL
    /// </summary>
    [HtmlAttributeName(AttributeName), PathReference]
    public string ViteHref { get; set; } = default!;

    /// <inheritdoc />
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(AttributeName);
        if (!TryGetViteUrl(ViteHref, out string? url)) throw new InvalidOperationException("Asset not found: " + ViteHref);
        output.Attributes.SetAttribute("href", url);
    }
}
