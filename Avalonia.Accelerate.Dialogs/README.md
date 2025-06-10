<img src="docs/logo.png" alt="Avalonia.Accelerate.Dialogs" width="300" />
# Avalonia.Accelerate.Dialogs

**Beautiful, modern dialogs for Avalonia UI applications — error, warning, info, validation, confirmation, and more.**

[![NuGet](https://img.shields.io/nuget/v/Avalonia.Accelerate.Dialogs.svg)](https://www.nuget.org/packages/Avalonia.Accelerate.Dialogs)
[![License](https://img.shields.io/github/license/your-username/Avalonia.Accelerate.Dialogs.svg)](LICENSE)
[![Build](https://img.shields.io/github/actions/workflow/status/your-username/Avalonia.Accelerate.Dialogs/build.yml)](https://github.com/your-username/Avalonia.Accelerate.Dialogs/actions)

---

## ✨ Features

✅ Simple API for showing dialogs  
✅ Consistent modern styling (Fluent-inspired)  
✅ **Error** dialog with optional exception details  
✅ **Warning** and **Info** dialogs  
✅ **Validation** dialog (errors + warnings list)  
✅ **Confirmation** dialogs with customizable buttons  
✅ Integration with `Microsoft.Extensions.DependencyInjection`  
✅ Static `MessageBox` class for convenience  
✅ Headless test support  
✅ No external dependencies other than Avalonia + MS.Extensions  

---

## 🚀 Getting Started

### 1️⃣ Install via NuGet

```bash
dotnet add package Avalonia.Accelerate.Dialogs
```

---

### 2️⃣ Setup in AppBuilder

#### Option 1: Use `DialogService` with DI

```csharp
AppBuilder.Configure<App>()
    .UsePlatformDetect()
    .UseDialogServices();
```

#### Option 2: Configure manually

```csharp
AppBuilder.Configure<App>()
    .UsePlatformDetect()
    .UseDialogServices(services =>
    {
        services.AddDialogServices();
        // register your own services here if needed
    });
```

---

### 3️⃣ Using the `IDialogService`

```csharp
var dialogService = DialogServiceLocator.GetRequiredService<IDialogService>();

await dialogService.ShowErrorAsync("Error", "Something went wrong", ex);

await dialogService.ShowInfoAsync("Info", "Everything is fine");

bool confirmed = await dialogService.ShowConfirmationAsync("Confirm", "Are you sure?");
```

---

### 4️⃣ Using the `MessageBox` static class

```csharp
await MessageBox.ShowErrorAsync("This is an error");

bool confirmed = await MessageBox.ShowConfirmationAsync("Proceed?", "Please confirm this action");
```

*Optional:* initialize `MessageBox` during startup:

```csharp
MessageBox.Initialize(serviceProvider);
```

---

## 🖼️ Available Dialogs

| Dialog Type      | Appearance | Features |
|------------------|------------|----------|
| **ErrorDialog**  | Red header, Exception details expandable | Exception stack trace, Copy to clipboard |
| **WarningDialog**| Yellow header | Simple warning |
| **InfoDialog**   | Blue header | Simple info |
| **ValidationErrorDialog** | Errors + Warnings summary | Grouped lists |
| **ConfirmationDialog** | Confirm / Cancel buttons | Customizable labels |

---

## 🧪 Testing Support

- Fully testable in **headless mode**  
- Integration tests provided in `Avalonia.Accelerate.Dialogs.Tests`

Use the provided `TestDialogHelper` for manual dialog testing:

```csharp
await TestDialogHelper.ShowTestDialog(myDialog);
```

---

## Example Screenshot

*(Add a screenshot here of the dialogs in your app or from the test project!)*

---

## Compatibility

- ✅ .NET 8.0+
- ✅ Avalonia UI `11.3.x`+
- ✅ Windows / Linux / macOS

---

## Roadmap

✅ Basic dialogs  
✅ Fluent-styled visuals  
✅ Dependency injection  
✅ Testable design  
🚧 Theme customization support  
🚧 Async file/folder dialogs integration  
🚧 Localization support  

---

## Contributing

Pull requests are welcome! If you spot an issue or want to improve the library, feel free to submit a PR or open an issue.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Credits

Part of the **Avalonia.Accelerate** suite of libraries.
