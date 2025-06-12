using Avalonia;
using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Accelerate.Appearance.Services.ValidationRules;
using Avalonia.Accelerate.Appearance.ViewModels;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Avalonia.Accelerate.Appearance.Extensions
{
    /// <summary>
    /// Provides extension methods for registering skin manager services in an Avalonia application.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds skin manager and related services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddSkinManagerServices(this IServiceCollection services)
        {
            // Logging (you can customize levels elsewhere if needed)
            services.AddLogging();


            // Application abstraction
            services.AddSingleton<IApplication, ApplicationWrapper>();
            services.AddSingleton<ISkinImportExportService,SkinImportExportService>();
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

            return services;
        }
    }
}
