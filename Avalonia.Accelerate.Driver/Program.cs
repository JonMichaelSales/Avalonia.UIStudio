using Avalonia.ReactiveUI;
using Avalonia.Accelerate.Dialogs.Extensions;
using System;
using Avalonia.Accelerate.Appearance.Extensions;
using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Accelerate.Driver.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.Accelerate.Driver
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
