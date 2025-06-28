using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services.ValidationRules;

namespace Avalonia.UIStudio.Appearance.Services;

/// <summary>
///     Validates skin configurations and provides error recovery.
/// </summary>
public class SkinValidator
{
    private readonly List<ISkinValidationRule> _validationRules;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SkinValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up the default validation rules for skin validation,
    ///     including checks for color contrast, font size, border consistency, naming conventions,
    ///     and accessibility compliance.
    /// </remarks>
    public SkinValidator()
    {
        _validationRules = new List<ISkinValidationRule>
        {
            new ColorContrastValidationRule(),
            new FontSizeValidationRule(),
            new BorderValidationRule(),
            new NameValidationRule(),
            new AccessibilityValidationRule(),
            
            new SkinCompletenessValidationRule()
        };
    }

    /// <summary>
    ///     Validates a skin and returns validation results.
    /// </summary>
    // Update the ValidateSkin method in SkinValidator class
    public SkinValidationResult ValidateSkin(Skin skin)
    {
        var result = new SkinValidationResult();

        foreach (var rule in _validationRules)
        {
            var ruleMessages = rule.Validate(skin);
            result.ValidationMessages.AddRange(ruleMessages);
        }

        result.IsValid = result.ValidationMessages.All(v => !v.IsError);

        return result;
    }

    /// <summary>
    ///     Attempts to fix validation errors automatically.
    /// </summary>
    public Skin AutoFixSkin(Skin skin)
    {
        var cloneSkin = CloneSkin(skin);

        // Fix null or invalid name
        if (string.IsNullOrWhiteSpace(cloneSkin.Name)) cloneSkin.Name = "Custom Skin";

        // Ensure font sizes are within reasonable bounds
        cloneSkin.FontSizeSmall = Math.Max(8, Math.Min(20, cloneSkin.FontSizeSmall));
        cloneSkin.FontSizeMedium = Math.Max(10, Math.Min(24, cloneSkin.FontSizeMedium));
        cloneSkin.FontSizeLarge = Math.Max(12, Math.Min(32, cloneSkin.FontSizeLarge));

        // Ensure border radius is positive
        cloneSkin.BorderRadius = Math.Max(0, cloneSkin.BorderRadius);

        // Fix color contrast issues
        cloneSkin = FixColorContrast(cloneSkin);

        return cloneSkin;
    }

    private Skin CloneSkin(Skin original)
    {
        return new Skin
        {
            Name = original.Name,
            PrimaryColor = original.PrimaryColor,
            SecondaryColor = original.SecondaryColor,
            AccentColor = original.AccentColor,
            PrimaryBackground = original.PrimaryBackground,
            SecondaryBackground = original.SecondaryBackground,
            PrimaryTextColor = original.PrimaryTextColor,
            SecondaryTextColor = original.SecondaryTextColor,
            FontFamily = original.FontFamily,
            FontSizeSmall = original.FontSizeSmall,
            FontSizeMedium = original.FontSizeMedium,
            FontSizeLarge = original.FontSizeLarge,
            FontWeight = original.FontWeight,
            BorderColor = original.BorderColor,
            BorderThickness = original.BorderThickness,
            BorderRadius = original.BorderRadius,
            ErrorColor = original.ErrorColor,
            WarningColor = original.WarningColor,
            SuccessColor = original.SuccessColor
        };
    }

    private Skin FixColorContrast(Skin skin)
    {
        // Calculate contrast ratio and adjust if needed
        var primaryContrastRatio = CalculateContrastRatio(skin.PrimaryTextColor, skin.PrimaryBackground);

        if (primaryContrastRatio < 4.5) // WCAG AA minimum
            // Adjust text color for better contrast
            skin.PrimaryTextColor = AdjustColorForContrast(skin.PrimaryTextColor, skin.PrimaryBackground, 4.5);

        var secondaryContrastRatio = CalculateContrastRatio(skin.SecondaryTextColor, skin.SecondaryBackground);

        if (secondaryContrastRatio < 3.0) // More lenient for secondary text
            skin.SecondaryTextColor =
                AdjustColorForContrast(skin.SecondaryTextColor, skin.SecondaryBackground, 3.0);

        return skin;
    }

    /// <summary>
    /// </summary>
    /// <param name="foreground"></param>
    /// <param name="background"></param>
    /// <returns></returns>
    public double CalculateContrastRatio(Color foreground, Color background)
    {
        try
        {
            var fgLuminance = GetRelativeLuminance(foreground);
            var bgLuminance = GetRelativeLuminance(background);

            var lighter = Math.Max(fgLuminance, bgLuminance);
            var darker = Math.Min(fgLuminance, bgLuminance);

            return (lighter + 0.05) / (darker + 0.05);
        }
        catch (Exception)
        {
            // Return a safe default contrast ratio
            return 1.0;
        }
    }

    private double GetRelativeLuminance(Color color)
    {
        var r = GetLuminanceComponent(color.R / 255.0);
        var g = GetLuminanceComponent(color.G / 255.0);
        var b = GetLuminanceComponent(color.B / 255.0);

        return 0.2126 * r + 0.7152 * g + 0.0722 * b;
    }

    private double GetLuminanceComponent(double component)
    {
        return component <= 0.03928
            ? component / 12.92
            : Math.Pow((component + 0.055) / 1.055, 2.4);
    }

    public Color AdjustColorForContrast(Color foreground, Color background, double targetRatio)
    {
        var bgLuminance = GetRelativeLuminance(background);
        var isDarkBackground = bgLuminance < 0.5;

        // For dark backgrounds, make text lighter; for light backgrounds, make text darker
        var step = isDarkBackground ? 10 : -10;
        var adjustedColor = foreground;

        for (var i = 0; i < 25; i++) // Limit iterations to prevent infinite loop
        {
            var ratio = CalculateContrastRatio(adjustedColor, background);
            if (ratio >= targetRatio) break;

            adjustedColor = Color.FromRgb(
                (byte)Math.Max(0, Math.Min(255, adjustedColor.R + step)),
                (byte)Math.Max(0, Math.Min(255, adjustedColor.G + step)),
                (byte)Math.Max(0, Math.Min(255, adjustedColor.B + step))
            );
        }

        return adjustedColor;
    }
}