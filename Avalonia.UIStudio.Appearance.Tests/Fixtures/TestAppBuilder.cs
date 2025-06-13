using Avalonia.Headless;
using Avalonia.UIStudio.Appearance.Tests.Fixtures;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace Avalonia.UIStudio.Appearance.Tests.Fixtures;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}