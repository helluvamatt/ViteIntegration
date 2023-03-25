using System.Text.Json;

namespace ViteIntegration.Tests;

public class ViteAssetTests
{
    [Test]
    public void Test_Deserialize()
    {
        // Arrange
        const string json = @"{
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

        // Act
        Dictionary<string, ViteAsset> manifest = JsonSerializer.Deserialize<Dictionary<string, ViteAsset>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        // Assert
        manifest.Should().NotBeNull();
        manifest.Should().HaveCount(3);
        manifest.Should().ContainKey("main.js");
        manifest["main.js"].File.Should().Be("assets/main.4889e940.js");
        manifest["main.js"].Src.Should().Be("main.js");
        manifest["main.js"].IsEntry.Should().BeTrue();
        manifest["main.js"].DynamicImports.Should().BeEquivalentTo("views/foo.js");
        manifest["main.js"].Css.Should().BeEquivalentTo("assets/main.b82dbe22.css");
        manifest["main.js"].Assets.Should().BeEquivalentTo("assets/asset.0ab0f9cd.png");
        manifest.Should().ContainKey("views/foo.js");
        manifest["views/foo.js"].File.Should().Be("assets/foo.869aea0d.js");
        manifest["views/foo.js"].Src.Should().Be("views/foo.js");
        manifest["views/foo.js"].IsDynamicEntry.Should().BeTrue();
        manifest["views/foo.js"].Imports.Should().BeEquivalentTo("_shared.83069a53.js");
        manifest.Should().ContainKey("_shared.83069a53.js");
        manifest["_shared.83069a53.js"].File.Should().Be("assets/shared.83069a53.js");
    }
}
