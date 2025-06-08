using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;

namespace Avalonia.Accelerate.Appearance.Services.ValidationRules
{
    /// <summary>
    /// Validates border-related properties of themes to ensure visual consistency and usability.
    /// </summary>
    public class BorderValidationRule : ISkinValidationRule
    {
        /// <summary>
        /// Validates border properties including thickness, radius, and color contrast.
        /// </summary>
        /// <param name="skin">The skin to validate</param>
        /// <returns>Validation result with any errors or warnings</returns>
        public SkinValidationResult Validate(Skin skin)
        {
            var result = new SkinValidationResult();

            // Validate border thickness
            ValidateBorderThickness(skin, result);

            // Validate border radius
            ValidateBorderRadius(skin, result);

            // Validate border color contrast
            ValidateBorderColorContrast(skin, result);

            return result;
        }

        private void ValidateBorderThickness(Skin theme, SkinValidationResult result)
        {
            var thickness = theme.BorderThickness;

            // Check for negative values
            if (thickness.Left < 0 || thickness.Top < 0 || thickness.Right < 0 || thickness.Bottom < 0)
            {
                result.AddError("Border thickness values cannot be negative");
            }

            // Check for excessive thickness
            var maxThickness = Math.Max(Math.Max(thickness.Left, thickness.Right),
                                      Math.Max(thickness.Top, thickness.Bottom));
            if (maxThickness > 10)
            {
                result.AddWarning($"Border thickness ({maxThickness}) is very large and may impact usability");
            }

            // Check for zero thickness (might be intentional)
            if (thickness.Left == 0 && thickness.Top == 0 && thickness.Right == 0 && thickness.Bottom == 0)
            {
                result.AddWarning("All border thickness values are zero - borders will be invisible");
            }
        }

        private void ValidateBorderRadius(Skin theme, SkinValidationResult result)
        {
            var radius = theme.BorderRadius;

            // Check for negative radius
            if (radius < 0)
            {
                result.AddError($"Border radius ({radius}) cannot be negative");
            }

            // Check for excessive radius
            if (radius > 50)
            {
                result.AddWarning($"Border radius ({radius}) is very large and may cause visual issues");
            }

            // Check for very small radius that might not be visible
            if (radius > 0 && radius < 1)
            {
                result.AddWarning($"Border radius ({radius}) is very small and may not be visible");
            }
        }

        private void ValidateBorderColorContrast(Skin theme, SkinValidationResult result)
        {
            var validator = new SkinValidator();

            // Check border contrast against primary background
            var primaryBorderContrast = validator.CalculateContrastRatio(theme.BorderColor, theme.PrimaryBackground);
            if (primaryBorderContrast < 1.5)
            {
                result.AddError($"Border color has insufficient contrast against primary background (ratio: {primaryBorderContrast:F2})");
            }
            else if (primaryBorderContrast < 2.0)
            {
                result.AddWarning($"Border color has low contrast against primary background (ratio: {primaryBorderContrast:F2})");
            }

            // Check border contrast against secondary background
            var secondaryBorderContrast = validator.CalculateContrastRatio(theme.BorderColor, theme.SecondaryBackground);
            if (secondaryBorderContrast < 1.5)
            {
                result.AddError($"Border color has insufficient contrast against secondary background (ratio: {secondaryBorderContrast:F2})");
            }
            else if (secondaryBorderContrast < 2.0)
            {
                result.AddWarning($"Border color has low contrast against secondary background (ratio: {secondaryBorderContrast:F2})");
            }

            // Check if border color is too similar to text colors (might cause confusion)
            var textSimilarity = validator.CalculateContrastRatio(theme.BorderColor, theme.PrimaryTextColor);
            if (textSimilarity < 1.2)
            {
                result.AddWarning("Border color is very similar to primary text color, which may cause visual confusion");
            }
        }
    }
}