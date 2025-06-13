using Avalonia.UIStudio.Dialogs.Tests.Fixtures;
using Avalonia.Headless;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]
namespace Avalonia.UIStudio.Dialogs.Tests.Fixtures
{
    public class TestAppBuilder
    {
        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                      .UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }
}