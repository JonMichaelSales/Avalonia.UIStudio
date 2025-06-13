using System;
using Avalonia.ReactiveUI;
using Avalonia.UIStudio.Appearance.Extensions;
using Avalonia.UIStudio.Dialogs.Extensions;
using Avalonia.UIStudio.Driver.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.UIStudio.Driver
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .UseSkinManager(services =>
                {
                    services.AddDialogServices(); // ✅ adds dialog services to main container
                    services.AddSkinManagerServices();
                    
                    services.AddTransient<MainWindowViewModel>();
                }, onBuilt: sp => App.Services = sp)
                .LogToTrace()
                .UseReactiveUI();

    }
}
