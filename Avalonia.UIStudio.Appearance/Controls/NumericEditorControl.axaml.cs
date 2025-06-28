using Avalonia.Markup.Xaml;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class NumericEditorControl : ValidatableEditorControlBase<NumericEditorControl>
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<NumericEditorControl, double>(nameof(Value));

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<NumericEditorControl, double>(nameof(Maximum), 100.0);

    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<NumericEditorControl, double>(nameof(Minimum));

    public static readonly StyledProperty<object?> SuggestedValueProperty =
        AvaloniaProperty.Register<NumericEditorControl, object?>(nameof(SuggestedValue));

    public NumericEditorControl()
    {
        InitializeComponent();
    }

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public object? SuggestedValue
    {
        get => GetValue(SuggestedValueProperty);
        set => SetValue(SuggestedValueProperty, value);
    }
}