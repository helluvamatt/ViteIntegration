using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ViteIntegration.Internals;

namespace ViteIntegration.Tests;

public class ServiceCollectionExtensionTests
{
    [Test]
    public void Test_AddViteIntegration()
    {
        // Arrange
        ServiceCollection services = [];

        // Act
        services.AddViteIntegration();

        // Assert
        services.Should().Contain(sd => sd.ServiceType == typeof(ITagHelperComponent) && sd.Lifetime == ServiceLifetime.Transient);
        services.Should().Contain(sd => sd.ServiceType == typeof(IViteAssetService) && sd.Lifetime == ServiceLifetime.Singleton);
    }
}
