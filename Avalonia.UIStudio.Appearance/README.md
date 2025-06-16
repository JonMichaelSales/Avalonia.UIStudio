

# 🎨 Avalonia.Accelerate.Appearance

![CI](https://github.com/JonMichaelSales/Avalonia.Accelerate/actions/workflows/ci.yml/badge.svg)  
![License](https://img.shields.io/github/license/JonMichaelSales/Avalonia.Accelerate)  
![NuGet](https://img.shields.io/nuget/v/Avalonia.Accelerate.Appearance?label=NuGet)  
![Platform](https://img.shields.io/badge/platform-.NET%208.0%20%7C%20Avalonia%2011-blueviolet)

> Powerful theme & skin management for Avalonia UI apps.  
> Runtime theme switching · Skin inheritance · Appearance validation · Quick theme switching control.

---

## ✨ Features

- ⚙️ **Skin Manager** with runtime theme switching
- 🎨 **Skin Inheritance** and property overrides
- 📂 **Skin Loader** for JSON-based skin definitions + AXAML control themes
- 🔍 **Validation rules** (color contrast, accessibility, borders, names)
- 🖼️ **QuickSkinSwitcher** reusable control
- 🗂️ **Automatic AppSettings persistence**
- 🚀 **Prebuilt sample skins**:
  - Dark, Light, Cyberpunk, Retro Terminal
  - Material Design 3, Ocean Blue, Forest Green, High Contrast, Zen Garden, Windows 11 Modern
- 🛠️ **Tooling-ready APIs**: `IThemeLoaderService`, `ISkinManager`

---

## 🏗 Project Structure

```text
Interfaces/        Core contracts: ISkinManager, ISkinValidationRule, etc.
Model/             Skin definitions, InheritableSkin, TypographyScale, AppSettings
Services/          SkinLoaderService, validation rules
Controls/          QuickSkinSwitcher.axaml (+ ViewModel)
Extensions/        AppBuilder, Application, ServiceCollection extensions
Skins/             Example skins with theme.json + ControlThemes/*.axaml
```

---

## 🛠 Getting Started

### Install the Package

```bash
dotnet add package Avalonia.Accelerate.Appearance
```

### Usage Example

#### 1️⃣ Configure AppBuilder

```csharp
AppBuilder.Configure<App>()
    .UsePlatformDetect()
    .UseSkinManager()
    .StartWithClassicDesktopLifetime(args);
```

#### 2️⃣ Initialize SkinManager

```csharp
protected override void OnFrameworkInitializationCompleted()
{
    AppBuilderExtensions.InitializeSkinManager();
    base.OnFrameworkInitializationCompleted();
}
```

#### 3️⃣ Include ThemeManager Styles (Optional)

```csharp
Application.Current.IncludeThemeManagerStyles();
```

#### 4️⃣ Use `QuickSkinSwitcher` Control

```xml
<appearance:QuickSkinSwitcher />
```

> This control binds to available themes and allows the user to switch themes at runtime.

---

## ✏️ Defining Custom Skins

1️⃣ Create a `theme.json`:

```json
{
  "name": "My Custom Theme",
  "description": "A beautiful custom skin",
  "version": "1.0",
  "PrimaryColor": "#FF123456",
  "AccentColor": "#FFABCDEF",
  "ControlThemes": [
    "ControlThemes/Button.axaml",
    "ControlThemes/TextBlock.axaml"
  ]
}
```

2️⃣ Place in: `Skins/MyTheme/theme.json`  
3️⃣ Load skins automatically at runtime.

---

## 🚦 Skin Validation

Available validation rules:

- `BorderValidationRule`
- `ColorContrastValidationRule`
- `NameValidationRule`
- `AccessibilityValidationRule`

Usage:

```csharp
var validator = new BorderValidationRule();
var result = validator.Validate(skin);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"Error: {error}");
    }
}
```

---

## 📚 Available Services

| Service Name            | Interface / Class              | Description |
|-------------------------|-------------------------------|-------------|
| Skin Manager            | `ISkinManager`                 | Runtime skin management |
| Theme Loader            | `IThemeLoaderService`          | Load skins from disk |
| Skin Validation Rules   | `ISkinValidationRule`          | Validate skins |
| App Settings            | `AppSettings` (singleton)      | Persist selected skin |
| Skin Inheritance Manager| `SkinInheritanceManager`       | Resolve inherited skins |

---

## 🗂 Sample Themes

- ✅ Arctic White
- ✅ Autumn Leaves
- ✅ Coffee Brown
- ✅ Coral Reef
- ✅ Cyberpunk
- ✅ Dark
- ✅ Electric Blue
- ✅ Forest Green
- ✅ High Contrast
- ✅ Lavender Dreams
- ✅ Light
- ✅ Material Design 3
- ✅ Midnight Purple
- ✅ ModernIce
- ✅ Neon Green
- ✅ Ocean Blue
- ✅ Purple Haze
- ✅ RetroTerminal
- ✅ Rose Gold
- ✅ Slate Grey
- ✅ Steel Blue
- ✅ Sunset Orange
- ✅ Windows 11 Modern
- ✅ Zen Garden

---

## 🎁 Advanced Features

- Skin Inheritance: `BaseSkinName` and `PropertyOverrides`
- Dynamic Preview Colors in QuickSkinSwitcher
- Extensible validation pipeline
- DI-friendly architecture with `IServiceCollection`

---

## 🚀 Roadmap

- [x] Inheritance support with property override system
- [x] JSON skin import/export
- [x] Full ControlTheme URI mapping
- [ ] Live preview in designer
- [ ] Theme editing UI component
- [ ] Skin pack import/export API
- [ ] NuGet skin theme packs

---

## ❤️ Example App

```csharp
var skinManager = AppBuilderExtensions.GetRequiredService<ISkinManager>();

skinManager.ApplySkin("Ocean Blue");

skinManager.SkinChanged += (_, __) =>
{
    Console.WriteLine($"Skin changed to: {skinManager.CurrentSkin?.Name}");
};
```

---

## ⚖️ License

This project is licensed under the MIT License.  
See the [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing

Contributions welcome!  
Please fork the repository, create a feature branch, and submit a pull request.

---

## 📎 Related Projects

- [Avalonia UI](https://github.com/AvaloniaUI/Avalonia) — Modern, cross-platform .NET UI framework
- [Avalonia.Accelerate.Icons](../Avalonia.Accelerate.Icons) — Icon system used in this project

---

## 🌟 Sponsor / Support

If this project saves you time or enhances your Avalonia apps — give it a ⭐ on GitHub!  
Feedback and contributions are very welcome.

---
