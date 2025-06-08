using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;
using System.Text.RegularExpressions;

namespace Avalonia.Accelerate.Appearance.Services.ValidationRules
{
    /// <summary>
    /// Validates theme naming conventions and ensures proper identification.
    /// </summary>
    public class NameValidationRule : ISkinValidationRule
    {
        private static readonly Regex ValidNamePattern = new Regex(@"^[a-zA-Z0-9\s\-_\.]+$", RegexOptions.Compiled);
        private static readonly string[] ReservedNames = { "Default", "System", "Auto", "None", "Null", "Empty" };
        private static readonly string[] ProblematicNames = { "Test", "Debug", "Temp", "Sample" };

        /// <summary>
        /// Validates skin name for proper format, uniqueness, and conventions.
        /// </summary>
        /// <param name="skin">The skin to validate</param>
        /// <returns>Validation result with any errors or warnings</returns>
        public SkinValidationResult Validate(Skin skin)
        {
            var result = new SkinValidationResult();

            // Validate name existence
            ValidateNameExists(skin, result);

            // Validate name format
            ValidateNameFormat(skin, result);

            // Validate name length
            ValidateNameLength(skin, result);

            // Validate reserved names
            ValidateReservedNames(skin, result);

            // Validate naming conventions
            ValidateNamingConventions(skin, result);

            return result;
        }

        private void ValidateNameExists(Skin theme, SkinValidationResult result)
        {
            if (string.IsNullOrEmpty(theme.Name))
            {
                result.AddError("Skin name is required and cannot be null or empty");
                return;
            }

            if (string.IsNullOrWhiteSpace(theme.Name))
            {
                result.AddError("Skin name cannot be only whitespace");
            }
        }

        private void ValidateNameFormat(Skin theme, SkinValidationResult result)
        {
            if (string.IsNullOrEmpty(theme.Name)) return;

            // Check for valid characters
            if (!ValidNamePattern.IsMatch(theme.Name))
            {
                result.AddError("Skin name contains invalid characters. Only letters, numbers, spaces, hyphens, underscores, and periods are allowed");
            }

            // Check for leading/trailing whitespace
            if (theme.Name != theme.Name.Trim())
            {
                result.AddWarning("Skin name has leading or trailing whitespace");
            }

            // Check for multiple consecutive spaces
            if (theme.Name.Contains("  "))
            {
                result.AddWarning("Skin name contains multiple consecutive spaces");
            }

            // Check for starting with special characters
            if (theme.Name.StartsWith("-") || theme.Name.StartsWith("_") || theme.Name.StartsWith("."))
            {
                result.AddWarning("Skin name starts with a special character, which may cause sorting issues");
            }
        }

        private void ValidateNameLength(Skin theme, SkinValidationResult result)
        {
            if (string.IsNullOrEmpty(theme.Name)) return;

            // Check minimum length
            if (theme.Name.Trim().Length < 2)
            {
                result.AddError("Skin name must be at least 2 characters long");
            }

            // Check maximum length
            if (theme.Name.Length > 50)
            {
                result.AddError($"Skin name is too long ({theme.Name.Length} characters). Maximum length is 50 characters");
            }
            else if (theme.Name.Length > 30)
            {
                result.AddWarning($"Skin name is quite long ({theme.Name.Length} characters). Consider a shorter name for better UI display");
            }
        }

        private void ValidateReservedNames(Skin theme, SkinValidationResult result)
        {
            if (string.IsNullOrEmpty(theme.Name)) return;

            var nameLower = theme.Name.ToLowerInvariant().Trim();

            // Check reserved system names
            if (ReservedNames.Any(reserved => string.Equals(nameLower, reserved.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase)))
            {
                result.AddError($"'{theme.Name}' is a reserved name and cannot be used for custom themes");
            }

            // Check problematic names that might cause confusion
            if (ProblematicNames.Any(problematic => string.Equals(nameLower, problematic.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase)))
            {
                result.AddWarning($"'{theme.Name}' might be confusing as it suggests a temporary or development theme");
            }

            // Check for names that might conflict with file system
            if (nameLower.Contains("con") || nameLower.Contains("prn") || nameLower.Contains("aux") ||
                nameLower.Contains("nul") || nameLower.StartsWith("com") || nameLower.StartsWith("lpt"))
            {
                result.AddWarning($"'{theme.Name}' contains patterns that might cause issues on some file systems");
            }
        }

        private void ValidateNamingConventions(Skin theme, SkinValidationResult result)
        {
            if (string.IsNullOrEmpty(theme.Name)) return;

            var name = theme.Name.Trim();

            // Check for descriptive naming
            if (name.Length < 4 && !char.IsUpper(name[0]))
            {
                result.AddWarning("Very short theme names should be capitalized for better readability");
            }

            // Check for version numbers in name (might indicate poor naming)
            if (Regex.IsMatch(name, @"\bv?\d+(\.\d+)*\b", RegexOptions.IgnoreCase))
            {
                result.AddWarning("Skin name contains version numbers. Consider using metadata for versioning instead");
            }

            // Check for excessive capitalization
            var upperCaseCount = name.Count(char.IsUpper);
            var letterCount = name.Count(char.IsLetter);
            if (letterCount > 0 && upperCaseCount / (double)letterCount > 0.6)
            {
                result.AddWarning("Skin name has excessive capitalization, which may impact readability");
            }

            // Check for common naming patterns
            if (name.ToLowerInvariant().EndsWith("theme") || name.ToLowerInvariant().EndsWith("skin"))
            {
                result.AddWarning("Skin name ends with 'theme' or 'skin', which is redundant in this context");
            }

            // Suggest better naming for generic names
            if (name.ToLowerInvariant().Equals("theme") || name.ToLowerInvariant().Equals("skin") ||
                name.ToLowerInvariant().Equals("custom") || name.ToLowerInvariant().Equals("new"))
            {
                result.AddWarning($"'{name}' is too generic. Consider a more descriptive name that reflects the theme's characteristics");
            }
        }
    }
}