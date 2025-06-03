using Shouldly;

namespace SmartStrings.Tests;

public class SmartStringExtensionsTests
{
    
    [Fact]
    public void Fill_WithSinglePlaceholder_ReplacesCorrectly()
    {
        var template = "Hello {name}";
        var result = template.Fill("John");
        result.ShouldBe("Hello John");
    }

    [Fact]
    public void Fill_WithMultipleValues_ReplacesInOrder()
    {
        var template = "Hi {0}, welcome to {1}";
        var result = template.Fill("Alice", "Wonderland");
        result.ShouldBe("Hi Alice, welcome to Wonderland");
    }

    [Fact]
    public void Fill_WithDictionary_ReplacesNamedPlaceholders()
    {
        var template = "Hello {user}, your plan is {plan}";
        var values = new Dictionary<string, string?>
        {
            { "user", "Lucas" },
            { "plan", "Premium" }
        };

        var result = template.Fill(values);
        result.ShouldBe("Hello Lucas, your plan is Premium");
    }

    [Fact]
    public void Fill_WithObject_ReplacesPlaceholdersWithPropertyValues()
    {
        var template = "User: {Name}, Plan: {Plan}";
        var result = template.Fill(new { Name = "Joana", Plan = "Basic" });

        result.ShouldBe("User: Joana, Plan: Basic");
    }

    [Fact]
    public void Fill_WithFallback_UsesFallbackWhenKeyIsMissing()
    {
        var template = "Hi {name:User}, welcome!";
        var result = template.Fill(new Dictionary<string, string?>());

        result.ShouldBe("Hi User, welcome!");
    }

    [Fact]
    public void Fill_WithNullValue_UsesFallback()
    {
        var template = "Hi {name:Guest}";
        var result = template.Fill(new Dictionary<string, string?> { ["name"] = null });

        result.ShouldBe("Hi Guest");
    }

    [Fact]
    public void Fill_WithEmptyTemplate_ReturnsEmptyString()
    {
        var result = "".Fill("test");
        result.ShouldBeEmpty();
    }

    [Fact]
    public void Fill_WithNoPlaceholders_ReturnsOriginalString()
    {
        var result = "Just a normal string".Fill("ignored");
        result.ShouldBe("Just a normal string");
    }

    [Fact]
    public void Fill_WithNullTemplate_ReturnTheNullTemplate()
    {
        string? template = null;

        var act = template!.Fill("value");
        act.ShouldBeNull();
    }

    [Fact]
    public void Fill_WithNullObject_ReplacesWithFallbacksOrEmpty()
    {
        var template = "Hello {name:Guest}, welcome!";
        object? nullValue = null;
        var result = template.Fill(nullValue!);

        result.ShouldBe("Hello Guest, welcome!");
    }

    [Fact]
    public void Fill_WithExtraValues_IgnoresThem()
    {
        var template = "Hello {0}";
        var result = template.Fill("Alice", "Extra", "More");

        result.ShouldBe("Hello Alice");
    }

    [Fact]
    public void Fill_WithFewerValues_LeavesPlaceholderUnchanged()
    {
        var template = "Hello {0}, welcome to {1}";
        var result = template.Fill("Alice");

        result.ShouldBe("Hello Alice, welcome to {1}");
    }

    [Fact]
    public void Fill_WithSpecialCharactersInValues_ReplacesCorrectly()
    {
        var template = "Path: {path}";
        var result = template.Fill(new { path = @"C:\Users\Name" });

        result.ShouldBe(@"Path: C:\Users\Name");
    }

    [Fact]
    public void Fill_WithBracesInsideValues_DoesNotBreak()
    {
        var template = "Log: {message}";
        var result = template.Fill(new { message = "Something {weird} happened" });

        result.ShouldBe("Log: Something {weird} happened");
    }
}