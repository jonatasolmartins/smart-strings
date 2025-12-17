using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Globalization;

namespace SmartStrings.Tests;

public class ConfigurationTests : IDisposable
{
    public ConfigurationTests()
    {
        // Reset global options before each test
        SmartStringExtensions.ConfigureDefaults(new SmartStringsOptions());
    }

    public void Dispose()
    {
        // Reset global options after each test
        SmartStringExtensions.ConfigureDefaults(new SmartStringsOptions());
    }

    [Fact]
    public void AddSmartStrings_WithConfigurationSection_BindsCorrectly()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["SmartStrings:DefaultCulture"] = "en-US",
            ["SmartStrings:InheritThreadCulture"] = "false"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();

        // Act
        services.AddSmartStrings(configuration.GetSection("SmartStrings"));

        // Assert - verify global configuration was applied
        var template = "Price: {amount:C2}";
        var result = template.Fill(new { amount = 29.99m });

        result.ShouldBe("Price: $29.99"); // Should use en-US culture
    }

    [Fact]
    public void AddSmartStrings_WithConfigurationAndOverride_CombinesCorrectly()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["SmartStrings:InheritThreadCulture"] = "false"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();

        // Act - config sets InheritThreadCulture=false, override sets culture
        services.AddSmartStrings(
            configuration.GetSection("SmartStrings"),
            options => options.DefaultCulture = new CultureInfo("pt-BR")
        );

        // Assert
        var template = "Price: {amount:C2}";
        var result = template.Fill(new { amount = 29.99m });

        result.ShouldBe("Price: R$ 29,99"); // Should use pt-BR culture from override
    }

    [Fact]
    public void AddSmartStrings_WithEmptyConfiguration_UsesDefaults()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var services = new ServiceCollection();

        // Act
        services.AddSmartStrings(configuration.GetSection("SmartStrings"));

        // Assert - should use default behavior (current culture)
        var template = "Price: {amount:C2}";
        var result = template.Fill(new { amount = 29.99m });

        result.ShouldStartWith("Price: ");
        (result.Contains("29.99") || result.Contains("29,99")).ShouldBeTrue();
    }
}
