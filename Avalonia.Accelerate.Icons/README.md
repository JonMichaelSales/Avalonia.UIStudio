<img src="docs/logo.png" alt="Avalonia.Accelerate.Icons" width="300" />
# Avalonia.Accelerate.Icons

A comprehensive SVG-based icon library for Avalonia UI applications, providing a centralized repository of scalable vector icons with performance optimization and clean architecture following SOLID principles.

## 🎯 Overview

Avalonia.Accelerate.Icons delivers a robust, type-safe icon management system specifically designed for cross-platform Avalonia applications. The library provides an extensive collection of SVG-based icons that integrate seamlessly with Avalonia's Path controls and theming system.

## ✨ Key Features

- **📦 Centralized Icon Repository**: Single source of truth for all application icons
- **🎨 SVG-Based Scalable Icons**: Vector graphics that maintain quality at any size
- **⚡ Performance Optimized**: Built-in caching system for optimal performance
- **🏗️ SOLID Architecture**: Interface-driven design following best practices
- **🎨 Theme Integration**: Works seamlessly with dynamic theming systems
- **📱 Cross-Platform**: Full compatibility with all Avalonia-supported platforms
- **🔧 Type-Safe Access**: Compile-time icon name validation
- **💡 Helper Methods**: Convenient methods for creating Path controls programmatically

## 🏗️ Architecture

The library follows a clean, interface-based architecture:

```
Avalonia.Accelerate.Icons/
├── ApplicationIcons.cs           # Main icon repository
├── Interfaces/
│   └── IIconProvider.cs         # Icon provider contract
└── Assets/
    └── jonBuilt.ico            # Library icon asset
```

### Core Components

- **`ApplicationIcons`**: Central repository implementing `IIconProvider`
- **`IIconProvider`**: Interface defining icon provider contract
- **Icon Categories**: Organized collections (Core System, UI Navigation, etc.)

## 📦 Installation

### Package Manager Console
```powershell
Install-Package Avalonia.Accelerate.Icons
```

### .NET CLI
```bash
dotnet add package Avalonia.Accelerate.Icons
```

### PackageReference
```xml
<PackageReference Include="Avalonia.Accelerate.Icons" Version="1.0.0" />
```

## 🚀 Quick Start

### 1. Add Namespace Reference

In your XAML files:
```xml
xmlns:icons="clr-namespace:Avalonia.Accelerate.Icons;assembly=Avalonia.Accelerate.Icons"
```

### 2. Use Icons in XAML

```xml
<!-- Basic icon usage -->
<Path Width="16" 
      Height="16"
      Data="{x:Static icons:ApplicationIcons.SettingsGeometry}"
      Fill="{DynamicResource AccentBlueBrush}"
      Stretch="Uniform" />

<!-- Status indicator with themed colors -->
<Path Width="12"
      Height="12" 
      Data="{x:Static icons:ApplicationIcons.SuccessGeometry}"
      Fill="{DynamicResource SuccessBrush}"
      Stretch="Uniform" />
```

### 3. Programmatic Usage

```csharp
using Avalonia.Accelerate.Icons;
using Avalonia.Accelerate.Icons.Interfaces;

// Create icon provider
var iconProvider = new ApplicationIcons();

// Get icon geometry
var searchIcon = iconProvider.GetIcon("Search");

// Create Path control with helper method
var iconPath = iconProvider.CreatePath("Settings", 16, Brushes.Blue);
```

## 📚 Detailed Usage

### XAML Integration

#### Button with Icon
```xml
<Button>
    <StackPanel Orientation="Horizontal" Spacing="8">
        <Path Width="16" 
              Height="16"
              Data="{x:Static icons:ApplicationIcons.RefreshGeometry}"
              Fill="{DynamicResource TextPrimaryBrush}"
              Stretch="Uniform" />
        <TextBlock Text="Refresh" />
    </StackPanel>
</Button>
```

#### Status Bar Integration
```xml
<StackPanel Orientation="Horizontal" Spacing="5">
    <Path Width="12"
          Height="12"
          Data="{x:Static icons:ApplicationIcons.SuccessGeometry}"
          Fill="{DynamicResource SuccessBrush}"
          Stretch="Uniform" />
    <TextBlock FontSize="12" Text="Operation completed successfully" />
</StackPanel>
```

### Programmatic Creation

#### Basic Icon Creation
```csharp
// Method 1: Using interface
IIconProvider iconProvider = new ApplicationIcons();
var geometry = iconProvider.GetIcon("File");

// Method 2: Direct access
var folderGeometry = ApplicationIcons.GetIcon("Folder");
```

#### Advanced Path Control Creation
```csharp
// Create a themed icon path
var iconPath = iconProvider.CreatePath(
    iconName: "Settings",
    size: 24,
    brush: new SolidColorBrush(Colors.Blue)
);

// Add to container
myStackPanel.Children.Add(iconPath);
```

### Icon Categories and Available Icons

#### Core System Icons
- `File` - Generic file representation
- `Folder` - Folder structure
- `FolderOpen` - Open folder state
- `Lock` - Security/locked state

#### UI Navigation Icons
- `Search` - Search functionality
- `Settings` - Configuration/settings
- `Refresh` - Refresh/reload operations
- `Success` - Success state indicator

#### Performance Features

The library includes built-in performance optimizations:

```csharp
// Icons are cached automatically for performance
private static readonly Dictionary<string, string> _iconCache = new();

// Cache is initialized once on first access
static ApplicationIcons()
{
    InitializeIconCache();
}
```

## 🎨 Theme Integration

The library works seamlessly with Avalonia's dynamic theming:

```xml
<!-- Icons automatically adapt to theme changes -->
<Path Data="{x:Static icons:ApplicationIcons.SettingsGeometry}"
      Fill="{DynamicResource AccentBlueBrush}" />
```

## 🏆 Best Practices

### 1. Icon Sizing
```xml
<!-- Use consistent sizing -->
<Path Width="16" Height="16" ... /> <!-- Small UI elements -->
<Path Width="24" Height="24" ... /> <!-- Medium buttons -->
<Path Width="32" Height="32" ... /> <!-- Large headers -->
```

### 2. Theme-Aware Colors
```xml
<!-- Always use dynamic resources for theming -->
<Path Fill="{DynamicResource TextPrimaryBrush}" ... />
<Path Fill="{DynamicResource AccentBlueBrush}" ... />
```

### 3. Accessibility
```xml
<!-- Provide meaningful automation properties -->
<Path Data="{x:Static icons:ApplicationIcons.SearchGeometry}"
      AutomationProperties.Name="Search"
      AutomationProperties.HelpText="Click to search" />
```

### 4. Performance Optimization
```csharp
// Cache icon provider instances when using programmatically
private static readonly IIconProvider _iconProvider = new ApplicationIcons();

// Reuse Path controls when possible
var cachedIconPath = _iconProvider.CreatePath("Settings", 16);
```

## 🔧 Advanced Scenarios

### Custom Icon Provider Implementation

```csharp
public class CustomIconProvider : IIconProvider
{
    public Geometry GetIcon(string iconName)
    {
        // Custom implementation
        return Geometry.Parse(GetCustomIconPath(iconName));
    }

    public IEnumerable<string> GetAvailableIcons()
    {
        // Return custom icon names
        return _customIcons.Keys;
    }

    public bool HasIcon(string iconName)
    {
        return _customIcons.ContainsKey(iconName);
    }
}
```

### Dependency Injection Integration

```csharp
// Program.cs or App.xaml.cs
services.AddSingleton<IIconProvider, ApplicationIcons>();

// ViewModel usage
public class MyViewModel
{
    private readonly IIconProvider _iconProvider;
    
    public MyViewModel(IIconProvider iconProvider)
    {
        _iconProvider = iconProvider;
    }
    
    public void CreateDynamicIcon()
    {
        var icon = _iconProvider.CreatePath("Success", 20, Brushes.Green);
        // Use icon...
    }
}
```

## 🔗 Integration with Avalonia.Accelerate Suite

This library is part of the larger Avalonia.Accelerate ecosystem:

- **Avalonia.Accelerate.Icons** (this library) - Icon management
- **Avalonia.Accelerate.Appearance** - Theme and skin management
- **Avalonia.Accelerate.Dialogs** - Consistent dialog system

Example of integrated usage:
```xml
<!-- Using icons with appearance theming -->
<Path Data="{x:Static icons:ApplicationIcons.SettingsGeometry}"
      Fill="{DynamicResource AccentBlueBrush}" />
```

## 📋 Requirements

- **.NET 8.0** or later
- **Avalonia UI 11.3.1** or later
- **C# 12** language features

### Dependencies
- Avalonia (≥11.3.1)
- Avalonia.Desktop (≥11.3.1)
- Microsoft.Extensions.DependencyInjection (≥9.0.5)
- Microsoft.Extensions.Logging (≥9.0.5)

## 🏃‍♂️ Performance Characteristics

- **Memory Efficient**: Icons cached as static strings
- **Fast Lookup**: Dictionary-based icon retrieval
- **Lazy Loading**: Icons loaded only when accessed
- **Small Footprint**: Minimal runtime overhead

## 🔍 Troubleshooting

### Common Issues

#### Icons Not Displaying
```xml
<!-- Ensure proper namespace reference -->
xmlns:icons="clr-namespace:Avalonia.Accelerate.Icons;assembly=Avalonia.Accelerate.Icons"

<!-- Verify icon name exists -->
<Path Data="{x:Static icons:ApplicationIcons.SearchGeometry}" />
```

#### Theme Colors Not Applied
```xml
<!-- Use DynamicResource instead of StaticResource -->
<Path Fill="{DynamicResource AccentBrush}" /> <!-- ✅ Correct -->
<Path Fill="{StaticResource AccentBrush}" />  <!-- ❌ Won't update -->
```

#### Performance Issues
```csharp
// Don't create new providers repeatedly
// ❌ Bad
for (int i = 0; i < 1000; i++)
{
    var provider = new ApplicationIcons(); // Creates cache each time
}

// ✅ Good  
var provider = new ApplicationIcons(); // Create once
for (int i = 0; i < 1000; i++)
{
    var icon = provider.GetIcon("Search"); // Reuse provider
}
```

## 📄 License

[Add your license information here]

## 🤝 Contributing

[Add contribution guidelines here]

## 📞 Support

[Add support information here]

---

**Built with ❤️ for the Avalonia community**