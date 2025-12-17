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

    [Fact]
    public void Fill_WithDateTimeFormat_FormatsCorrectly()
    {
        var template = "Date: {date:yyyy-MM-dd}";
        var result = template.Fill(new { date = new DateTime(2025, 12, 17) });

        result.ShouldBe("Date: 2025-12-17");
    }

    [Fact]
    public void Fill_WithCurrencyFormat_FormatsCorrectly()
    {
        var template = "Price: {amount:C2}";
        var result = template.Fill(new { amount = 29.99m });

        result.ShouldBe("Price: ¤29.99"); // InvariantCulture uses ¤ symbol
    }

    [Fact]
    public void Fill_WithNumberFormat_FormatsCorrectly()
    {
        var template = "Count: {count:N0}";
        var result = template.Fill(new { count = 1234567 });

        result.ShouldBe("Count: 1,234,567");
    }

    [Fact]
    public void Fill_WithMultipleFormats_FormatsAllCorrectly()
    {
        var template = "Order on {date:MMM dd, yyyy} for {price:C2}";
        var result = template.Fill(new { 
            date = new DateTime(2025, 12, 17), 
            price = 99.50m 
        });

        result.ShouldBe("Order on Dec 17, 2025 for ¤99.50"); // InvariantCulture uses ¤ symbol
    }

    [Fact]
    public void Fill_WithStringAndFallback_UsesFallbackNotFormat()
    {
        var template = "Hello {name:Guest}";
        var result = template.Fill(new { name = "John" });

        result.ShouldBe("Hello John");
    }

    [Fact]
    public void Fill_WithInvalidFormatOnDateTime_AppliesPartialFormat()
    {
        var template = "Value: {num:0.00}";
        var result = template.Fill(new { num = 123.456m });

        result.ShouldBe("Value: 123.46");
    }
}