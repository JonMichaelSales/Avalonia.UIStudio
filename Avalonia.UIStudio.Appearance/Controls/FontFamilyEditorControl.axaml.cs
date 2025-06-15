using System.Collections.ObjectModel;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class FontFamilyEditorControl : ValidatableEditorControlBase<FontFamilyEditorControl>
{
    public static readonly StyledProperty<FontFamily> ValueProperty =
        AvaloniaProperty.Register<FontFamilyEditorControl, FontFamily>(nameof(Value));

    public static readonly StyledProperty<ObservableCollection<FontFamily>> AvailableFontFamiliesProperty =
        AvaloniaProperty.Register<FontFamilyEditorControl, ObservableCollection<FontFamily>>(nameof(AvailableFontFamilies));

    public static readonly StyledProperty<FontFamily?> SuggestedValueProperty =
        AvaloniaProperty.Register<FontFamilyEditorControl, FontFamily?>(nameof(SuggestedValue));

    public FontFamilyEditorControl()
    {
        InitializeComponent();

        // Default font family list � you can customize or load from FontManager
        var fonts = FontManager.Current.SystemFonts.ToList();
        AvailableFontFamilies = new ObservableCollection<FontFamily>();
        foreach (var font in fonts) AvailableFontFamilies.Add(font.Name);
    }

    public FontFamily Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public ObservableCollection<FontFamily> AvailableFontFamilies
    {
        get => GetValue(AvailableFontFamiliesProperty);
        set => SetValue(AvailableFontFamiliesProperty, value);
    }

    public FontFamily? SuggestedValue
    {
        get => GetValue(SuggestedValueProperty);
        set => SetValue(SuggestedValueProperty, value);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}