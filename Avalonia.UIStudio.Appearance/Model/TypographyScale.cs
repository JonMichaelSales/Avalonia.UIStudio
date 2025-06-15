// Skin/TypographySystem.cs

namespace Avalonia.UIStudio.Appearance.Model;

/// <summary>
///     Defines typography scale and settings for skins.
/// </summary>
public class TypographyScale
{
    // Display sizes (largest)
    /// <summary>
    ///     Gets or sets the size of the largest display typography.
    /// </summary>
    /// <value>
    ///     A <see cref="double" /> representing the size of the largest display typography.
    ///     The default value is 57.
    /// </value>
    public double DisplayLarge { get; set; } = 57;

    /// <summary>
    ///     Gets or sets the medium display typography size.
    /// </summary>
    /// <value>
    ///     The size of the medium display typography, typically used for prominent text elements.
    ///     The default value is 45.
    /// </value>
    public double DisplayMedium { get; set; } = 45;

    /// <summary>
    ///     Gets or sets the size of the "Display Small" typography, typically used for smaller display text.
    /// </summary>
    /// <value>
    ///     A <see cref="double" /> representing the size of the "Display Small" typography. The default value is 36.
    /// </value>
    public double DisplaySmall { get; set; } = 36;

    // Headline sizes
    /// <summary>
    ///     Gets or sets the font size for large headlines in the typography scale.
    /// </summary>
    /// <value>
    ///     A <see cref="double" /> representing the font size for large headlines.
    ///     The default value is 32.
    /// </value>
    public double HeadlineLarge { get; set; } = 32;

    /// <summary>
    ///     Gets or sets the font size for medium-sized headlines in the typography scale.
    /// </summary>
    /// <value>
    ///     The font size for medium-sized headlines, typically used for emphasizing content
    ///     that is less prominent than large headlines but more significant than small headlines.
    ///     The default value is 28.
    /// </value>
    public double HeadlineMedium { get; set; } = 28;

    /// <summary>
    ///     Gets or sets the font size for small headline text in the typography scale.
    /// </summary>
    /// <value>
    ///     A <see cref="double" /> representing the font size for small headline text.
    ///     The default value is 24.
    /// </value>
    public double HeadlineSmall { get; set; } = 24;

    // Title sizes
    /// <summary>
    ///     Gets or sets the font size for large titles in the typography scale.
    /// </summary>
    /// <value>
    ///     The font size for large titles, typically used for prominent headings or titles.
    ///     The default value is 22.
    /// </value>
    public double TitleLarge { get; set; } = 22;

    /// <summary>
    ///     Gets or sets the font size for medium-sized titles in the typography scale.
    /// </summary>
    /// <value>
    ///     The font size for medium-sized titles, typically used for medium emphasis text elements.
    ///     The default value is 16.
    /// </value>
    public double TitleMedium { get; set; } = 16;

    /// <summary>
    ///     Gets or sets the font size for small titles in the typography scale.
    /// </summary>
    /// <value>
    ///     The font size for small titles, typically used for less prominent headings or titles.
    ///     The default value is 14.
    /// </value>
    public double TitleSmall { get; set; } = 14;

    // Label sizes
    /// <summary>
    ///     Gets or sets the font size for large labels in the typography scale.
    /// </summary>
    /// <value>
    ///     The size of the font for large labels, typically used for prominent labeling.
    ///     The default value is 14.
    /// </value>
    public double LabelLarge { get; set; } = 14;

    /// <summary>
    ///     Gets or sets the font size for medium-sized labels in the typography scale.
    /// </summary>
    /// <value>
    ///     The font size for medium-sized labels. The default value is 12.
    /// </value>
    public double LabelMedium { get; set; } = 12;

    /// <summary>
    ///     Gets or sets the font size for small labels in the typography scale.
    /// </summary>
    /// <value>
    ///     The font size, in device-independent units (DIPs), for small labels. The default value is 11.
    /// </value>
    public double LabelSmall { get; set; } = 11;

    // Body sizes
    /// <summary>
    ///     Gets or sets the font size for large body text.
    /// </summary>
    /// <value>
    ///     The font size for large body text, typically used for primary content areas.
    ///     Default value is 16.
    /// </value>
    public double BodyLarge { get; set; } = 16;

    /// <summary>
    ///     Gets or sets the font size for medium body text in the typography scale.
    /// </summary>
    /// <value>
    ///     The font size, in device-independent units (DIPs), for medium body text.
    ///     The default value is 14.
    /// </value>
    public double BodyMedium { get; set; } = 14;

    /// <summary>
    ///     Gets or sets the font size for small body text.
    /// </summary>
    /// <value>
    ///     The font size for small body text, typically used for less prominent content.
    /// </value>
    public double BodySmall { get; set; } = 12;

    /// <summary>
    ///     Applies a scale factor to all typography sizes.
    /// </summary>
    public void ApplyScale(double scaleFactor)
    {
        DisplayLarge *= scaleFactor;
        DisplayMedium *= scaleFactor;
        DisplaySmall *= scaleFactor;
        HeadlineLarge *= scaleFactor;
        HeadlineMedium *= scaleFactor;
        HeadlineSmall *= scaleFactor;
        TitleLarge *= scaleFactor;
        TitleMedium *= scaleFactor;
        TitleSmall *= scaleFactor;
        LabelLarge *= scaleFactor;
        LabelMedium *= scaleFactor;
        LabelSmall *= scaleFactor;
        BodyLarge *= scaleFactor;
        BodyMedium *= scaleFactor;
        BodySmall *= scaleFactor;
    }

    // Add these methods to TypographyScale.cs
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public TypographyScale Clone()
    {
        return new TypographyScale
        {
            DisplayLarge = DisplayLarge,
            DisplayMedium = DisplayMedium,
            DisplaySmall = DisplaySmall,
            HeadlineLarge = HeadlineLarge,
            HeadlineMedium = HeadlineMedium,
            HeadlineSmall = HeadlineSmall,
            TitleLarge = TitleLarge,
            TitleMedium = TitleMedium,
            TitleSmall = TitleSmall,
            LabelLarge = LabelLarge,
            LabelMedium = LabelMedium,
            LabelSmall = LabelSmall,
            BodyLarge = BodyLarge,
            BodyMedium = BodyMedium,
            BodySmall = BodySmall
        };
    }

    /// <summary>
    /// </summary>
    public void Reset()
    {
        DisplayLarge = 57;
        DisplayMedium = 45;
        DisplaySmall = 36;
        HeadlineLarge = 32;
        HeadlineMedium = 28;
        HeadlineSmall = 24;
        TitleLarge = 22;
        TitleMedium = 16;
        TitleSmall = 14;
        LabelLarge = 14;
        LabelMedium = 12;
        LabelSmall = 11;
        BodyLarge = 16;
        BodyMedium = 14;
        BodySmall = 12;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public bool ValidateScale()
    {
        return DisplayLarge > DisplayMedium &&
               DisplayMedium > DisplaySmall &&
               HeadlineLarge > HeadlineMedium &&
               HeadlineMedium > HeadlineSmall &&
               TitleLarge > TitleMedium &&
               TitleMedium > TitleSmall &&
               BodyLarge > BodyMedium &&
               BodyMedium > BodySmall;
    }

    public void ApplyFallbacksFrom(TypographyScale fallback)
    {
        DisplayLarge = DisplayLarge == 0 ? fallback.DisplayLarge : DisplayLarge;
        DisplayMedium = DisplayMedium == 0 ? fallback.DisplayMedium : DisplayMedium;
        DisplaySmall = DisplaySmall == 0 ? fallback.DisplaySmall : DisplaySmall;
        HeadlineLarge = HeadlineLarge == 0 ? fallback.HeadlineLarge : HeadlineLarge;
        HeadlineMedium = HeadlineMedium == 0 ? fallback.HeadlineMedium : HeadlineMedium;
        HeadlineSmall = HeadlineSmall == 0 ? fallback.HeadlineSmall : HeadlineSmall;
        TitleLarge = TitleLarge == 0 ? fallback.TitleLarge : TitleLarge;
        TitleMedium = TitleMedium == 0 ? fallback.TitleMedium : TitleMedium;
        TitleSmall = TitleSmall == 0 ? fallback.TitleSmall : TitleSmall;
        LabelLarge = LabelLarge == 0 ? fallback.LabelLarge : LabelLarge;
        LabelMedium = LabelMedium == 0 ? fallback.LabelMedium : LabelMedium;
        LabelSmall = LabelSmall == 0 ? fallback.LabelSmall : LabelSmall;
        BodyLarge = BodyLarge == 0 ? fallback.BodyLarge : BodyLarge;
        BodyMedium = BodyMedium == 0 ? fallback.BodyMedium : BodyMedium;
        BodySmall = BodySmall == 0 ? fallback.BodySmall : BodySmall;
    }
}