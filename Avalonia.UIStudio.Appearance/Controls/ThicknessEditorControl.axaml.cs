using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.UIStudio.Appearance.Controls;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class ThicknessEditorControl : ValidatableEditorControlBase<ThicknessEditorControl>
{
    public static readonly StyledProperty<Thickness> ValueProperty =
        AvaloniaProperty.Register<ThicknessEditorControl, Thickness>(nameof(Value));

    public ThicknessEditorControl()
    {
        InitializeComponent();
    }
    
    public string ValueString => $"{Value.Left}, {Value.Top}, {Value.Right}, {Value.Bottom}";

    public Thickness Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}