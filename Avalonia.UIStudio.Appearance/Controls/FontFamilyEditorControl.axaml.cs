using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Controls
{
    public partial class FontFamilyEditorControl : ValidatableEditorControlBase<FontFamilyEditorControl>
    {
       
        public static readonly StyledProperty<string> ValueProperty =
            AvaloniaProperty.Register<FontFamilyEditorControl, string>(nameof(Value));

        public string Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
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

            // Default font family list � you can customize or load from FontManager
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
