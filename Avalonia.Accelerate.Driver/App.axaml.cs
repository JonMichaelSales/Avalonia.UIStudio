using Avalonia.Accelerate.Dialogs.Services;
using Avalonia.Accelerate.Driver.ViewModels;
using Avalonia.Accelerate.Driver.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using Avalonia.Accelerate.Appearance.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.Accelerate.Driver
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; set; } = default!;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                AppBuilderExtensions.InitializeSkinManager(); // first!

                // Do NOT overwrite Services here!
                // Services = DialogServiceLocator.ServiceProvider; // REMOVE THIS LINE

                if (Avalonia.Application.Current != null)
                {
                    var vm = App.Services.GetRequiredService<MainWindowViewModel>();
                    desktop.MainWindow = new MainWindow { DataContext = vm };
                }
            }

            base.OnFrameworkInitializationCompleted();
        }
    }

}