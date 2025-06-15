// Fixtures/TestAppBuilder.cs

using Avalonia.UIStudio.Icons.Tests.Fixtures;
using Avalonia.Headless;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]
namespace Avalonia.UIStudio.Icons.Tests.Fixtures
{
    public class TestAppBuilder
    {
        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }
}