using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;

namespace Avalonia.Accelerate.Appearance.Services.ValidationRules
{
    public class FontSizeValidationRule : ISkinValidationRule
    {
        private const double SmallMin = 8.0;
        private const double SmallMax = 20.0;
        private const double MediumMin = 10.0;
        private const double MediumMax = 24.0;
        private const double LargeMin = 12.0;
        private const double LargeMax = 32.0;

        public List<SkinValidationMessage> Validate(Skin skin)
        {
            var messages = new List<SkinValidationMessage>();

            // FontSizeSmall
            if (skin.FontSizeSmall < SmallMin || skin.FontSizeSmall > SmallMax)
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = $"Small font size ({skin.FontSizeSmall}px) must be between {SmallMin} and {SmallMax} px.",
                    InvolvedProperties = new List<string> { "FontSizeSmall" },
                    SuggestedValues = new Dictionary<string, object?>
                    {
                        { "FontSizeSmall", Math.Clamp(skin.FontSizeSmall, SmallMin, SmallMax) }
                    }
                });
            }

            // FontSizeMedium
            if (skin.FontSizeMedium < MediumMin || skin.FontSizeMedium > MediumMax)
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = $"Medium font size ({skin.FontSizeMedium}px) must be between {MediumMin} and {MediumMax} px.",
                    InvolvedProperties = new List<string> { "FontSizeMedium" },
                    SuggestedValues = new Dictionary<string, object?>
                    {
                        { "FontSizeMedium", Math.Clamp(skin.FontSizeMedium, MediumMin, MediumMax) }
                    }
                });
            }

            // FontSizeLarge
            if (skin.FontSizeLarge < LargeMin || skin.FontSizeLarge > LargeMax)
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = $"Large font size ({skin.FontSizeLarge}px) must be between {LargeMin} and {LargeMax} px.",
                    InvolvedProperties = new List<string> { "FontSizeLarge" },
                    SuggestedValues = new Dictionary<string, object?>
                    {
                        { "FontSizeLarge", Math.Clamp(skin.FontSizeLarge, LargeMin, LargeMax) }
                    }
                });
            }

            // Logical font size progression checks
            if (skin.FontSizeSmall >= skin.FontSizeMedium)
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = "Small font size should be smaller than medium font size.",
                    InvolvedProperties = new List<string> { "FontSizeSmall", "FontSizeMedium" },
                    SuggestedValues = new Dictionary<string, object?>()
                });
            }

            if (skin.FontSizeMedium >= skin.FontSizeLarge)
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = "Medium font size should be smaller than large font size.",
                    InvolvedProperties = new List<string> { "FontSizeMedium", "FontSizeLarge" },
                    SuggestedValues = new Dictionary<string, object?>()
                });
            }

            return messages;
        }
    }
}
