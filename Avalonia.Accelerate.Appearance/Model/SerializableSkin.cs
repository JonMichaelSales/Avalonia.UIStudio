using Avalonia.Media;
using Newtonsoft.Json;

namespace Avalonia.Accelerate.Appearance.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SerializableSkin
    {
        /// <summary>
        /// Gets or sets the name of the theme.
        /// </summary>
        /// <remarks>
        /// This property represents the unique identifier or display name of the theme.
        /// It is a required field and must not be null, empty, or whitespace.
        /// </remarks>
        public string? Name { get; set; } = "";
        /// <summary>
        /// Gets or sets a description of the theme, providing additional context or details about its purpose or design.
        /// </summary>
        public string Description { get; set; } = "";
        /// <summary>
        /// Gets or sets the version of the theme.
        /// </summary>
        /// <remarks>
        /// This property indicates the version of the theme, which can be useful for compatibility checks
        /// or identifying updates to the theme.
        /// </remarks>
        public string Version { get; set; } = "1.0";
        /// <summary>
        /// Gets or sets the author of the theme.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the name of the theme's author.
        /// </value>
        public string Author { get; set; } = "";
        /// <summary>
        /// Gets or sets the date and time when the theme was created.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> representing the creation date and time of the theme.
        /// </value>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Colors as hex strings for JSON serialization
        /// <summary>
        /// 
        /// </summary>
        public string PrimaryColor { get; set; } = "#343B48";
        /// <summary>
        /// 
        /// </summary>
        public string SecondaryColor { get; set; } = "#3D4654";
        /// <summary>
        /// 
        /// </summary>
        public string AccentColor { get; set; } = "#3498DB";
        /// <summary>
        /// 
        /// </summary>
        public string PrimaryBackground { get; set; } = "#2C313D";
        /// <summary>
        /// 
        /// </summary>
        public string SecondaryBackground { get; set; } = "#464F62";
        /// <summary>
        /// 
        /// </summary>
        public string PrimaryTextColor { get; set; } = "#FFFFFF";
        /// <summary>
        /// 
        /// </summary>
        public string SecondaryTextColor { get; set; } = "#CCCCCC";
        /// <summary>
        /// 
        /// </summary>
        public string BorderColor { get; set; } = "#5D6778";
        /// <summary>
        /// 
        /// </summary>
        public string ErrorColor { get; set; } = "#E74C3C";
        /// <summary>
        /// 
        /// </summary>
        public string WarningColor { get; set; } = "#F39C12";
        /// <summary>
        /// 
        /// </summary>
        public string SuccessColor { get; set; } = "#2ECC71";


        // Typography
        /// <summary>
        /// 
        /// </summary>
        public string FontFamily { get; set; } = "Segoe UI, San Francisco, Helvetica, Arial, sans-serif";
        /// <summary>
        /// 
        /// </summary>
        public double FontSizeSmall { get; set; } = 10;
        /// <summary>
        /// 
        /// </summary>
        public double FontSizeMedium { get; set; } = 12;
        /// <summary>
        /// 
        /// </summary>
        public double FontSizeLarge { get; set; } = 16;
        /// <summary>
        /// 
        /// </summary>
        public string FontWeight { get; set; } = "Normal";

        // Layout
        /// <summary>
        /// 
        /// </summary>
        public double BorderRadius { get; set; } = 4;
        /// <summary>
        /// 
        /// </summary>
        public SerializableThickness BorderThickness { get; set; } = new() { Left = 1, Top = 1, Right = 1, Bottom = 1 };

        // Advanced typography (optional)
        /// <summary>
        /// 
        /// </summary>
        public SerializableTypography? AdvancedTypography { get; set; }

        // Inheritance (optional)
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public SerializableSkin? BaseSkin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object>? PropertyOverrides { get; set; }

        /// <summary>
        /// Gets or sets the list of control theme file paths relative to the skin base directory.
        /// </summary>
        public List<string>? ControlThemes { get; set; }
        /// <summary>
        /// Optional named assets like logos or backgrounds.
        /// </summary>
        public Dictionary<string, string>? Assets { get; set; }
        /// <summary>
        /// Name of the base skin (for inheritance), serialized for persistence.
        /// </summary>
        public string? BaseSkinName { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class ThemeConverterExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static Skin ToSkin(this SerializableSkin skin)
        {
            return new Skin
            {
                Name = skin.Name,
                PrimaryColor = Color.Parse(skin.PrimaryColor),
                SecondaryColor = Color.Parse(skin.SecondaryColor),
                AccentColor = Color.Parse(skin.AccentColor),
                PrimaryBackground = Color.Parse(skin.PrimaryBackground),
                SecondaryBackground = Color.Parse(skin.SecondaryBackground),
                PrimaryTextColor = Color.Parse(skin.PrimaryTextColor),
                SecondaryTextColor = Color.Parse(skin.SecondaryTextColor),
                BorderColor = Color.Parse(skin.BorderColor),
                ErrorColor = Color.Parse(skin.ErrorColor),
                WarningColor = Color.Parse(skin.WarningColor),
                SuccessColor = Color.Parse(skin.SuccessColor),
                FontFamily = new FontFamily(skin.FontFamily),
                FontSizeSmall = skin.FontSizeSmall,
                FontSizeMedium = skin.FontSizeMedium,
                FontSizeLarge = skin.FontSizeLarge,
                FontWeight = Enum.TryParse<FontWeight>(skin.FontWeight, true, out var fw)
                    ? fw
                    : FontWeight.Normal,
                BorderRadius = skin.BorderRadius,
                BorderThickness = new Thickness(
                    skin.BorderThickness.Left,
                    skin.BorderThickness.Top,
                    skin.BorderThickness.Right,
                    skin.BorderThickness.Bottom
                ),
                Typography =
                    new TypographyScale
                    {
                        DisplayLarge = skin.AdvancedTypography?.DisplayLarge ?? 57,
                        DisplayMedium = skin.AdvancedTypography?.DisplayMedium ?? 45,
                        DisplaySmall = skin.AdvancedTypography?.DisplaySmall ?? 36,
                        HeadlineLarge = skin.AdvancedTypography?.HeadlineLarge ?? 32,
                        HeadlineMedium = skin.AdvancedTypography?.HeadlineMedium ?? 28,
                        HeadlineSmall = skin.AdvancedTypography?.HeadlineSmall ?? 24,
                        TitleLarge = skin.AdvancedTypography?.TitleLarge ?? 22,
                        TitleMedium = skin.AdvancedTypography?.TitleMedium ?? 16,
                        TitleSmall = skin.AdvancedTypography?.TitleSmall ?? 14,
                        LabelLarge = skin.AdvancedTypography?.LabelLarge ?? 14,
                        LabelMedium = skin.AdvancedTypography?.LabelMedium ?? 12,
                        LabelSmall = skin.AdvancedTypography?.LabelSmall ?? 11,
                        BodyLarge = skin.AdvancedTypography?.BodyLarge ?? 16,
                        BodyMedium = skin.AdvancedTypography?.BodyMedium ?? 14,
                        BodySmall = skin.AdvancedTypography?.BodySmall ?? 12,
                    },
                HeaderFontFamily = new FontFamily(skin.AdvancedTypography?.HeaderFontFamily ?? skin.FontFamily),
                BodyFontFamily = new FontFamily(skin.AdvancedTypography?.BodyFontFamily ?? skin.FontFamily),
                MonospaceFontFamily =
                    new FontFamily(skin.AdvancedTypography?.MonospaceFontFamily ??
                                   "Consolas, Monaco, 'Courier New', monospace"),
                LineHeight = skin.AdvancedTypography?.LineHeight ?? 1.5,
                LetterSpacing = skin.AdvancedTypography?.LetterSpacing ?? 0,
                EnableLigatures = skin.AdvancedTypography?.EnableLigatures ?? true,
                ControlThemeUris = skin.ControlThemes?.ToDictionary(x => x, x => x) ?? new(),
                StyleUris = skin.PropertyOverrides?.ToDictionary(x => x.Key,
                    x => x.Value?.ToString()) ?? new(),
                AssetUris = skin.Assets?.ToDictionary(x => x.Key, x => x.Value) ?? new()
            };
        }
    }

}
