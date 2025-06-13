using Avalonia.Headless.XUnit;
using Avalonia.Markup.Xaml.Styling;

namespace Avalonia.UIStudio.Appearance.Tests.Extensions;

public class ApplicationExtensionsTests
{
    [AvaloniaFact]
    public void IncludeSkinManagerStyles_AddsResourceDictionary()
    {
        var app = Application.Current!;
        int initialCount = app.Resources.MergedDictionaries.Count;

        app.Initialize();

        Assert.True(app.Resources.MergedDictionaries.Count > initialCount);

        var addedDict = app.Resources.MergedDictionaries
            .OfType<ResourceInclude>()
            .FirstOrDefault(d => d.Source != null && d.Source.ToString()!.Contains("Skins/AppSkin.axaml"));

        Assert.NotNull(addedDict);
    }
}