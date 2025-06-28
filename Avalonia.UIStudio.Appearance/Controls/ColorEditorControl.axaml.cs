using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class ColorEditorControl : ValidatableEditorControlBase<ColorEditorControl>
{
    public static readonly StyledProperty<Color> ValueProperty =
        AvaloniaProperty.Register<ColorEditorControl, Color>(nameof(Value));

    public ColorEditorControl()
    {
        InitializeComponent();
    }

    public Color Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}