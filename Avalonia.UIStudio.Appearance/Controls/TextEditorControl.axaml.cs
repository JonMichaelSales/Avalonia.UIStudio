using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Controls
{
    public partial class TextEditorControl : UserControl
    {
        public static readonly StyledProperty<ValidatedProperty?> ValidatedPropertyProperty =
            AvaloniaProperty.Register<TextEditorControl, ValidatedProperty?>(nameof(ValidatedProperty));

        public ValidatedProperty? ValidatedProperty
        {
            get => GetValue(ValidatedPropertyProperty);
            set => SetValue(ValidatedPropertyProperty, value);
        }

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<TextEditorControl, string?>(nameof(PropertyName));

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly StyledProperty<string> ValueProperty =
            AvaloniaProperty.Register<TextEditorControl, string>(nameof(Value));

        public string Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly StyledProperty<string> LabelProperty =
            AvaloniaProperty.Register<TextEditorControl, string>(nameof(Label));

        public string Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly StyledProperty<bool> IsReadOnlyProperty =
            AvaloniaProperty.Register<TextEditorControl, bool>(nameof(IsReadOnly));

        public bool IsReadOnly
        {
            get => GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public TextEditorControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
