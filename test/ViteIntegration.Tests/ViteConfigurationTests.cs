using ViteIntegration.Internals;

namespace ViteIntegration.Tests;

public class ViteConfigurationTests
{
    [Test]
    public void Test_Defaults()
    {
        ViteConfiguration configuration = new();
        configuration.Assets.Should().BeEmpty();
        configuration.DefaultAssets.Should().BeEmpty();
        configuration.ViteDevServerUrl.Should().BeNull();
        configuration.IsScriptModule.Should().BeTrue();
        configuration.IsScriptDefer.Should().BeTrue();
    }

    [Test]
    public void Test_Builder()
    {
        // Arrange
        DefaultViteConfigurationBuilder builder = new();
        ViteConfiguration configuration = new();

        // Act
        builder.WithDefaultAssets("foo");
        builder.WithDevServer("http://localhost:3000");
        builder.WithScriptOptions(false, false);
        builder.AddAsset("foo", new ViteAsset { Src = "foo", File = "foo.js", IsEntry = true });
        builder.Build(configuration);

        // Assert
        configuration.Assets.Should().HaveCount(1);
        configuration.Assets.Should().ContainKey("foo");
        configuration.Assets["foo"].Src.Should().Be("foo");
        configuration.Assets["foo"].File.Should().Be("foo.js");
        configuration.Assets["foo"].IsEntry.Should().BeTrue();
        configuration.DefaultAssets.Should().BeEquivalentTo(new List<string> { "foo" });
        configuration.ViteDevServerUrl.Should().Be("http://localhost:3000");
        configuration.IsScriptModule.Should().BeFalse();
        configuration.IsScriptDefer.Should().BeFalse();
    }

    [Test]
    public void Test_AddAssetsFromManifestJson()
    {
        // Arrange
        const string manifestJson = @"{
  ""main.js"": {
        ""file"": ""assets/main.4889e940.js"",
        ""src"": ""main.js"",
        ""isEntry"": true,
        ""dynamicImports"": [""views/foo.js""],
        ""css"": [""assets/main.b82dbe22.css""],
        ""assets"": [""assets/asset.0ab0f9cd.png""]
    },
    ""views/foo.js"": {
        ""file"": ""assets/foo.869aea0d.js"",
        ""src"": ""views/foo.js"",
        ""isDynamicEntry"": true,
        ""imports"": [""_shared.83069a53.js""]
    },
    ""_shared.83069a53.js"": {
        ""file"": ""assets/shared.83069a53.js""
    }
}
";
        DefaultViteConfigurationBuilder builder = new();
        ViteConfiguration configuration = new();

        // Act
        builder.AddAssetsFromManifestJson(manifestJson);
        builder.Build(configuration);

        // Assert
        configuration.Assets.Should().HaveCount(3);
        configuration.Assets.Should().ContainKey("main.js");
        configuration.Assets["main.js"].File.Should().Be("assets/main.4889e940.js");
        configuration.Assets["main.js"].Src.Should().Be("main.js");
        configuration.Assets["main.js"].IsEntry.Should().BeTrue();
        configuration.Assets["main.js"].DynamicImports.Should().BeEquivalentTo(new List<string> { "views/foo.js" });
        configuration.Assets["main.js"].Css.Should().BeEquivalentTo(new List<string> { "assets/main.b82dbe22.css" });
        configuration.Assets["main.js"].Assets.Should().BeEquivalentTo(new List<string> { "assets/asset.0ab0f9cd.png" });
        configuration.Assets.Should().ContainKey("views/foo.js");
        configuration.Assets["views/foo.js"].File.Should().Be("assets/foo.869aea0d.js");
        configuration.Assets["views/foo.js"].Src.Should().Be("views/foo.js");
        configuration.Assets["views/foo.js"].IsDynamicEntry.Should().BeTrue();
        configuration.Assets["views/foo.js"].Imports.Should().BeEquivalentTo(new List<string> { "_shared.83069a53.js" });
        configuration.Assets.Should().ContainKey("_shared.83069a53.js");
        configuration.Assets["_shared.83069a53.js"].File.Should().Be("assets/shared.83069a53.js");
    }

    [Test]
    public void Test_AddAssetsFromManifest()
    {
        // Arrange
        DefaultViteConfigurationBuilder builder = new();
        ViteConfiguration configuration = new();
        ViteManifest manifest = new()
        {
            { "foo", new ViteAsset { Src = "foo", File = "foo.js" } }
        };

        // Act
        builder.AddAssetsFromManifest(manifest);
        builder.Build(configuration);

        // Assert
        configuration.Assets.Should().HaveCount(1);
        configuration.Assets.Should().ContainKey("foo");
        configuration.Assets["foo"].Src.Should().Be("foo");
        configuration.Assets["foo"].File.Should().Be("foo.js");
    }

    [Test]
    public void Test_AddAsset_ExistingThrows()
    {
        // Arrange
        DefaultViteConfigurationBuilder builder = new();

        // Act
        builder.AddAsset("foo", new ViteAsset { Src = "foo", File = "foo.js" });
        Action action = () => builder.AddAsset("foo", new ViteAsset { Src = "foo", File = "foo.js" });

        // Assert
        action.Should().Throw<ArgumentException>();
    }
}
