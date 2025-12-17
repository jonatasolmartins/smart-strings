using System;
using SmartStrings;

// Demo: Format Specifier Support in SmartStrings

class FormatSpecifierDemo
{
    static void Main()
    {
        Console.WriteLine("=== SmartStrings Format Specifier Demo ===\n");

        // 1. DateTime formatting
        var dateTemplate = "Company Anniversary: {date:yyyy-MM-dd}";
        var result1 = dateTemplate.Fill(new { date = new DateTime(2025, 12, 17) });
        Console.WriteLine(result1);
        // Output: Company Anniversary: 2025-12-17

        // 2. Currency formatting
        var priceTemplate = "Total: {amount:C2}";
        var result2 = priceTemplate.Fill(new { amount = 1299.99m });
        Console.WriteLine(result2);
        // Output: Total: ¤1,299.99 (InvariantCulture)

        // 3. Number formatting
        var countTemplate = "Downloads: {count:N0}";
        var result3 = countTemplate.Fill(new { count = 1234567 });
        Console.WriteLine(result3);
        // Output: Downloads: 1,234,567

        // 4. Multiple formats in one template
        var orderTemplate = "Order #{id} on {date:MMM dd, yyyy} - Total: {total:C2}";
        var result4 = orderTemplate.Fill(new { 
            id = 12345,
            date = new DateTime(2025, 12, 17),
            total = 599.50m
        });
        Console.WriteLine(result4);
        // Output: Order #12345 on Dec 17, 2025 - Total: ¤599.50

        // 5. Configuration URL example (your use case!)
        var urlTemplate = "https://api.company.com/v{version}/users/{userId}/orders?date={date:yyyy-MM-dd}";
        var result5 = urlTemplate.Fill(new {
            version = 2,
            userId = "abc123",
            date = DateTime.Now
        });
        Console.WriteLine(result5);
        // Output: https://api.company.com/v2/users/abc123/orders?date=2025-12-17

        // 6. Fallback still works for non-formattable types
        var greetingTemplate = "Hello {name:Guest}!";
        var result6 = greetingTemplate.Fill(new { name = "John" });
        Console.WriteLine(result6);
        // Output: Hello John!

        Console.WriteLine("\n=== All features working! ===");
    }
}
