using Avalonia.Markup.Xaml;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class TextEditorControl : ValidatableEditorControlBase<TextEditorControl>
{
    public static readonly StyledProperty<string> ValueProperty =
        AvaloniaProperty.Register<TextEditorControl, string>(nameof(Value));

    public TextEditorControl()
    {
        InitializeComponent();
    }

    public string Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}