using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Controls
{
    public partial class NumericEditorControl : ValidatableEditorControlBase<NumericEditorControl>
    {
        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<NumericEditorControl, double>(nameof(Value));

        public double Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
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
