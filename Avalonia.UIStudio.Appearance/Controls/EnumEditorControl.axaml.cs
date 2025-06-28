using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class EnumEditorControl : ValidatableEditorControlBase<EnumEditorControl>
{
    public static readonly StyledProperty<Enum?> ValueProperty =
        AvaloniaProperty.Register<EnumEditorControl, Enum?>(nameof(Value));

    public EnumEditorControl()
    {
        InitializeComponent();
    }

    public Enum? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}