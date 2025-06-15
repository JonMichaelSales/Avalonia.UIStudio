using Avalonia.Markup.Xaml;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class BoolEditorControl : ValidatableEditorControlBase<TextEditorControl>
{
    public static readonly StyledProperty<bool> ValueProperty =
        AvaloniaProperty.Register<TextEditorControl, bool>(nameof(Value));

    public BoolEditorControl()
    {
        InitializeComponent();
    }

    public bool Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}