using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;
using Avalonia.UIStudio.Appearance.Services.ValidationRules;
using Avalonia.UIStudio.Appearance.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using PropertyGrid = Avalonia.UIStudio.Appearance.Controls.PropertyGrid;

namespace Avalonia.UIStudio.Appearance.Extensions;

/// <summary>
///     Provides extension methods for registering skin manager services in an Avalonia application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds skin manager and related services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddSkinManagerServices(this IServiceCollection services)
    {
        // Logging (you can customize levels elsewhere if needed)
        services.AddLogging();


        // Application abstraction
        services.AddSingleton<IApplication, ApplicationWrapper>();
        services.AddSingleton<ISkinImportExportService, SkinImportExportService>();
        // Core services
        services.AddSingleton<ISkinLoaderService, SkinLoaderService>();
        services.AddSingleton<ISkinManager, SkinManager>();

        // Skin inheritance manager
        services.AddSingleton<SkinInheritanceManager>();

        // Validation rules
        services.AddSingleton<ISkinValidationRule, BorderValidationRule>();
        services.AddSingleton<ISkinValidationRule, ColorContrastValidationRule>();
        services.AddSingleton<ISkinValidationRule, NameValidationRule>();
        services.AddSingleton<ISkinValidationRule, AccessibilityValidationRule>();

        // ViewModels
        services.AddTransient<SkinSettingsViewModel>();
        services.AddTransient<QuickSkinSwitcherViewModel>();
        services.AddTransient<PropertyGrid>();

        return services;
    }
}