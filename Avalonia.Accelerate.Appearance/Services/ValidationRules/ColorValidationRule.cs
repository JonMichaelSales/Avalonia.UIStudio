using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;

namespace Avalonia.Accelerate.Appearance.Services.ValidationRules
{
    public class ColorContrastValidationRule : ISkinValidationRule
    {
        public List<SkinValidationMessage> Validate(Skin skin)
        {
            var messages = new List<SkinValidationMessage>();
            var validator = new SkinValidator();

            // Check primary text contrast
            var primaryContrast = validator.CalculateContrastRatio(skin.PrimaryTextColor, skin.PrimaryBackground);
            if (primaryContrast < 4.5) // WCAG AA standard
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = $"Primary text contrast ratio ({primaryContrast:F2}) is below WCAG AA standard (4.5:1)",
                    InvolvedProperties = new List<string> { "PrimaryTextColor", "PrimaryBackground" },
                    SuggestedValues = new Dictionary<string, object?>
                    {
                        { "PrimaryTextColor", validator.AdjustColorForContrast(skin.PrimaryTextColor, skin.PrimaryBackground, 4.5) },
                        { "PrimaryBackground", null }
                    }
                });
            }
            else if (primaryContrast < 7.0) // WCAG AAA standard
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = false,
                    Message = $"Primary text contrast ratio ({primaryContrast:F2}) is below WCAG AAA standard (7.0:1)",
                    InvolvedProperties = new List<string> { "PrimaryTextColor", "PrimaryBackground" },
                    SuggestedValues = new Dictionary<string, object?>
                    {
                        { "PrimaryTextColor", validator.AdjustColorForContrast(skin.PrimaryTextColor, skin.PrimaryBackground, 7.0) },
                        { "PrimaryBackground", null }
                    }
                });
            }

            // Check secondary text contrast
            var secondaryContrast = validator.CalculateContrastRatio(skin.SecondaryTextColor, skin.SecondaryBackground);
            if (secondaryContrast < 3.0)
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = $"Secondary text contrast ratio ({secondaryContrast:F2}) is below minimum standard (3.0:1)",
                    InvolvedProperties = new List<string> { "SecondaryTextColor", "SecondaryBackground" },
                    SuggestedValues = new Dictionary<string, object?>
                    {
                        { "SecondaryTextColor", validator.AdjustColorForContrast(skin.SecondaryTextColor, skin.SecondaryBackground, 3.0) },
                        { "SecondaryBackground", null }
                    }
                });
            }

            // Check accent color readability
            var accentContrast = validator.CalculateContrastRatio(skin.PrimaryTextColor, skin.AccentColor);
            if (accentContrast < 3.0)
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = false,
                    Message = $"Accent color contrast with primary text ({accentContrast:F2}) may be difficult to read",
                    InvolvedProperties = new List<string> { "PrimaryTextColor", "AccentColor" },
                    SuggestedValues = new Dictionary<string, object?>
                    {
                        { "PrimaryTextColor", validator.AdjustColorForContrast(skin.PrimaryTextColor, skin.AccentColor, 3.0) },
                        { "AccentColor", null }
                    }
                });
            }

            return messages;
        }
    }
}
