using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using ViteIntegration.Internals;

namespace ViteIntegration.Tests;

public class AssetTagHelperComponentTests
{
    [Test]
    public void Test_ProcessHead()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration());
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperOutput output = new("head", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        ViteAsset asset = new() { File = "foo.js", Css = ["site.css"] };

        // Act
        assetTagHelperComponent.ProcessHead(output, new []{ asset });

        // Assert
        output.PostContent.IsModified.Should().BeTrue();
        StringWriter stringWriter = new();
        output.WriteTo(stringWriter, new HtmlTestEncoder());
        stringWriter.ToString().Should().Be("<head><link rel=\"stylesheet\" href=\"/site.css\"></head>");
    }

    [Test]
    public void Test_ProcessHead_DevMode()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration { ViteDevServerUrl = new Uri("http://localhost:3000") });
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperOutput output = new("head", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        ViteAsset asset = new() { File = "foo.js", Css = ["site.css"] };

        // Act
        assetTagHelperComponent.ProcessHead(output, new[] { asset });

        // Assert
        // DevMode does not add any CSS since CSS is injected by @vite/client
        output.PostContent.IsModified.Should().BeFalse();
    }

    [Test]
    public void Test_ProcessHead_AssetHasNoCss()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration());
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperOutput output = new("head", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        ViteAsset asset = new() { File = "foo.js" };

        // Act
        assetTagHelperComponent.ProcessHead(output, new []{ asset });

        // Assert
        output.PostContent.IsModified.Should().BeFalse();
    }

    [Test]
    public void Test_ProcessBody()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration());
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperOutput output = new("body", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        ViteAsset asset = new() { File = "foo.js", Css = ["site.css", "button.css"] };

        // Act
        assetTagHelperComponent.ProcessBody(output, new[] { asset });

        // Assert
        output.PostContent.IsModified.Should().BeTrue();
        StringWriter stringWriter = new();
        output.WriteTo(stringWriter, new HtmlTestEncoder());
        stringWriter.ToString().Should().Be("<body><script type=\"module\" src=\"/foo.js\"></script></body>");
    }

    [Test]
    public void Test_ProcessBody_ScriptModeDefer()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration { ScriptMode = ScriptMode.Defer });
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperOutput output = new("body", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        ViteAsset asset = new() { File = "foo.js", Css = ["site.css", "button.css"] };

        // Act
        assetTagHelperComponent.ProcessBody(output, new[] { asset });

        // Assert
        output.PostContent.IsModified.Should().BeTrue();
        StringWriter stringWriter = new();
        output.WriteTo(stringWriter, new HtmlTestEncoder());
        stringWriter.ToString().Should().Be("<body><script type=\"text/javascript\" defer src=\"/foo.js\"></script></body>");
    }

    [Test]
    public void Test_ProcessBody_ScriptModeAsync()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration { ScriptMode = ScriptMode.Async });
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperOutput output = new("body", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        ViteAsset asset = new() { File = "foo.js", Css = ["site.css", "button.css"] };

        // Act
        assetTagHelperComponent.ProcessBody(output, new[] { asset });

        // Assert
        output.PostContent.IsModified.Should().BeTrue();
        StringWriter stringWriter = new();
        output.WriteTo(stringWriter, new HtmlTestEncoder());
        stringWriter.ToString().Should().Be("<body><script type=\"text/javascript\" async src=\"/foo.js\"></script></body>");
    }

    [Test]
    public void Test_ProcessBody_DevMode()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration { ViteDevServerUrl = new Uri("http://localhost:3000") });
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperOutput output = new("body", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        ViteAsset asset = new() { Src = "foo", File = "foo.js", Css = ["site.css", "button.css"] };

        // Act
        assetTagHelperComponent.ProcessBody(output, new[] { asset });

        // Assert
        output.PostContent.IsModified.Should().BeTrue();
        StringWriter stringWriter = new();
        output.WriteTo(stringWriter, new HtmlTestEncoder());
        stringWriter.ToString().Should().Be("<body><script type=\"module\" src=\"http://localhost:3000/@vite/client\"></script><script type=\"module\" src=\"http://localhost:3000/foo\"></script></body>");
    }

    [Test]
    public void Test_Init_RequiredAssetNotExistsThrows()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("bar");
        Dictionary<string, ViteAsset> assets = new()
        {
            { "foo", new ViteAsset { File = "foo.js" } }
        };
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration { Assets = assets });
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperContext context = new("body", [], new Dictionary<object, object>(), Guid.NewGuid().ToString());

        // Act
        Action actual = () => assetTagHelperComponent.Init(context);

        // Assert
        actual.Should().Throw<InvalidOperationException>().WithMessage("Asset not found: bar");
    }

    [Test]
    public void Test_Init_NullContextThrows()
    {
        // Arrange
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(new ViteConfiguration());
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };

        // Act
        Action actual = () => assetTagHelperComponent.Init(null!);

        // Assert
        actual.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Test_Process_ForHead()
    {
        // Arrange
        ViteConfiguration configuration = new()
        {
            Assets = new Dictionary<string, ViteAsset>
            {
                { "foo", new ViteAsset { File = "foo.js", Css = ["site.css"] } }
            }
        };
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(configuration);
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperContext context = new("head", [], new Dictionary<object, object>(), Guid.NewGuid().ToString());
        TagHelperOutput output = new("head", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        assetTagHelperComponent.Init(context);

        // Act
        assetTagHelperComponent.Process(context, output);

        // Assert
        output.PostContent.IsModified.Should().BeTrue();
        StringWriter stringWriter = new();
        output.WriteTo(stringWriter, new HtmlTestEncoder());
        stringWriter.ToString().Should().Be("<head><link rel=\"stylesheet\" href=\"/site.css\"></head>");
    }

    [Test]
    public void Test_Process_ForBody()
    {
        // Arrange
        ViteConfiguration configuration = new()
        {
            Assets = new Dictionary<string, ViteAsset>
            {
                { "foo", new ViteAsset { File = "foo.js", Css = ["site.css"] } }
            }
        };
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(configuration);
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperContext context = new("body", [], new Dictionary<object, object>(), Guid.NewGuid().ToString());
        TagHelperOutput output = new("body", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        assetTagHelperComponent.Init(context);

        // Act
        assetTagHelperComponent.Process(context, output);

        // Assert
        output.PostContent.IsModified.Should().BeTrue();
        StringWriter stringWriter = new();
        output.WriteTo(stringWriter, new HtmlTestEncoder());
        stringWriter.ToString().Should().Be("<body><script type=\"module\" src=\"/foo.js\"></script></body>");
    }

    [Test]
    public void Test_Process_NullOutputThrows()
    {
        // Arrange
        ViteConfiguration configuration = new()
        {
            Assets = new Dictionary<string, ViteAsset>
            {
                { "foo", new ViteAsset { File = "foo.js", Css = ["site.css"] } }
            }
        };
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(configuration);
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperContext context = new("body", [], new Dictionary<object, object>(), Guid.NewGuid().ToString());
        assetTagHelperComponent.Init(context);

        // Act
        Action action = () => assetTagHelperComponent.Process(context, null!);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'output')");
    }

    [Test]
    public void Test_Process_InitNotCalledThrows()
    {
        // Arrange
        ViteConfiguration configuration = new()
        {
            Assets = new Dictionary<string, ViteAsset>
            {
                { "foo", new ViteAsset { File = "foo.js", Css = ["site.css"] } }
            }
        };
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(configuration);
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperContext context = new("body", [], new Dictionary<object, object>(), Guid.NewGuid().ToString());
        TagHelperOutput output = new("body", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));

        // Act
        Action action = () => assetTagHelperComponent.Process(context, output);

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("Assets not found in context");
    }

    [Test]
    public void Test_Process_NullContextThrows()
    {
        // Arrange
        ViteConfiguration configuration = new()
        {
            Assets = new Dictionary<string, ViteAsset>
            {
                { "foo", new ViteAsset { File = "foo.js", Css = ["site.css"] } }
            }
        };
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(configuration);
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperContext context = new("body", [], new Dictionary<object, object>(), Guid.NewGuid().ToString());
        TagHelperOutput output = new("body", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        assetTagHelperComponent.Init(context);

        // Act
        Action action = () => assetTagHelperComponent.Process(null!, output);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public void Test_Process_TagNotHeadOrBodyDoesNothing()
    {
        // Arrange
        ViteConfiguration configuration = new()
        {
            Assets = new Dictionary<string, ViteAsset>
            {
                { "foo", new ViteAsset { File = "foo.js", Css = ["site.css"] } }
            }
        };
        IHostEnvironment dev = new HostingEnvironment { EnvironmentName = Environments.Development };
        ViewContext viewContext = new();
        viewContext.RequireViteAssets("foo");
        IOptions<ViteConfiguration> options = new OptionsWrapper<ViteConfiguration>(configuration);
        IViteAssetService viteAssetService = new DefaultViteAssetService(options, dev);
        AssetTagHelperComponent assetTagHelperComponent = new(viteAssetService)
        {
            ViewContext = viewContext
        };
        TagHelperContext context = new("table", [], new Dictionary<object, object>(), Guid.NewGuid().ToString());
        TagHelperOutput output = new("table", [], [ExcludeFromCodeCoverage] (_, _) => Task.FromResult((TagHelperContent)new DefaultTagHelperContent()));
        assetTagHelperComponent.Init(context);

        // Act
        assetTagHelperComponent.Process(context, output);

        // Assert
        output.PostContent.IsModified.Should().BeFalse();
        StringWriter stringWriter = new();
        output.WriteTo(stringWriter, new HtmlTestEncoder());
        stringWriter.ToString().Should().Be("<table></table>");
    }
}
