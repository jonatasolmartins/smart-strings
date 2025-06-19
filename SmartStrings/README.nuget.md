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
  - Nested object
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

## ✅ 6. Manual mapping with nested model

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

## ✅ 7. Using TemplateString.Fill (Alternative API)

```csharp
TemplateString.Fill("Hello {USERNAME}", new { USERNAME = "Joana" });

TemplateString.Fill("User: {NAME}", user, map => {
    map.Bind("NAME", u => u.User.Name);
});
```
---


## ✅ Compatibility

- .NET Standard 2.0
- .NET 6, 7, 8 and above

## 📦 Install

```bash
dotnet add package SmartStrings
```

📚 Full source, license, and documentation available at  
[github.com/jonatasolmartins/smart-strings](https://github.com/jonatasolmartins/smart-strings)
