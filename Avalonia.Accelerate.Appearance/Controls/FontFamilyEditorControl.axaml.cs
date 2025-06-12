using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Accelerate.Appearance.Model;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Media;

namespace Avalonia.Accelerate.Appearance.Controls
{
    public partial class FontFamilyEditorControl : UserControl
    {
        public static readonly StyledProperty<ValidatedProperty?> ValidatedPropertyProperty =
            AvaloniaProperty.Register<FontFamilyEditorControl, ValidatedProperty?>(nameof(ValidatedProperty));

        public ValidatedProperty? ValidatedProperty
        {
            get => GetValue(ValidatedPropertyProperty);
            set => SetValue(ValidatedPropertyProperty, value);
        }

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<FontFamilyEditorControl, string?>(nameof(PropertyName));

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly StyledProperty<string> ValueProperty =
            AvaloniaProperty.Register<FontFamilyEditorControl, string>(nameof(Value));

        public string Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly StyledProperty<string> LabelProperty =
            AvaloniaProperty.Register<FontFamilyEditorControl, string>(nameof(Label));

        public string Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly StyledProperty<bool> IsReadOnlyProperty =
            AvaloniaProperty.Register<FontFamilyEditorControl, bool>(nameof(IsReadOnly));

        public bool IsReadOnly
        {
            get => GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<string>> AvailableFontFamiliesProperty =
            AvaloniaProperty.Register<FontFamilyEditorControl, ObservableCollection<string>>(nameof(AvailableFontFamilies));

        public ObservableCollection<string> AvailableFontFamilies
        {
            get => GetValue(AvailableFontFamiliesProperty);
            set => SetValue(AvailableFontFamiliesProperty, value);
        }

        public static readonly StyledProperty<object?> SuggestedValueProperty =
            AvaloniaProperty.Register<FontFamilyEditorControl, object?>(nameof(SuggestedValue));

        public object? SuggestedValue
        {
            get => GetValue(SuggestedValueProperty);
            set => SetValue(SuggestedValueProperty, value);
        }

        public FontFamilyEditorControl()
        {
            InitializeComponent();

            // Default font family list — you can customize or load from FontManager
            var fonts = FontManager.Current.SystemFonts.ToList();
            AvailableFontFamilies = new ObservableCollection<string>();
            foreach (var font in fonts)
            {
                AvailableFontFamilies.Add(font.Name);
            }
            

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
