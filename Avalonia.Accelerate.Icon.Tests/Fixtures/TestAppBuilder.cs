// Fixtures/TestAppBuilder.cs
using Avalonia;
using Avalonia.Accelerate.Icons.Tests.Fixtures;
using Avalonia.Headless;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]
namespace Avalonia.Accelerate.Icons.Tests.Fixtures
{
    public class TestAppBuilder
    {
        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }
}