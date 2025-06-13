using Avalonia.UIStudio.Dialogs.Interfaces;
using Avalonia.UIStudio.Dialogs.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.UIStudio.Dialogs.Extensions
{
    /// <summary>
    /// Provides extension methods for registering dialog services in an Avalonia application.
    /// </summary>
    public static class DialogServiceExtensions
    {
        public static AppBuilder UseDialogServices(this AppBuilder builder,
            Action<IServiceCollection>? configure = null)
        {
            return builder.AfterSetup(_ =>
            {
                var services = new ServiceCollection();

                // ✅ Register logging
                services.AddLogging(); // You can customize this if needed

                // Core dialog services
                services.AddSingleton<IDialogService, DialogService>();

                // Allow app-specific registration
                configure?.Invoke(services);

                DialogServiceLocator.ServiceProvider = services.BuildServiceProvider();
            });
        }

        public static IServiceCollection AddDialogServices(this IServiceCollection services)
        {
            // Register logging if needed (optional — you can move this elsewhere if you want centralized logging)
            services.AddLogging();

            // Core dialog services
            services.AddSingleton<IDialogService, DialogService>();

            return services;
        }


    }
}