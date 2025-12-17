# SmartStrings v2.0.0 Release Notes

## 🎉 Major Release - Culture Support & Format Specifiers

SmartStrings v2.0.0 introduces powerful new features that transform it from a simple templating library into a full-featured internationalization solution for .NET applications.

## ✨ New Features

### 🌍 **Culture Support**
- **ASP.NET Core Integration**: Full DI container support with `builder.Services.AddSmartStrings()`
- **Configuration Binding**: `appsettings.json` support for culture settings
- **Thread Culture Inheritance**: Respects ASP.NET Core request localization
- **Per-Template Overrides**: Specify culture for individual templates

### 📊 **Format Specifiers**
- **DateTime Formatting**: `{date:yyyy-MM-dd}`, `{date:MMM dd, yyyy}`
- **Currency Formatting**: `{price:C2}` with culture-aware symbols
- **Number Formatting**: `{count:N0}`, `{value:0.00}`
- **IFormattable Support**: Works with all .NET formattable types

### ⚙️ **Configuration Options**
- **Multiple Configuration Methods**: Code, DI, appsettings.json
- **Flexible Culture Resolution**: Explicit > Configured > Thread.CurrentCulture > InvariantCulture
- **Environment Compatibility**: Works across different development environments

## 🚀 Usage Examples

### Basic Format Specifiers
```csharp
"{date:yyyy-MM-dd}".Fill(new { date = DateTime.Now }); 
// Result: "2025-12-17"

"{price:C2}".Fill(new { price = 29.99m }); 
// Result: "$29.99" (en-US) or "R$ 29,99" (pt-BR)
```

### ASP.NET Core Integration
```csharp
// Program.cs
builder.Services.AddSmartStrings("en-US");

// Or from configuration
builder.Services.AddSmartStrings(builder.Configuration.GetSection("SmartStrings"));

// appsettings.json
{
  "SmartStrings": {
    "DefaultCulture": "pt-BR",
    "InheritThreadCulture": true
  }
}
```

### Culture Overrides
```csharp
var template = "Price: {amount:C2}";
var data = new { amount = 29.99m };

template.Fill(data, new CultureInfo("en-US")); // "$29.99"
template.Fill(data, new CultureInfo("pt-BR")); // "R$ 29,99"
template.Fill(data, new CultureInfo("fr-FR")); // "29,99 €"
```

### Configuration URLs
```csharp
// Perfect for appsettings.json + HTTP clients
var urlTemplate = "https://api.com/v{version}/orders?date={date:yyyy-MM-dd}";
var url = urlTemplate.Fill(new { version = 2, date = DateTime.Now });
// Result: "https://api.com/v2/orders?date=2025-12-17"
```

## 🔧 API Additions

### New Methods
- `Fill<T>(template, values, CultureInfo culture)`
- `TemplateString.Fill<T>(template, values, CultureInfo culture)`

### New Extensions (.NET 6+)
- `AddSmartStrings(string cultureName)`
- `AddSmartStrings(Action<SmartStringsOptions> configure)`
- `AddSmartStrings(IConfigurationSection configurationSection)`
- `AddSmartStrings(IConfigurationSection, Action<SmartStringsOptions>)`

### New Configuration
- `SmartStringsOptions` class
- `DefaultCulture` property
- `InheritThreadCulture` property

## 🛠 Compatibility

- ✅ **Fully Backward Compatible** - All existing code works unchanged
- ✅ **.NET Framework 4.6.1+** - Core functionality
- ✅ **.NET 6, 7, 8+** - Full feature set including DI integration
- ✅ **Multi-Target** - netstandard2.0 + net8.0

## 🧪 Quality Assurance

- **50 Comprehensive Tests** - Including culture-specific scenarios
- **Environment Independent** - Works in WSL, VS Code, CI/CD
- **Culture Agnostic Tests** - Handles different decimal separators and formats
- **Production Ready** - Extensive real-world scenario coverage

## 📦 Package Updates

- **Enhanced Description** - Reflects new capabilities
- **Updated Tags** - `culture`, `localization`, `aspnetcore`
- **Improved Documentation** - Comprehensive examples and usage patterns

## 🎯 Breaking Changes

**None!** This is a fully backward-compatible release. All existing SmartStrings code will continue to work exactly as before.

## 🙏 Acknowledgments

Special thanks to the .NET community for inspiration and the ASP.NET Core team for the excellent localization patterns that influenced this implementation.

---

**Download:** [NuGet Package](https://www.nuget.org/packages/SmartStrings/2.0.0)  
**Source:** [GitHub Repository](https://github.com/jonatasolmartins/smart-strings)  
**Documentation:** [README](https://github.com/jonatasolmartins/smart-strings#readme)
