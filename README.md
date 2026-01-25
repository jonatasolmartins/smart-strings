# SmartStrings


![SmartStrings logo](./SmartStrings/logo-readme.png)

[![NuGet](https://img.shields.io/nuget/v/SmartStrings.svg)](https://www.nuget.org/packages/SmartStrings)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SmartStrings.svg)](https://www.nuget.org/packages/SmartStrings)
[![CI](https://github.com/jonatasolmartins/smart-strings/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/jonatasolmartins/smart-strings/actions/workflows/nuget-publish.yml)
[![Coverage Status](https://img.shields.io/badge/coverage-100%25-brightgreen)](https://github.com/jonatasolmartins/smart-strings)
[![GitHub stars](https://img.shields.io/github/stars/jonatasolmartins/smart-strings.svg?style=social)](https://github.com/jonatasolmartins/smart-strings/stargazers)

**SmartStrings** is a lightweight and intuitive C# string templating library. It adds extension methods like `.Fill()` that let you replace placeholders in strings using objects, dictionaries, or parameter arrays — with optional fallbacks.

> 💡 Inspired by the flexibility of `$"{name}"`, but better suited for dynamic or external templates.

---

## ✨ Features

- ✅ Replace named placeholders like `{name}`, `{plan}`, etc.
- ✅ **Format specifiers** for dates, numbers, currency: `{date:yyyy-MM-dd}`, `{price:C2}`
- ✅ **Culture support** with ASP.NET Core integration and `appsettings.json` binding
- ✅ Optional fallback values using `{name:Guest}` syntax
- ✅ Fill from:
  - A single value
  - Multiple ordered values
  - An object
  - A dictionary
  - Nested object
- ✅ Safe: handles `null`, missing keys, and extra placeholders gracefully
- ✅ Works with .NET Framework (4.6.1+), .NET 6, 7, 8 and future versions

---

## 📦 Installation

Install via NuGet:

```bash
dotnet add package SmartStrings
```

Or reference it from a local `.nupkg`:

```bash
dotnet add package SmartStrings --source ./packages
```

---

## 🚀 Usage

### ✅ 1. Single placeholder with one value

```csharp
using SmartStrings;

var template = "Welcome, {user}!";
var result = template.Fill("Alice");
// Result: "Welcome, Alice!"
```

---

### ✅ 2. Multiple placeholders (ordered)

```csharp
const string template = "Hello {0}, your plan is {1}";
var result = template.Fill("Joe", "Premium");
// Result: "Hello Joe, your plan is Premium"

// Alternative
var result = TemplateString.Fill(template, "Joe", "Premium");
// Result: "Hello Joe, your plan is Premium"
```

Or using named placeholders:

```csharp
var template = "Hello {name}, your {plan} plan is active.";
var result = template.Fill("Jonatas", "Gold");
// Result: "Hello Jonatas, your Gold plan is active."s
```

---

### ✅ 3. Using a dictionary

```csharp
var template = "Hi {name}, you're on the {plan} plan.";
var result = template.Fill(new Dictionary<string, string?>
{
    ["name"] = "Carla",
    ["plan"] = "Standard"
});
// Result: "Hi Carla, you're on the Standard plan."
```

---

### ✅ 4. Using an anonymous object

```csharp
var template = "Welcome {name}, your ID is {id}.";
var result = template.Fill(new { name = "Lucas", id = "12345" });
// Result: "Welcome Lucas, your ID is 12345."
```

---

### ✅ 5. Using fallback values

```csharp
var template = "Hi {name:Guest}, welcome!";
var result = template.Fill(new { });
// Result: "Hi Guest, welcome!"
```

---

### ✅ 6. Format specifiers for dates, numbers, and currency

```csharp
// DateTime formatting
var template = "Event date: {date:yyyy-MM-dd}";
var result = template.Fill(new { date = new DateTime(2025, 12, 17) });
// Result: "Event date: 2025-12-17"

// Currency formatting
var template = "Total: {amount:C2}";
var result = template.Fill(new { amount = 1299.99m });
// Result: "Total: ¤1,299.99"

// Number formatting
var template = "Downloads: {count:N0}";
var result = template.Fill(new { count = 1234567 });
// Result: "Downloads: 1,234,567"

// Multiple formats in one template
var template = "Order #{id} on {date:MMM dd, yyyy} - Total: {total:C2}";
var result = template.Fill(new { 
    id = 12345,
    date = new DateTime(2025, 12, 17),
    total = 599.50m
});
// Result: "Order #12345 on Dec 17, 2025 - Total: ¤599.50"
```

---

### ✅ 7. Configuration-based URL templating

Perfect for `appsettings.json` and HTTP clients:

```json
{
  "ApiEndpoints": {
    "UserOrders": "https://api.company.com/v{version}/users/{userId}/orders?date={date:yyyy-MM-dd}&status={status:active}"
  }
}
```

```csharp
var urlTemplate = _config["ApiEndpoints:UserOrders"];
var url = urlTemplate.Fill(new {
    version = 2,
    userId = "abc123",
    date = DateTime.Now,
    status = "pending"
});
// Result: "https://api.company.com/v2/users/abc123/orders?date=2025-12-17&status=pending"
```

---

### ✅ 8. Culture support for international applications

**ASP.NET Core Integration:**
```csharp
// Program.cs - Multiple configuration options
builder.Services.AddSmartStrings("en-US"); // Simple culture
builder.Services.AddSmartStrings(options => 
{
    options.DefaultCulture = new CultureInfo("pt-BR");
    options.InheritThreadCulture = true; // Respects ASP.NET request localization
});

// From appsettings.json
builder.Services.AddSmartStrings(builder.Configuration.GetSection("SmartStrings"));
```

**appsettings.json:**
```json
{
  "SmartStrings": {
    "DefaultCulture": "en-US",
    "InheritThreadCulture": true
  }
}
```

**Usage with different cultures:**
```csharp
// Uses configured culture
"{price:C2}".Fill(new { price = 29.99m }); // "$29.99" (en-US) or "R$ 29,99" (pt-BR)

// Override culture per template
"{price:C2}".Fill(new { price = 29.99m }, new CultureInfo("fr-FR")); // "29,99 €"

// Different templates for different cultures
var data = new { price = 29.99m };
var usPrice = "US: {price:C2}".Fill(data, new CultureInfo("en-US")); // "US: $29.99"
var brPrice = "BR: {price:C2}".Fill(data, new CultureInfo("pt-BR")); // "BR: R$ 29,99"
var frPrice = "FR: {price:C2}".Fill(data, new CultureInfo("fr-FR")); // "FR: 29,99 €"
```

---

## ✅ 9. Manual mapping with nested model

```csharp
var card = new Card()
{
    User = new User() {
        Name = "Brian",
        Company = "SmartCo"
    }
};
const string template = "Welcome {NAME} from {COMPANY}"

template.Fill(card, map => {
    map.Bind("NAME", c => c.User.Name);
    map.Bind("COMPANY", c => c.User.Company);
});
// Welcome Brian from SmartCo
```

---

## ✅ 10. Using TemplateString.Fill (Alternative API)

```csharp
TemplateString.Fill("Hello {USERNAME}", new { USERNAME = "Joana" });

TemplateString.Fill("User: {NAME}", user, map => {
    map.Bind("NAME", u => u.User.Name);
});

// With culture
TemplateString.Fill("Price: {amount:C2}", new { amount = 29.99m }, new CultureInfo("en-US"));
```
---

## 🧪 Supported APIs

```csharp
// Replace first {placeholder}
string Fill(this string template, string value);

// Replace all {..} in order
string Fill(this string template, params string[] values);

// Replaces placeholders with values from a model or primitive values
string Fill<T>(this string template, T values);

// With culture support
string Fill<T>(this string template, T values, CultureInfo culture);

// Replace named placeholders with dictionary values
string Fill(this string template, Dictionary<string, string?> values);

// Manual mapping for nested or formatted values
string Fill<T>(this string template, T model, Action<TemplateMap<T>> map);

// ASP.NET Core DI integration (.NET 6+)
IServiceCollection AddSmartStrings(this IServiceCollection services, string cultureName);
IServiceCollection AddSmartStrings(this IServiceCollection services, Action<SmartStringsOptions> configure);
IServiceCollection AddSmartStrings(this IServiceCollection services, IConfigurationSection configurationSection);
```

---

## 🛠 Compatibility

SmartStrings targets:
- ✅ .NET Framework (4.6.1+)
- ✅ .NET 6
- ✅ .NET 7
- ✅ .NET 8 and above

---

---

## 💖 Support This Project

If SmartStrings has saved you time or made your life easier, please consider:

- ⭐ Starring the repo
- 📣 Sharing it with others
- 🧩 [Contributing](/CONTRIBUTING.md) with ideas, feedback, or pull requests


Thank you! 🙌

☕ [Buy Me a Coffee with ₿] -> erodingbowl94@walletofsatoshi.com or bc1q4sv5sgp4k6tdtrcrhk9nldq5d4ja3rhvr5rluu

## 📄 License

This project is licensed under the [MIT License](./%20LICENSE.txt).
© 2025 Jonatas Olziris Martins. All rights reserved.

You may use, modify, and distribute this library in commercial and non-commercial applications. Attribution required.
