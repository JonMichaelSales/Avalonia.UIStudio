using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Controls
{
    public partial class NumericEditorControl : UserControl
    {
        public static readonly StyledProperty<ValidatedProperty?> ValidatedPropertyProperty =
            AvaloniaProperty.Register<NumericEditorControl, ValidatedProperty?>(nameof(ValidatedProperty));

        public ValidatedProperty? ValidatedProperty
        {
            get => GetValue(ValidatedPropertyProperty);
            set => SetValue(ValidatedPropertyProperty, value);
        }

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<NumericEditorControl, string?>(nameof(PropertyName));

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<NumericEditorControl, double>(nameof(Value));

        public double Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly StyledProperty<string> LabelProperty =
            AvaloniaProperty.Register<NumericEditorControl, string>(nameof(Label));

        public string Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly StyledProperty<bool> IsReadOnlyProperty =
            AvaloniaProperty.Register<NumericEditorControl, bool>(nameof(IsReadOnly));

        public bool IsReadOnly
        {
            get => GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly StyledProperty<double> MaximumProperty =
            AvaloniaProperty.Register<NumericEditorControl, double>(nameof(Maximum), 100.0);

        public double Maximum
        {
            get => GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly StyledProperty<double> MinimumProperty =
            AvaloniaProperty.Register<NumericEditorControl, double>(nameof(Minimum), 0.0);

        public double Minimum
        {
            get => GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly StyledProperty<object?> SuggestedValueProperty =
            AvaloniaProperty.Register<NumericEditorControl, object?>(nameof(SuggestedValue));

        public object? SuggestedValue
        {
            get => GetValue(SuggestedValueProperty);
            set => SetValue(SuggestedValueProperty, value);
        }

        public NumericEditorControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
