using Avalonia.Accelerate.Appearance.Extensions;
using Avalonia.Headless.XUnit;
using Avalonia.Markup.Xaml.Styling;

namespace Avalonia.Accelerate.Appearance.Tests.Extensions;

public class ApplicationExtensionsTests
{
    [AvaloniaFact]
    public void IncludeThemeManagerStyles_AddsResourceDictionary()
    {
        var app = Application.Current!;
        int initialCount = app.Resources.MergedDictionaries.Count;

        app.IncludeThemeManagerStyles();

        Assert.True(app.Resources.MergedDictionaries.Count > initialCount);

        var addedDict = app.Resources.MergedDictionaries
            .OfType<ResourceInclude>()
            .FirstOrDefault(d => d.Source != null && d.Source.ToString()!.Contains("Skins/CustomThemes.axaml"));

        Assert.NotNull(addedDict);
    }
}