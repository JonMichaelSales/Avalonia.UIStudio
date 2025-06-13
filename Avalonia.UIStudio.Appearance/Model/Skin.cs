using Avalonia.Media;

namespace Avalonia.UIStudio.Appearance.Model
{
    /// <summary>
    /// Represents a customizable skin for an Avalonia application, defining colors, fonts, and other visual properties.
    /// </summary>
    /// <remarks>
    /// The <see cref="Skin"/> class provides a set of properties to define the appearance of an application, 
    /// including primary and secondary colors, background colors, text colors, font settings, and additional UI properties.
    /// It also includes functionality to convert colors to brushes and a default constructor for initializing a dark skin.
    /// </remarks>
    public class Skin
    {
        public string Description { get; set; }
        // Basic colors
        /// <summary>
        /// Gets or sets the primary color of the skin.
        /// </summary>
        /// <remarks>
        /// This color is typically used as the main color for UI elements and serves as a foundation
        /// for the overall skin design. The default value for the dark skin is GunMetal Dark (#343B48).
        /// </remarks>
        public Color PrimaryColor { get; set; }
        /// <summary>
        /// Gets or sets the secondary color used in the skin.
        /// </summary>
        /// <remarks>
        /// This color is typically used for medium-tone elements within the skin.
        /// The default value for the dark skin is <c>#3D4654</c> (GunMetal Medium).
        /// </remarks>
        public Color SecondaryColor { get; set; }
        /// <summary>
        /// Gets or sets the accent color of the skin.
        /// </summary>
        /// <remarks>
        /// The accent color is used to highlight key elements in the user interface,
        /// providing a visually distinct color that complements the primary and secondary colors.
        /// </remarks>
        public Color AccentColor { get; set; }
        // Backgrounds
        /// <summary>
        /// Gets or sets the primary background color of the skin.
        /// </summary>
        /// <value>
        /// A <see cref="Color"/> representing the primary background color.
        /// </value>
        public Color PrimaryBackground { get; set; }
        /// <summary>
        /// Gets or sets the secondary background color of the skin.
        /// </summary>
        /// <remarks>
        /// This property defines the secondary background color used in the skin. 
        /// It is typically a lighter shade of the primary background color to provide contrast and visual hierarchy.
        /// </remarks>
        public Color SecondaryBackground { get; set; }
        /// <summary>
        /// Gets or sets the primary text color used in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="Color"/> representing the primary text color. 
        /// The default value is white (<c>#FFFFFF</c>).
        /// </value>
        public Color PrimaryTextColor { get; set; }
        /// <summary>
        /// Gets or sets the color used for secondary text elements in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="Color"/> representing the secondary text color. The default value is a light gray color (#CCCCCC).
        /// </value>
        public Color SecondaryTextColor { get; set; }
        // Font properties
        /// <summary>
        /// Gets or sets the font family used for text rendering in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="FontFamily"/> representing the font family. The default value is "Segoe UI, San Francisco, Helvetica, Arial, sans-serif".
        /// </value>
        public FontFamily FontFamily { get; set; }
        /// <summary>
        /// Gets or sets the font size for small text elements in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the font size for small text elements. The default value is 10.
        /// </value>
        public double FontSizeSmall { get; set; }
        /// <summary>
        /// Gets or sets the medium font size used in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the medium font size.
        /// </value>
        public double FontSizeMedium { get; set; }
        /// <summary>
        /// Gets or sets the font size for large text elements in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the font size for large text elements. 
        /// The default value is 16.
        /// </value>
        public double FontSizeLarge { get; set; }
        /// <summary>
        /// Gets or sets the font weight used in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="FontWeight"/> value that specifies the weight of the font.
        /// The default value is <see cref="FontWeight.Normal"/>.
        /// </value>
        public FontWeight FontWeight { get; set; }
        /// <summary>
        /// Gets or sets the color of the border in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="Color"/> representing the border color.
        /// </value>
        public Color BorderColor { get; set; }
        /// <summary>
        /// Gets or sets the thickness of the border for the skin.
        /// </summary>
        /// <value>
        /// A <see cref="Thickness"/> structure that specifies the thickness of the border.
        /// </value>
        public Thickness BorderThickness { get; set; }
        /// <summary>
        /// Gets or sets the border radius applied to UI elements in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing the radius of the border corners, in device-independent units (DIPs).
        /// </value>
        public double BorderRadius { get; set; }
        // Additional UI properties
        /// <summary>
        /// Gets or sets the color used to represent error states in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="Color"/> representing the error color. The default value is typically a shade of red.
        /// </value>
        public Color ErrorColor { get; set; }
        /// <summary>
        /// Gets or sets the color used to represent warnings in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="Color"/> representing the warning color. The default value is "#F39C12".
        /// </value>
        public Color WarningColor { get; set; }
        /// <summary>
        /// Gets or sets the color used to represent success states in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="Color"/> representing the success color.
        /// </value>
        public Color SuccessColor { get; set; }
        // Name of the skin
        /// <summary>
        /// Gets or sets the name of the skin.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the name of the skin. 
        /// The default value is "Dark".
        /// </value>
        public string? Name { get; set; }
        // Constructor with default values
        // URIs to ControlTheme resources mapped by control type or key
        /// <summary>
        /// Gets or sets the dictionary of control theme resource URIs used in the skin.
        /// </summary>
        /// <remarks>
        /// Keys typically represent control types or identifiers, and values are the associated resource URIs (e.g., avares URIs).
        /// </remarks>
        public Dictionary<string, string> ControlThemeUris { get; set; } = new();
        // URIs to general Style resources mapped by key
        /// <summary>
        /// Gets or sets the dictionary of style resource URIs used in the skin.
        /// </summary>
        /// <remarks>
        /// These styles can define visual behavior for multiple controls or layout elements.
        /// </remarks>
        public Dictionary<string, string?> StyleUris { get; set; } = new();
        // Extended typography information such as font scaling and weight map
        /// <summary>
        /// Gets or sets the typography scale for the skin.
        /// </summary>
        /// <value>
        /// A <see cref="TypographyScale"/> object used to define consistent typography sizing and scaling for various text styles.
        /// </value>
        public TypographyScale? Typography { get; set; } = new();
        // Additional font families for specific text roles
        /// <summary>
        /// Gets or sets the font family used for headers.
        /// </summary>
        public FontFamily? HeaderFontFamily { get; set; }
        /// <summary>
        /// Gets or sets the font family used for body text.
        /// </summary>
        public FontFamily? BodyFontFamily { get; set; }
        /// <summary>
        /// Gets or sets the font family used for monospace content (e.g., code blocks).
        /// </summary>
        public FontFamily? MonospaceFontFamily { get; set; }
        // Line height and spacing for typographic elements
        /// <summary>
        /// Gets or sets the line height multiplier used in text layout.
        /// </summary>
        /// <remarks>
        /// This is typically a multiplier on the font size to determine the vertical spacing between lines.
        /// </remarks>
        public double LineHeight { get; set; } = 1.5;

        /// <summary>
        /// Gets or sets the letter spacing used in the skin.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> representing additional space between letters in DIPs. Default is 0.
        /// </value>
        public double LetterSpacing { get; set; } = 0;
        /// <summary>
        /// Gets or sets a value indicating whether ligatures are enabled in text rendering.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable ligatures; otherwise, <c>false</c>.
        /// </value>
        public bool EnableLigatures { get; set; } = true;
        /// <summary>
        /// Dictionary of named asset URIs (images, SVGs, etc.).
        /// </summary>
        public Dictionary<string, string> AssetUris { get; set; } = new();
        /// <summary>
        /// 
        /// </summary>
        public InheritableSkin? BaseSkin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Skin()
        {
            // Set default values for Dark skin
            PrimaryColor = Color.Parse("#343B48");        // GunMetal Dark
            SecondaryColor = Color.Parse("#3D4654");      // GunMetal Medium
            AccentColor = Color.Parse("#3498DB");         // Accent Blue
            PrimaryBackground = Color.Parse("#2C313D");   // Dark background
            SecondaryBackground = Color.Parse("#464F62"); // GunMetal Light
            PrimaryTextColor = Color.Parse("#FFFFFF");
            SecondaryTextColor = Color.Parse("#CCCCCC");
            FontFamily = new FontFamily("Segoe UI, San Francisco, Helvetica, Arial, sans-serif");
            FontSizeSmall = 10;
            FontSizeMedium = 12;
            FontSizeLarge = 16;
            FontWeight = FontWeight.Normal;
            BorderColor = Color.Parse("#5D6778");
            BorderThickness = new Thickness(1);
            BorderRadius = 4;
            ErrorColor = Color.Parse("#E74C3C");
            WarningColor = Color.Parse("#F39C12");
            SuccessColor = Color.Parse("#2ECC71");
            HeaderFontFamily = FontFamily;
            BodyFontFamily = FontFamily;
            MonospaceFontFamily = new FontFamily("Consolas, Monaco, 'Courier New', monospace");
            BaseSkin = null;
            Name = "Dark";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseSkin"></param>
        public void InheritFrom(Skin baseSkin)
        {
            PrimaryColor = PrimaryColor == default ? baseSkin.PrimaryColor : PrimaryColor;
            SecondaryColor = SecondaryColor == default ? baseSkin.SecondaryColor : SecondaryColor;
            AccentColor = AccentColor == default ? baseSkin.AccentColor : AccentColor;
            PrimaryBackground = PrimaryBackground == default ? baseSkin.PrimaryBackground : PrimaryBackground;
            SecondaryBackground = SecondaryBackground == default ? baseSkin.SecondaryBackground : SecondaryBackground;
            PrimaryTextColor = PrimaryTextColor == default ? baseSkin.PrimaryTextColor : PrimaryTextColor;
            SecondaryTextColor = SecondaryTextColor == default ? baseSkin.SecondaryTextColor : SecondaryTextColor;
            BorderColor = BorderColor == default ? baseSkin.BorderColor : BorderColor;
            ErrorColor = ErrorColor == default ? baseSkin.ErrorColor : ErrorColor;
            WarningColor = WarningColor == default ? baseSkin.WarningColor : WarningColor;
            SuccessColor = SuccessColor == default ? baseSkin.SuccessColor : SuccessColor;

            FontFamily ??= baseSkin.FontFamily;
            HeaderFontFamily ??= baseSkin.HeaderFontFamily;
            BodyFontFamily ??= baseSkin.BodyFontFamily;
            MonospaceFontFamily ??= baseSkin.MonospaceFontFamily;

            FontSizeSmall = FontSizeSmall == 0 ? baseSkin.FontSizeSmall : FontSizeSmall;
            FontSizeMedium = FontSizeMedium == 0 ? baseSkin.FontSizeMedium : FontSizeMedium;
            FontSizeLarge = FontSizeLarge == 0 ? baseSkin.FontSizeLarge : FontSizeLarge;
            FontWeight = FontWeight == default ? baseSkin.FontWeight : FontWeight;

            BorderRadius = BorderRadius == 0 ? baseSkin.BorderRadius : BorderRadius;
            BorderThickness = BorderThickness == default ? baseSkin.BorderThickness : BorderThickness;

            LineHeight = LineHeight == 0 ? baseSkin.LineHeight : LineHeight;
            LetterSpacing = LetterSpacing == 0 ? baseSkin.LetterSpacing : LetterSpacing;
            // Typography scale
            Typography ??= new TypographyScale();
            if (baseSkin.Typography != null) Typography.ApplyFallbacksFrom(baseSkin.Typography);

            // Merge control theme URIs and styles (child overrides take precedence)
            foreach (var kvp in baseSkin.ControlThemeUris)
                ControlThemeUris.TryAdd(kvp.Key, kvp.Value);

            foreach (var kvp in baseSkin.StyleUris)
                StyleUris.TryAdd(kvp.Key, kvp.Value);
        }
        // Creates a brush from a color
        /// <summary>
        /// Converts the specified <see cref="Color"/> to a <see cref="SolidColorBrush"/>.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        /// <returns>A <see cref="SolidColorBrush"/> representing the specified color.</returns>
        public static SolidColorBrush ToBrush(Color color)
        {
            return new SolidColorBrush(color);
        }
    }
}