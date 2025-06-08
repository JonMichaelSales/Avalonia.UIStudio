using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;

namespace Avalonia.Accelerate.Appearance.Services.ValidationRules
{
    /// <summary>
    /// 
    /// </summary>
    public class FontSizeValidationRule : ISkinValidationRule
    {
        /// <summary>
        /// 
        /// </summary>
        public SkinValidationResult Validate(Skin skin)
        {
            var result = new SkinValidationResult();

            if (skin.FontSizeSmall < 8 || skin.FontSizeSmall > 20)
            {
                result.AddError($"Small font size ({skin.FontSizeSmall}) should be between 8 and 20");
            }

            if (skin.FontSizeMedium < 10 || skin.FontSizeMedium > 24)
            {
                result.AddError($"Medium font size ({skin.FontSizeMedium}) should be between 10 and 24");
            }

            if (skin.FontSizeLarge < 12 || skin.FontSizeLarge > 32)
            {
                result.AddError($"Large font size ({skin.FontSizeLarge}) should be between 12 and 32");
            }

            if (skin.FontSizeSmall >= skin.FontSizeMedium)
            {
                result.AddError("Small font size should be smaller than medium font size");
            }

            if (skin.FontSizeMedium >= skin.FontSizeLarge)
            {
                result.AddError("Medium font size should be smaller than large font size");
            }

            return result;
        }
    }
}
