# SmartStrings

**SmartStrings** is a lightweight and intuitive C# string templating library. It adds extension methods like `.Fill()` that let you replace placeholders in strings using objects, dictionaries, or parameter arrays — with optional fallbacks.

> 💡 Inspired by the flexibility of `$"{name}"`, but better suited for dynamic or external templates.

## ✨ Features

- ✅ Replace named placeholders like `{name}`, `{plan}`, etc.
- ✅ Optional fallback values using `{name:Guest}` syntax
- ✅ Fill from:
  - A single value
  - Multiple ordered values
  - An object
  - A dictionary
- ✅ Safe: handles `null`, missing keys, and extra placeholders gracefully
- ✅ Works with .NET Framework (4.6.1+), .NET 6, 7, 8 and future versions


## 🚀 Usage

### ✅ 1. Single placeholder with one value

```csharp
using SmartStrings;

var template = "Welcome, {user}!";
var result = template.Fill("Alice");
// Result: "Welcome, Alice!"
```

### ✅ 2. Using fallback values

```csharp
var template = "Hi {name:Guest}, welcome!";
var result = template.Fill(new { });
// Result: "Hi Guest, welcome!"
```

## ✅ Compatibility

- .NET Standard 2.0
- .NET 6, 7, 8 and above

## 📦 Install

```bash
dotnet add package SmartStrings
```

📚 Full source, license, and documentation available at  
[github.com/yourusername/smart-string](https://github.com/jonatasolmartins/smart-strings)
