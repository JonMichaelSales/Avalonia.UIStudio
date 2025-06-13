//using Avalonia;
//using Avalonia.Accelerate.Dialogs.Extensions;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace Avalonia.Accelerate.Dialogs.Extensions
//{
//    /// <summary>
//    /// Provides extension methods for configuring dialog services with the Avalonia AppBuilder.
//    /// </summary>
//    public static class DialogAppBuilderExtensions
//    {
//        /// <summary>
//        /// Adds dialog services to the Avalonia application using dependency injection.
//        /// </summary>
//        /// <param name="builder">The AppBuilder instance</param>
//        /// <param name="configureServices">Action to configure services including dialog services</param>
//        /// <returns>The AppBuilder instance for method chaining</returns>
//        public static AppBuilder ConfigureDialogServices(this AppBuilder builder, Action<IServiceCollection> configureServices)
//        {
//            return builder.AfterSetup(app =>
//            {
//                var services = new ServiceCollection();
//                configureServices(services);

//                // Build the service provider and store it somewhere accessible
//                // Note: You'll need to decide how to make this available to your app
//                var serviceProvider = services.BuildServiceProvider();

//                // You could store this in App.Current or use a service locator pattern
//                // Example: ((App)app).ServiceProvider = serviceProvider;
//            });
//        }

//        /// <summary>
//        /// Adds dialog services with default configuration to the Avalonia application.
//        /// </summary>
//        /// <param name="builder">The AppBuilder instance</param>
//        /// <param name="initializeMessageBox">Whether to initialize the static MessageBox class</param>
//        /// <returns>The AppBuilder instance for method chaining</returns>
//        public static AppBuilder UseDialogServices(this AppBuilder builder, bool initializeMessageBox = true)
//        {
//            return builder.ConfigureDialogServices(services =>
//            {
//                if (initializeMessageBox)
//                {
//                    services.AddDialogServicesWithMessageBox();
//                }
//                else
//                {
//                    services.AddDialogServices();
//                }
//            });
//        }

//        /// <summary>
//        /// Adds dialog services with custom logging configuration.
//        /// </summary>
//        /// <param name="builder">The AppBuilder instance</param>
//        /// <param name="configureLogging">Action to configure logging</param>
//        /// <param name="initializeMessageBox">Whether to initialize the static MessageBox class</param>
//        /// <returns>The AppBuilder instance for method chaining</returns>
//        public static AppBuilder UseDialogServices(this AppBuilder builder, Action<ILoggingBuilder> configureLogging, bool initializeMessageBox = true)
//        {
//            return builder.ConfigureDialogServices(services =>
//            {
//                if (initializeMessageBox)
//                {
//                    services.AddDialogServicesWithMessageBox(configureLogging);
//                }
//                else
//                {
//                    services.AddDialogServices(configureLogging);
//                }
//            });
//        }
//    }
//}