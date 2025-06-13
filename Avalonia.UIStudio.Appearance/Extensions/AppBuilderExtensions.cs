using Microsoft.Extensions.DependencyInjection;
using System;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;
using Avalonia.UIStudio.Appearance.ViewModels;
using Avalonia.UIStudio.Appearance.Views;

namespace Avalonia.UIStudio.Appearance.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring and integrating the AvaloniaSkinManager 
    /// into an Avalonia application using the <see cref="AppBuilder"/>.
    /// </summary>
    /// <remarks>
    /// This class contains methods to enable the AvaloniaSkinManager with default or custom configurations.
    /// It simplifies the setup process by allowing developers to chain skin manager configuration
    /// into the application initialization pipeline.
    /// </remarks>
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Static service provider holder for accessing services throughout the application
        /// </summary>
        private static IServiceProvider? _serviceProvider;

        /// <summary>
        /// Action to be executed when application is ready
        /// </summary>
        private static Action? _initializationAction;

        /// <summary>
        /// Gets the current service provider
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get => _serviceProvider ?? throw new InvalidOperationException("Service provider not initialized. Ensure UseSkinManager() was called during application setup.");
            private set => _serviceProvider = value;
        }

        /// <summary>
        /// Adds AvaloniaSkinManager to the application with dependency injection support
        /// </summary>
        /// <param name="builder">The AppBuilder instance</param>
        /// <param name="configureServices">Optional service configuration action</param>
        /// <returns>The AppBuilder instance for method chaining</returns>
        public static AppBuilder UseSkinManager(this AppBuilder builder, Action<IServiceCollection>? configureServices = null, Action<IServiceProvider>? onBuilt = null)
        {
            return builder.AfterSetup(appBuilder =>
            {
                // Set up dependency injection
                var services = new ServiceCollection();

                // Add skin manager services
                services.AddSkinManagerServices();
                services.AddSingleton<IApplication>(_ => (IApplication)(Avalonia.Application.Current ?? throw new InvalidOperationException("Application.Current is not available.")));
                services.AddTransient<IStylesCollection, AvaloniaStylesWrapper>();
                services.AddSingleton<ISkinLoaderService, SkinLoaderService>();
                services.AddTransient<ISkinImportExportService, SkinImportExportService>();
                services.AddSingleton<IQuickSkinSwitcherViewModel,QuickSkinSwitcherViewModel>();
                services.AddTransient<SkinSettingsViewModel>();
                services.AddTransient<SkinSettingsDialog>();
                services.AddSingleton<ISkinManager, SkinManager>();
                // Allow additional service configuration
                configureServices?.Invoke(services);

                // Build and store the service provider
                ServiceProvider = services.BuildServiceProvider();
                onBuilt?.Invoke(ServiceProvider); // Notify caller — they can assign App.Services here if they want


                // Store initialization action to be called when application is ready
                _initializationAction = () =>
                {
                    try
                    {
                        var skinManager = (SkinManager)ServiceProvider.GetRequiredService<ISkinManager>();
                        SkinManager.Instance = skinManager;
                        skinManager.LoadSavedSkin();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error initializing skin manager: {ex.Message}");
                    }
                };
            });
        }

        /// <summary>
        /// Adds AvaloniaSkinManager with custom configuration (legacy method for backward compatibility)
        /// </summary>
        /// <param name="builder">The AppBuilder instance</param>
        /// <param name="configure">Configuration action</param>
        /// <returns>The AppBuilder instance for method chaining</returns>
        public static AppBuilder UseSkinManager(this AppBuilder builder, System.Action<SkinManager> configure)
        {
            return builder.AfterSetup(appBuilder =>
            {
                // Set up dependency injection
                var services = new ServiceCollection();
                services.AddSkinManagerServices();
                ServiceProvider = services.BuildServiceProvider();
                
                // Store initialization action to be called when application is ready
                _initializationAction = () =>
                {
                    try
                    {
                        var skinManager = ServiceProvider.GetRequiredService<ISkinManager>() as SkinManager;
                        if (skinManager != null)
                        {
                            configure(skinManager);
                            skinManager.LoadSavedSkin();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error configuring skin manager: {ex.Message}");
                    }
                };
            });
        }

        /// <summary>
        /// Internal method to be called by the Application when it's ready to initialize the skin manager
        /// This should be called from Application.OnFrameworkInitializationCompleted()
        /// </summary>
        public static void InitializeSkinManager()
        {
            _initializationAction?.Invoke();
            _initializationAction = null; // Clear after execution
        }

        /// <summary>
        /// Gets a service from the application's service provider
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        /// <exception cref="InvalidOperationException">Thrown when the service provider is not available</exception>
        public static T GetRequiredService<T>() where T : notnull
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Gets a service from the application's service provider
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance, or null if not found</returns>
        public static T? GetService<T>() where T : class
        {
            try
            {
                return ServiceProvider.GetService<T>();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Extension method for Application to get services (for compatibility)
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <param name="app">The application instance</param>
        /// <returns>The service instance</returns>
        public static T GetRequiredService<T>(this Application app) where T : notnull
        {
            return GetRequiredService<T>();
        }

        /// <summary>
        /// Extension method for Application to get services (for compatibility)
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <param name="app">The application instance</param>
        /// <returns>The service instance, or null if not found</returns>
        public static T? GetService<T>(this Application app) where T : class
        {
            return GetService<T>();
        }

    }
}