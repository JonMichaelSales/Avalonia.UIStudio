using Avalonia.Headless.XUnit;
using Avalonia.UIStudio.Appearance.Extensions;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Services;
using Avalonia.UIStudio.Appearance.Services.ValidationRules;
using Avalonia.UIStudio.Appearance.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.UIStudio.Appearance.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [AvaloniaFact]
    public void AddSkinManagerServices_RegistersExpectedServices()
    {
        var services = new ServiceCollection();

        services.AddSkinManagerServices();

        var provider = services.BuildServiceProvider();

        // Core services
        Assert.NotNull(provider.GetService<ISkinLoaderService>());
        Assert.NotNull(provider.GetService<ISkinManager>());
        Assert.NotNull(provider.GetService<SkinInheritanceManager>());

        // Validation rules — all should be registered
        var rules = provider.GetServices<ISkinValidationRule>().ToList();
        Assert.Contains(rules, r => r is BorderValidationRule);
        Assert.Contains(rules, r => r is ColorContrastValidationRule);
        Assert.Contains(rules, r => r is NameValidationRule);
        Assert.Contains(rules, r => r is AccessibilityValidationRule);

        // ViewModels
        Assert.NotNull(provider.GetService<SkinSettingsViewModel>());
        Assert.NotNull(provider.GetService<QuickSkinSwitcherViewModel>());
    }
}