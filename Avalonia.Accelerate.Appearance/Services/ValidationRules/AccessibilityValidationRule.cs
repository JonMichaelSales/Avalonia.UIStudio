using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Media;

namespace Avalonia.Accelerate.Appearance.Services.ValidationRules
{
    /// <summary>
    /// Validates themes for accessibility compliance including WCAG guidelines.
    /// </summary>
    public class AccessibilityValidationRule : ISkinValidationRule
    {
        private const double WcagAaContrastRatio = 4.5;
        private const double WcagAaaContrastRatio = 7.0;
        private const double MinimumFontSize = 12.0;
        private const double RecommendedMinimumFontSize = 14.0;
        private const double MaximumRecommendedFontSize = 32.0;

        /// <summary>
        /// Validates skin for accessibility compliance across multiple criteria.
        /// </summary>
        /// <param name="skin">The skin to validate</param>
        /// <returns>Validation result with accessibility errors or warnings</returns>
        public SkinValidationResult Validate(Skin skin)
        {
            var result = new SkinValidationResult();
            var validator = new SkinValidator();

            // WCAG 2.1 Color Contrast Validation
            ValidateColorContrast(skin, validator, result);

            // Font Size Accessibility
            ValidateFontSizes(skin, result);

            // Color-Only Information (check for sufficient differentiation)
            ValidateColorDifferentiation(skin, validator, result);

            // Focus Indicators
            ValidateFocusIndicators(skin, validator, result);

            // Status Colors Accessibility
            ValidateStatusColors(skin, validator, result);

            // Motion and Animation Considerations
            ValidateVisualStability(skin, result);

            return result;
        }

        private void ValidateColorContrast(Skin theme, SkinValidator validator, SkinValidationResult result)
        {
            // Primary text on primary background
            var primaryContrast = validator.CalculateContrastRatio(theme.PrimaryTextColor, theme.PrimaryBackground);
            ValidateContrastRatio(primaryContrast, "Primary text on primary background", result);

            // Primary text on secondary background
            var primaryOnSecondaryContrast = validator.CalculateContrastRatio(theme.PrimaryTextColor, theme.SecondaryBackground);
            ValidateContrastRatio(primaryOnSecondaryContrast, "Primary text on secondary background", result);

            // Secondary text on primary background
            var secondaryContrast = validator.CalculateContrastRatio(theme.SecondaryTextColor, theme.PrimaryBackground);
            ValidateContrastRatio(secondaryContrast, "Secondary text on primary background", result, isSecondaryText: true);

            // Secondary text on secondary background
            var secondaryOnSecondaryContrast = validator.CalculateContrastRatio(theme.SecondaryTextColor, theme.SecondaryBackground);
            ValidateContrastRatio(secondaryOnSecondaryContrast, "Secondary text on secondary background", result, isSecondaryText: true);

            // Accent color accessibility
            var accentOnPrimaryContrast = validator.CalculateContrastRatio(theme.AccentColor, theme.PrimaryBackground);
            if (accentOnPrimaryContrast < 3.0)
            {
                result.AddWarning($"Accent color on primary background has low contrast ({accentOnPrimaryContrast:F2}:1). May not be distinguishable for users with visual impairments");
            }
        }

        private void ValidateContrastRatio(double ratio, string context, SkinValidationResult result, bool isSecondaryText = false)
        {
            var minRatio = isSecondaryText ? 3.0 : WcagAaContrastRatio;
            var recommendedRatio = isSecondaryText ? WcagAaContrastRatio : WcagAaaContrastRatio;

            if (ratio < minRatio)
            {
                result.AddError($"{context} contrast ratio ({ratio:F2}:1) fails WCAG {(isSecondaryText ? "AA" : "AA")} minimum ({minRatio}:1)");
            }
            else if (ratio < recommendedRatio)
            {
                result.AddWarning($"{context} contrast ratio ({ratio:F2}:1) meets minimum but not enhanced WCAG AAA standard ({recommendedRatio}:1)");
            }
        }

        private void ValidateFontSizes(Skin theme, SkinValidationResult result)
        {
            // Check minimum font sizes for accessibility
            if (theme.FontSizeSmall < MinimumFontSize)
            {
                result.AddError($"Small font size ({theme.FontSizeSmall}px) is below accessibility minimum ({MinimumFontSize}px)");
            }
            else if (theme.FontSizeSmall < RecommendedMinimumFontSize)
            {
                result.AddWarning($"Small font size ({theme.FontSizeSmall}px) is below recommended minimum ({RecommendedMinimumFontSize}px) for good accessibility");
            }

            if (theme.FontSizeMedium < RecommendedMinimumFontSize)
            {
                result.AddWarning($"Medium font size ({theme.FontSizeMedium}px) is below recommended size ({RecommendedMinimumFontSize}px) for primary content");
            }

            // Check for excessively large fonts that might cause layout issues
            if (theme.FontSizeLarge > MaximumRecommendedFontSize)
            {
                result.AddWarning($"Large font size ({theme.FontSizeLarge}px) exceeds recommended maximum ({MaximumRecommendedFontSize}px) and may cause layout issues");
            }

            // Check font size progression for logical hierarchy
            var smallToMediumRatio = theme.FontSizeMedium / theme.FontSizeSmall;
            var mediumToLargeRatio = theme.FontSizeLarge / theme.FontSizeMedium;

            if (smallToMediumRatio < 1.1)
            {
                result.AddWarning("Small and medium font sizes are too similar. Consider larger difference for better visual hierarchy");
            }

            if (mediumToLargeRatio < 1.2)
            {
                result.AddWarning("Medium and large font sizes are too similar. Consider larger difference for better visual hierarchy");
            }
        }

        private void ValidateColorDifferentiation(Skin theme, SkinValidator validator, SkinValidationResult result)
        {
            // Check if primary and secondary colors are sufficiently different
            var primarySecondaryDiff = validator.CalculateContrastRatio(theme.PrimaryColor, theme.SecondaryColor);
            if (primarySecondaryDiff < 1.5)
            {
                result.AddWarning($"Primary and secondary colors are very similar ({primarySecondaryDiff:F2}:1). Users may have difficulty distinguishing them");
            }

            // Check background color differentiation
            var backgroundDiff = validator.CalculateContrastRatio(theme.PrimaryBackground, theme.SecondaryBackground);
            if (backgroundDiff < 1.3)
            {
                result.AddWarning($"Primary and secondary backgrounds are very similar ({backgroundDiff:F2}:1). May reduce visual hierarchy");
            }

            // Ensure accent color is sufficiently different from primary colors
            var accentPrimaryDiff = validator.CalculateContrastRatio(theme.AccentColor, theme.PrimaryColor);
            if (accentPrimaryDiff < 2.0)
            {
                result.AddWarning($"Accent color is too similar to primary color ({accentPrimaryDiff:F2}:1). May not provide sufficient emphasis");
            }
        }

        private void ValidateFocusIndicators(Skin theme, SkinValidator validator, SkinValidationResult result)
        {
            // Check if accent color (typically used for focus) is visible against backgrounds
            var accentFocusVisibility = validator.CalculateContrastRatio(theme.AccentColor, theme.PrimaryBackground);
            if (accentFocusVisibility < 3.0)
            {
                result.AddError($"Accent color (focus indicator) has insufficient contrast against primary background ({accentFocusVisibility:F2}:1). Focus may not be visible to all users");
            }

            // Check border visibility for focus indicators
            var borderFocusVisibility = validator.CalculateContrastRatio(theme.BorderColor, theme.PrimaryBackground);
            if (borderFocusVisibility < 2.0)
            {
                result.AddWarning($"Border color has low contrast against primary background ({borderFocusVisibility:F2}:1). May impact focus indicator visibility");
            }
        }

        private void ValidateStatusColors(Skin theme, SkinValidator validator, SkinValidationResult result)
        {
            // Validate error color visibility
            var errorVisibility = validator.CalculateContrastRatio(theme.ErrorColor, theme.PrimaryBackground);
            if (errorVisibility < 3.0)
            {
                result.AddError($"Error color has insufficient contrast ({errorVisibility:F2}:1). Critical error messages may not be visible");
            }

            // Validate warning color visibility
            var warningVisibility = validator.CalculateContrastRatio(theme.WarningColor, theme.PrimaryBackground);
            if (warningVisibility < 3.0)
            {
                result.AddWarning($"Warning color has low contrast ({warningVisibility:F2}:1). Warning messages may not be clearly visible");
            }

            // Validate success color visibility
            var successVisibility = validator.CalculateContrastRatio(theme.SuccessColor, theme.PrimaryBackground);
            if (successVisibility < 3.0)
            {
                result.AddWarning($"Success color has low contrast ({successVisibility:F2}:1). Success messages may not be clearly visible");
            }

            // Check that status colors are sufficiently different from each other
            ValidateStatusColorDifferentiation(theme, validator, result);
        }

        private void ValidateStatusColorDifferentiation(Skin theme, SkinValidator validator, SkinValidationResult result)
        {
            var errorWarningDiff = validator.CalculateContrastRatio(theme.ErrorColor, theme.WarningColor);
            if (errorWarningDiff < 2.0)
            {
                result.AddWarning($"Error and warning colors are too similar ({errorWarningDiff:F2}:1). Users may confuse error and warning states");
            }

            var errorSuccessDiff = validator.CalculateContrastRatio(theme.ErrorColor, theme.SuccessColor);
            if (errorSuccessDiff < 2.0)
            {
                result.AddWarning($"Error and success colors are too similar ({errorSuccessDiff:F2}:1). Users may confuse error and success states");
            }

            var warningSuccessDiff = validator.CalculateContrastRatio(theme.WarningColor, theme.SuccessColor);
            if (warningSuccessDiff < 2.0)
            {
                result.AddWarning($"Warning and success colors are too similar ({warningSuccessDiff:F2}:1). Users may confuse warning and success states");
            }
        }

        private void ValidateVisualStability(Skin theme, SkinValidationResult result)
        {
            // Check for colors that might trigger photosensitive epilepsy
            if (IsHighSaturationColor(theme.AccentColor) || IsHighSaturationColor(theme.ErrorColor))
            {
                result.AddWarning("Skin contains very bright, saturated colors that could be problematic for users with photosensitive conditions");
            }

            // Check for extreme contrast that might cause eye strain
            var textBackgroundContrast = new SkinValidator().CalculateContrastRatio(theme.PrimaryTextColor, theme.PrimaryBackground);
            if (textBackgroundContrast > 15.0)
            {
                result.AddWarning($"Very high contrast ratio ({textBackgroundContrast:F2}:1) may cause eye strain for some users during extended use");
            }
        }

        private bool IsHighSaturationColor(Color color)
        {
            // Convert to HSV to check saturation
            var max = Math.Max(color.R, Math.Max(color.G, color.B)) / 255.0;
            var min = Math.Min(color.R, Math.Min(color.G, color.B)) / 255.0;

            var saturation = max == 0 ? 0 : (max - min) / max;
            var value = max;

            // High saturation (>0.8) and high value (>0.8) might be problematic
            return saturation > 0.8 && value > 0.8;
        }
    }
}