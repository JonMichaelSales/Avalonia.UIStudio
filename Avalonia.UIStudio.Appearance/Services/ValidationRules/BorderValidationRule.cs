using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Services.ValidationRules;

public class BorderValidationRule : ISkinValidationRule
{
    public List<SkinValidationMessage> Validate(Skin skin)
    {
        var messages = new List<SkinValidationMessage>();

        ValidateBorderThickness(skin, messages);
        ValidateBorderRadius(skin, messages);
        ValidateBorderColorContrast(skin, messages);

        return messages;
    }

    private void ValidateBorderThickness(Skin skin, List<SkinValidationMessage> messages)
    {
        var thickness = skin.BorderThickness;

        // Negative values
        if (thickness.Left < 0 || thickness.Top < 0 || thickness.Right < 0 || thickness.Bottom < 0)
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = "Border thickness values cannot be negative.",
                InvolvedProperties = new List<string> { "BorderThickness" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    {
                        "BorderThickness",
                        new Thickness(Math.Max(0, thickness.Left), Math.Max(0, thickness.Top),
                            Math.Max(0, thickness.Right), Math.Max(0, thickness.Bottom))
                    }
                }
            });

        // Excessive thickness
        var maxThickness = Math.Max(Math.Max(thickness.Left, thickness.Right),
            Math.Max(thickness.Top, thickness.Bottom));

        if (maxThickness > 10)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = $"Border thickness ({maxThickness}) is very large and may impact usability.",
                InvolvedProperties = new List<string> { "BorderThickness" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    {
                        "BorderThickness", new Thickness(
                            Math.Min(thickness.Left, 10),
                            Math.Min(thickness.Top, 10),
                            Math.Min(thickness.Right, 10),
                            Math.Min(thickness.Bottom, 10))
                    }
                }
            });

        // All zero thickness
        if (thickness.Left == 0 && thickness.Top == 0 && thickness.Right == 0 && thickness.Bottom == 0)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "All border thickness values are zero — borders will be invisible.",
                InvolvedProperties = new List<string> { "BorderThickness" },
                SuggestedValues = new Dictionary<string, object?>()
            });
    }

    private void ValidateBorderRadius(Skin skin, List<SkinValidationMessage> messages)
    {
        var radius = skin.BorderRadius;

        // Negative radius
        if (radius < 0)
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = $"Border radius ({radius}) cannot be negative.",
                InvolvedProperties = new List<string> { "BorderRadius" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    { "BorderRadius", 0.0 }
                }
            });

        // Excessive radius
        if (radius > 50)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = $"Border radius ({radius}) is very large and may cause visual issues.",
                InvolvedProperties = new List<string> { "BorderRadius" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    { "BorderRadius", 50.0 }
                }
            });

        // Very small radius
        if (radius > 0 && radius < 1)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = $"Border radius ({radius}) is very small and may not be visible.",
                InvolvedProperties = new List<string> { "BorderRadius" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    { "BorderRadius", 1.0 }
                }
            });
    }

    private void ValidateBorderColorContrast(Skin skin, List<SkinValidationMessage> messages)
    {
        var validator = new SkinValidator();

        // Border vs PrimaryBackground
        var primaryBorderContrast = validator.CalculateContrastRatio(skin.BorderColor, skin.PrimaryBackground);
        if (primaryBorderContrast < 1.5)
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message =
                    $"Border color has insufficient contrast against primary background (ratio: {primaryBorderContrast:F2}).",
                InvolvedProperties = new List<string> { "BorderColor", "PrimaryBackground" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    { "BorderColor", validator.AdjustColorForContrast(skin.BorderColor, skin.PrimaryBackground, 1.5) },
                    { "PrimaryBackground", null }
                }
            });
        else if (primaryBorderContrast < 2.0)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message =
                    $"Border color has low contrast against primary background (ratio: {primaryBorderContrast:F2}).",
                InvolvedProperties = new List<string> { "BorderColor", "PrimaryBackground" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    { "BorderColor", validator.AdjustColorForContrast(skin.BorderColor, skin.PrimaryBackground, 2.0) },
                    { "PrimaryBackground", null }
                }
            });

        // Border vs SecondaryBackground
        var secondaryBorderContrast = validator.CalculateContrastRatio(skin.BorderColor, skin.SecondaryBackground);
        if (secondaryBorderContrast < 1.5)
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message =
                    $"Border color has insufficient contrast against secondary background (ratio: {secondaryBorderContrast:F2}).",
                InvolvedProperties = new List<string> { "BorderColor", "SecondaryBackground" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    {
                        "BorderColor", validator.AdjustColorForContrast(skin.BorderColor, skin.SecondaryBackground, 1.5)
                    },
                    { "SecondaryBackground", null }
                }
            });
        else if (secondaryBorderContrast < 2.0)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message =
                    $"Border color has low contrast against secondary background (ratio: {secondaryBorderContrast:F2}).",
                InvolvedProperties = new List<string> { "BorderColor", "SecondaryBackground" },
                SuggestedValues = new Dictionary<string, object?>
                {
                    {
                        "BorderColor", validator.AdjustColorForContrast(skin.BorderColor, skin.SecondaryBackground, 2.0)
                    },
                    { "SecondaryBackground", null }
                }
            });

        // Border vs PrimaryTextColor
        var textSimilarity = validator.CalculateContrastRatio(skin.BorderColor, skin.PrimaryTextColor);
        if (textSimilarity < 1.2)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Border color is very similar to primary text color, which may cause visual confusion.",
                InvolvedProperties = new List<string> { "BorderColor", "PrimaryTextColor" },
                SuggestedValues = new Dictionary<string, object?>()
            });
    }
}