using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;

namespace Avalonia.Accelerate.Appearance.Services.ValidationRules
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorContrastValidationRule : ISkinValidationRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public SkinValidationResult Validate(Skin skin)
        {
            var result = new SkinValidationResult();
            var validator = new SkinValidator();

            // Check primary text contrast
            var primaryContrast = validator.CalculateContrastRatio(skin.PrimaryTextColor, skin.PrimaryBackground);
            if (primaryContrast < 4.5) // WCAG AA standard
            {
                result.AddError($"Primary text contrast ratio ({primaryContrast:F2}) is below WCAG AA standard (4.5:1)");
            }
            else if (primaryContrast < 7.0) // WCAG AAA standard
            {
                result.AddWarning($"Primary text contrast ratio ({primaryContrast:F2}) is below WCAG AAA standard (7.0:1)");
            }

            // Check secondary text contrast
            var secondaryContrast = validator.CalculateContrastRatio(skin.SecondaryTextColor, skin.SecondaryBackground);
            if (secondaryContrast < 3.0) // More lenient for secondary text
            {
                result.AddError($"Secondary text contrast ratio ({secondaryContrast:F2}) is below minimum standard (3.0:1)");
            }

            // Check accent color readability
            var accentContrast = validator.CalculateContrastRatio(skin.PrimaryTextColor, skin.AccentColor);
            if (accentContrast < 3.0)
            {
                result.AddWarning($"Accent color contrast with primary text ({accentContrast:F2}) may be difficult to read");
            }

            return result;
        }
    }
}
