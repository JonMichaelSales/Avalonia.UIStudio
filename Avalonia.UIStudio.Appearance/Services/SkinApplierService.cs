using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;

public class SkinApplierService : ISkinApplierService
{
    private readonly IApplication _application;
    private readonly IStylesCollection _styles;
    private Skin? _currentSkin;
    public Skin? CurrentSkin => _currentSkin;

    private readonly List<IResourceProvider> _appliedControlThemeDictionaries = new();
    private readonly List<IStyle> _appliedControlThemes = new();

    public event EventHandler? SkinChanged;

    public SkinApplierService(IApplication application)
    {
        _application = application;
        _styles = _application.AppStyles ?? throw new InvalidOperationException("Application.AppStyles is null.");
    }

    public void ApplySkin(Skin? skin)
    {
        _currentSkin = skin ?? new Skin();

        try
        {
            UpdateResources();
            UpdateTypographyResources();
            ApplyControlThemes();
            ForceVisualUpdate();

            SkinChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying skin: {ex.Message}");
        }
    }

    private void UpdateResources()
    {
        var dict = _application.Resources;
        if (dict == null || _currentSkin == null) return;

        void Set(string key, object value) => dict[key] = value;

        UpdateBrush(dict, "PrimaryColorBrush", _currentSkin.PrimaryColor);
        UpdateBrush(dict, "SecondaryColorBrush", _currentSkin.SecondaryColor);
        UpdateBrush(dict, "AccentBlueBrush", _currentSkin.AccentColor);
        UpdateBrush(dict, "GunMetalDarkBrush", _currentSkin.PrimaryColor);
        UpdateBrush(dict, "GunMetalMediumBrush", _currentSkin.SecondaryColor);
        UpdateBrush(dict, "GunMetalLightBrush", _currentSkin.SecondaryBackground);
        UpdateBrush(dict, "BackgroundBrush", _currentSkin.PrimaryBackground);
        UpdateBrush(dict, "BackgroundLightBrush", _currentSkin.SecondaryBackground);

        var dark = new Color(_currentSkin.PrimaryBackground.A,
            (byte)(_currentSkin.PrimaryBackground.R * 0.8),
            (byte)(_currentSkin.PrimaryBackground.G * 0.8),
            (byte)(_currentSkin.PrimaryBackground.B * 0.8));

        UpdateBrush(dict, "BackgroundDarkBrush", dark);
        UpdateBrush(dict, "TextPrimaryBrush", _currentSkin.PrimaryTextColor);
        UpdateBrush(dict, "TextSecondaryBrush", _currentSkin.SecondaryTextColor);
        UpdateBrush(dict, "BorderBrush", _currentSkin.BorderColor);
        UpdateBrush(dict, "ErrorBrush", _currentSkin.ErrorColor);
        UpdateBrush(dict, "WarningBrush", _currentSkin.WarningColor);
        UpdateBrush(dict, "SuccessBrush", _currentSkin.SuccessColor);

        Set("DefaultFontFamily", _currentSkin.FontFamily);
        Set("FontSizeSmall", _currentSkin.FontSizeSmall);
        Set("FontSizeMedium", _currentSkin.FontSizeMedium);
        Set("FontSizeLarge", _currentSkin.FontSizeLarge);
        Set("DefaultFontWeight", _currentSkin.FontWeight);
        Set("BorderThickness", _currentSkin.BorderThickness);
        Set("CornerRadius", new CornerRadius(_currentSkin.BorderRadius));
    }

    private void UpdateTypographyResources()
    {
        if (_currentSkin?.Typography is not { } t) return;

        var r = _application.Resources;
        void Set(string k, object v) => r[k] = v;

        Set("DisplayLargeFontSize", t.DisplayLarge);
        Set("DisplayMediumFontSize", t.DisplayMedium);
        Set("DisplaySmallFontSize", t.DisplaySmall);
        Set("HeadlineLargeFontSize", t.HeadlineLarge);
        Set("HeadlineMediumFontSize", t.HeadlineMedium);
        Set("HeadlineSmallFontSize", t.HeadlineSmall);
        Set("TitleLargeFontSize", t.TitleLarge);
        Set("TitleMediumFontSize", t.TitleMedium);
        Set("TitleSmallFontSize", t.TitleSmall);
        Set("LabelLargeFontSize", t.LabelLarge);
        Set("LabelMediumFontSize", t.LabelMedium);
        Set("LabelSmallFontSize", t.LabelSmall);
        Set("BodyLargeFontSize", t.BodyLarge);
        Set("BodyMediumFontSize", t.BodyMedium);
        Set("BodySmallFontSize", t.BodySmall);

        Set("HeaderFontFamily", _currentSkin.HeaderFontFamily);
        Set("BodyFontFamily", _currentSkin.BodyFontFamily);
        Set("MonospaceFontFamily", _currentSkin.MonospaceFontFamily);
        Set("DefaultLineHeight", _currentSkin.LineHeight);
        Set("DefaultLetterSpacing", _currentSkin.LetterSpacing);
    }

    private void ApplyControlThemes()
    {
        if (_currentSkin == null) return;

        foreach (var style in _appliedControlThemes)
            _styles.Remove(style);
        _appliedControlThemes.Clear();

        foreach (var d in _appliedControlThemeDictionaries)
            _application.Resources.MergedDictionaries.Remove(d);
        _appliedControlThemeDictionaries.Clear();

        foreach (var kvp in _currentSkin.ControlThemeUris)
        {
            var baseUri = new Uri($"avares://Avalonia.UIStudio.Appearance/Skins/{_currentSkin.Name}/");
            var include = new ResourceInclude(baseUri) { Source = new Uri(kvp, UriKind.Relative) };

            _application.Resources.MergedDictionaries.Add(include);
            _appliedControlThemeDictionaries.Add(include);
        }

        foreach (var styleUri in _currentSkin.StyleUris)
        {
            if (styleUri is null) continue;

            var style = new StyleInclude(new Uri("avares://Avalonia.UIStudio.Appearance/"))
            {
                Source = new Uri(styleUri)
            };

            _styles.Add(style);
            _appliedControlThemes.Add(style);
        }
    }

    private void ForceVisualUpdate()
    {
        if (_application.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        foreach (var window in desktop.Windows)
        {
            window.InvalidateVisual();
            InvalidateRecursive(window);
        }
    }

    private void InvalidateRecursive(Control control)
    {
        control.InvalidateVisual();

        if (control is Panel panel)
            foreach (var child in panel.Children)
                InvalidateRecursive(child);
        else if (control is ContentControl cc && cc.Content is Control c)
            InvalidateRecursive(c);
        else if (control is ItemsControl ic && ic.ItemsPanelRoot is Control ip)
            InvalidateRecursive(ip);
    }

    private void UpdateBrush(IResourceDictionary dict, string key, Color color)
    {
        if (dict.TryGetValue(key, out var existing) && existing is SolidColorBrush brush)
            brush.Color = color;
        else
            dict[key] = new SolidColorBrush(color);
    }
}
