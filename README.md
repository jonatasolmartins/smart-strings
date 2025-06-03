# SmartStrings

**SmartStrings** is a lightweight and intuitive C# string templating library. It adds extension methods like `.Fill()` that let you replace placeholders in strings using objects, dictionaries, or parameter arrays — with optional fallbacks.

> 💡 Inspired by the flexibility of `$"{name}"`, but better suited for dynamic or external templates.

---

## ✨ Features

- ✅ Replace named placeholders like `{name}`, `{plan}`, etc.
- ✅ Optional fallback values using `{name:Guest}` syntax
- ✅ Fill from:
  - A single value
  - Multiple ordered values
  - An object
  - A dictionary
- ✅ Safe: handles `null`, missing keys, and extra placeholders gracefully
- ✅ Works with .NET 6, 7, 8 and future versions

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
var template = "Hello {0}, your plan is {1}";
var result = template.Fill("Joe", "Premium");
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

## 🧪 Supported APIs

```csharp
// Replace first {placeholder}
string Fill(this string template, string value);

// Replace all {..} in order
string Fill(this string template, params string[] values);

// Replace named placeholders with object properties
string Fill(this string template, object values);

// Replace named placeholders with dictionary values
string Fill(this string template, Dictionary<string, string?> values);
```

---

## 🛠 Compatibility

SmartStrings targets:

- ✅ .NET 6
- ✅ .NET 7
- ✅ .NET 8 and above

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).  
© 2025 Jonatas Olziris Martins. All rights reserved.

You may use, modify, and distribute this library in commercial and non-commercial applications. Attribution required.
