using Microsoft.AspNetCore.Mvc.Rendering;

namespace ViteIntegration.Tests;

public class ViewContextExtensionsTests
{
    [Test]
    public void Test_RequireViteAssets()
    {
        // Arrange
        ViewContext viewContext = new();

        // Act
        viewContext.RequireViteAssets("foo", "bar");

        // Assert
        viewContext.ViewData.ContainsKey(ViewContextExtensions.ContextKey).Should().BeTrue();
        viewContext.ViewData[ViewContextExtensions.ContextKey].Should().BeOfType<List<string>>();
        viewContext.ViewData[ViewContextExtensions.ContextKey].Should().BeEquivalentTo(new List<string> { "foo", "bar" });
    }

    [Test]
    public void Test_GetAssetNames()
    {
        // Arrange
        ViewContext viewContext = new()
        {
            ViewData =
            {
                [ViewContextExtensions.ContextKey] = new List<string> { "foo", "bar" }
            }
        };

        // Act
        IEnumerable<string> assetNames = viewContext.GetAssetNames();

        // Assert
        assetNames.Should().BeEquivalentTo(new List<string> { "foo", "bar" });
    }
}
