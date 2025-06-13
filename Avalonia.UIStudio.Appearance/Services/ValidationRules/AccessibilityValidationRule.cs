using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Services.ValidationRules;

public class AccessibilityValidationRule : ISkinValidationRule
{
    public List<SkinValidationMessage> Validate(Skin skin)
    {
        var result = new SkinValidationResult();

        // Text & background contrast
        ValidateContrast(result, "PrimaryTextColor", skin.PrimaryTextColor, skin.PrimaryBackground);
        ValidateContrast(result, "SecondaryTextColor", skin.SecondaryTextColor, skin.PrimaryBackground);
        ValidateContrast(result, "BorderColor", skin.BorderColor, skin.PrimaryBackground);

        // Font sizes
        ValidateFontSize(result, "FontSizeSmall", skin.FontSizeSmall, 12.0, 20.0);
        ValidateFontSize(result, "FontSizeMedium", skin.FontSizeMedium, 14.0, 24.0);
        ValidateFontSize(result, "FontSizeLarge", skin.FontSizeLarge, 18.0, 32.0);


        // Feedback colors
        ValidateContrast(result, "WarningColor", skin.WarningColor, skin.PrimaryBackground);
        ValidateContrast(result, "SuccessColor", skin.SuccessColor, skin.PrimaryBackground);
        ValidateContrast(result, "ErrorColor", skin.ErrorColor, skin.PrimaryBackground);

        ValidateVisualStability(result,skin);
        
        
        // Optional info-level UX guidance: warning vs success similarity
        if (AreStatusColorsTooSimilar(skin.WarningColor, skin.SuccessColor))
        {
            result.ValidationMessages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "WarningColor and SuccessColor are perceptually too similar.",
                InvolvedProperties = new List<string> { "WarningColor", "SuccessColor" }
            });
        }
        // Update IsValid
        result.IsValid = !result.ValidationMessages.Any(vm => vm.IsError);

        return result.ValidationMessages;
    }

    public void ValidateStatusColorDifferentiation(SkinValidationResult result, Skin skin)
    {
        ValidateColorSimilarity(result, "WarningColor", skin.WarningColor, "SuccessColor", skin.SuccessColor);
        ValidateColorSimilarity(result, "WarningColor", skin.WarningColor, "ErrorColor", skin.ErrorColor);
        ValidateColorSimilarity(result, "SuccessColor", skin.SuccessColor, "ErrorColor", skin.ErrorColor);
    }

    public void ValidateColorSimilarity(SkinValidationResult result, string nameA, Color a, string nameB, Color b)
    {
        if (AreStatusColorsTooSimilar(a, b))
        {
            result.ValidationMessages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = $"{nameA} and {nameB} are perceptually too similar.",
                InvolvedProperties = new List<string> { nameA, nameB }
            });
        }
    }


    public bool AreColorsHueSimilar(Color c1, Color c2, double hueThreshold = 30)
    {
        var h1 = RgbToHue(c1);
        var h2 = RgbToHue(c2);
        double diff = Math.Abs(h1 - h2);
        return diff < hueThreshold || diff > 360 - hueThreshold;
    }

    public bool AreStatusColorsTooSimilar(Color a, Color b)
    {
        double hueDiff = Math.Abs(RgbToHue(a) - RgbToHue(b));
        double lumDiff = Math.Abs(RelativeLuminance(a) - RelativeLuminance(b));
        return hueDiff < 30 && lumDiff < 0.2;
    }
    private double RgbToHue(Color color)
    {
        double r = color.R / 255.0;
        double g = color.G / 255.0;
        double b = color.B / 255.0;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;

        if (delta == 0) return 0;

        if (max == r)
            return 60 * (((g - b) / delta) % 6);
        else if (max == g)
            return 60 * (((b - r) / delta) + 2);
        else // max == b
            return 60 * (((r - g) / delta) + 4);
    }

    private void ValidateVisualStability(SkinValidationResult result, Skin skin)
    {
        ValidateColorSaturation(result, "AccentColor", skin.AccentColor);
        ValidateColorSaturation(result, "ErrorColor", skin.ErrorColor);
    }

    private void ValidateColorSaturation(SkinValidationResult result, string propertyName, Color color)
    {
        if (IsHighSaturationColor(color))
        {
            result.ValidationMessages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = $"{propertyName} uses very bright, saturated colors.",
                InvolvedProperties = new List<string> { propertyName }
            });
        }
    }

    private bool IsHighSaturationColor(Color color)
    {
        double r = color.R / 255.0;
        double g = color.G / 255.0;
        double b = color.B / 255.0;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double saturation = max == 0 ? 0 : (max - min) / max;

        return saturation > 0.8;
    }

    private void ValidateContrast(SkinValidationResult result, string propertyName, Color fg, Color bg)
    {
        var contrast = ContrastRatio(fg, bg);
        if (contrast < 4.5)
        {
            result.ValidationMessages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = $"{propertyName} contrast vs background is too low ({contrast:F2}:1).",
                InvolvedProperties = new List<string> { propertyName, "PrimaryBackground" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    { propertyName, SuggestBetterColor(fg, bg) }
                }
            });
        }
    }

    private void ValidateFontSize(SkinValidationResult result, string propertyName, double px, double min, double max)
    {
        if (px < min)
        {
            result.ValidationMessages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = $"{propertyName} ({px}px) is below minimum of {min}px.",
                InvolvedProperties = new List<string> { propertyName },
                SuggestedValues = new Dictionary<string, object?>
                {
                    { propertyName, min }
                }
            });
        }
        else if (px > max)
        {
            result.ValidationMessages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = $"{propertyName} ({px}px) is unusually large — consider reducing (max recommended {max}px).",
                InvolvedProperties = new List<string> { propertyName },
                SuggestedValues = new Dictionary<string, object?>
                {
                    { propertyName, max }
                }
            });
        }
    }

    public void ValidateFontSize(SkinValidationResult result, Skin skin)
    {
        ValidateFontSize(result, "FontSizeSmall", skin.FontSizeSmall, 12.0, 20.0);
        ValidateFontSize(result, "FontSizeMedium", skin.FontSizeMedium, 14.0, 24.0);
        ValidateFontSize(result, "FontSizeLarge", skin.FontSizeLarge, 18.0, 32.0);
    }
    
    
    public void ValidateStatusColors(SkinValidationResult result, Skin skin)
    {
        ValidateContrast(result, "WarningColor", skin.WarningColor, skin.PrimaryBackground);
        ValidateContrast(result, "SuccessColor", skin.SuccessColor, skin.PrimaryBackground);
        ValidateContrast(result, "ErrorColor", skin.ErrorColor, skin.PrimaryBackground);
    }


    private double ContrastRatio(Color c1, Color c2)
    {
        double L1 = RelativeLuminance(c1);
        double L2 = RelativeLuminance(c2);
        return (Math.Max(L1, L2) + 0.05) / (Math.Min(L1, L2) + 0.05);
    }

    private double RelativeLuminance(Color color)
    {
        double R = ToLinear(color.R / 255.0);
        double G = ToLinear(color.G / 255.0);
        double B = ToLinear(color.B / 255.0);
        return 0.2126 * R + 0.7152 * G + 0.0722 * B;
    }

    private double ToLinear(double channel) =>
        channel <= 0.03928 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);

    // Optionally: suggest a slightly darker/lighter color for better contrast
    private Color SuggestBetterColor(Color original, Color background)
    {
        double contrast = ContrastRatio(original, background);

        // If contrast too low, suggest darkening
        if (contrast < 4.5)
        {
            double factor = 0.8; // Darken by 20%
            return Color.FromRgb(
                (byte)(original.R * factor),
                (byte)(original.G * factor),
                (byte)(original.B * factor));
        }

        return original; // Already acceptable
    }
}