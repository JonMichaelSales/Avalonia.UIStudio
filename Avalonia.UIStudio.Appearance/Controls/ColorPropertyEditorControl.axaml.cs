using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Controls
{
    public partial class ColorEditorControl : UserControl
    {
        public static readonly StyledProperty<ValidatedProperty?> ValidatedPropertyProperty =
            AvaloniaProperty.Register<ColorEditorControl, ValidatedProperty?>(nameof(ValidatedProperty));

        public ValidatedProperty? ValidatedProperty
        {
            get => GetValue(ValidatedPropertyProperty);
            set => SetValue(ValidatedPropertyProperty, value);
        }

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<ColorEditorControl, string?>(nameof(PropertyName));

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly StyledProperty<Color> ValueProperty =
            AvaloniaProperty.Register<ColorEditorControl, Color>(nameof(Value));

        public Color Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly StyledProperty<string> LabelProperty =
            AvaloniaProperty.Register<ColorEditorControl, string>(nameof(Label));

        public string Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly StyledProperty<bool> IsEditModeProperty =
            AvaloniaProperty.Register<ColorEditorControl, bool>(nameof(IsEditMode));

        public bool IsEditMode
        {
            get => GetValue(IsEditModeProperty);
            set => SetValue(IsEditModeProperty, value);
        }

        public ColorEditorControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
