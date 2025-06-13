using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Controls
{
    public partial class TextEditorControl : ValidatableEditorControlBase<TextEditorControl>
    {
        public static readonly StyledProperty<string> ValueProperty =
            AvaloniaProperty.Register<TextEditorControl, string>(nameof(Value));

        public string Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
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
