using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Services.ValidationRules;

public class NameValidationRule : ISkinValidationRule
{
    private const int MinLength = 2;
    private const int MaxLength = 50;
    private const int QuiteLongThreshold = 30;
    private static readonly string[] ReservedNames = { "Default", "System", "Auto", "None", "Null", "Empty" };
    private static readonly string[] GenericNames = { "Theme", "Skin", "Custom", "New" };
    private static readonly string[] ProblematicNames = { "Test", "Debug", "Temp", "Sample" };
    private static readonly string[] FilesystemConflictNames = { "con", "prn", "aux", "nul", "com1", "lpt1" };

    public List<SkinValidationMessage> Validate(Skin skin)
    {
        var messages = new List<SkinValidationMessage>();

        var name = skin.Name ?? string.Empty;

        // Empty or whitespace
        if (string.IsNullOrWhiteSpace(name))
        {
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = "Skin name is empty or whitespace.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", "Custom Skin" } }
            });
            return messages; // No point validating further if empty
        }

        name = name.Trim();

        // Too short
        if (name.Length < MinLength)
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = $"Skin name is too short (minimum {MinLength} characters required).",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", "Custom Skin" } }
            });

        // Too long
        if (name.Length > MaxLength)
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = $"Skin name is too long (max {MaxLength} characters allowed).",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", name.Substring(0, MaxLength) } }
            });

        // Leading/trailing whitespace
        if (skin.Name != name)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name has leading or trailing whitespace.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", name } }
            });

        // Multiple consecutive spaces
        if (Regex.IsMatch(name, @" {2,}"))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name contains multiple consecutive spaces.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", Regex.Replace(name, @" {2,}", " ") } }
            });

        // Starts with special character
        if (Regex.IsMatch(name, @"^[\-\._]"))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name starts with a special character.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", name.TrimStart('-', '_', '.') } }
            });

        // Invalid characters (allow letters, digits, spaces, hyphens, underscores, dots)
        if (!Regex.IsMatch(name, @"^[a-zA-Z0-9 _\.\-]+$"))
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = "Skin name contains invalid characters.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", SanitizeName(name) } }
            });

        // Reserved names
        if (ReservedNames.Any(r => string.Equals(r, name, StringComparison.OrdinalIgnoreCase)))
            messages.Add(new SkinValidationMessage
            {
                IsError = true,
                Message = $"Skin name '{name}' is a reserved name.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", name + " Custom" } }
            });

        // Generic names
        if (GenericNames.Any(g => string.Equals(g, name, StringComparison.OrdinalIgnoreCase)))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name is too generic.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", name + " Custom" } }
            });

        // Quite long
        if (name.Length > QuiteLongThreshold)
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name is quite long and may be hard to read.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", name } }
            });

        // Problematic names
        if (ProblematicNames.Any(p => string.Equals(p, name, StringComparison.OrdinalIgnoreCase)))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name might be confusing (common debug/test name).",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", name + " Custom" } }
            });

        // Filesystem conflict patterns
        if (FilesystemConflictNames.Any(f => string.Equals(f, name, StringComparison.OrdinalIgnoreCase)))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name matches common reserved names in file systems.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?> { { "Name", name + " Skin" } }
            });

        // Short name not capitalized
        if (name.Length <= 3 && !char.IsUpper(name[0]))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Short skin name should be capitalized.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?>
                    { { "Name", char.ToUpper(name[0]) + name.Substring(1) } }
            });

        // Contains version numbers (simple pattern vX.X or vX)
        if (Regex.IsMatch(name, @"\bv\d+(\.\d+)?\b", RegexOptions.IgnoreCase))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name contains version numbers.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?>
                    { { "Name", Regex.Replace(name, @"\bv\d+(\.\d+)?\b", "").Trim() } }
            });

        // Excessive capitalization
        if (name.Length >= 3 && name.All(char.IsUpper))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name has excessive capitalization.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?>
                    { { "Name", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()) } }
            });

        // Ends with "Theme" or "Skin"
        if (Regex.IsMatch(name, @"(Theme|Skin)$", RegexOptions.IgnoreCase))
            messages.Add(new SkinValidationMessage
            {
                IsError = false,
                Message = "Skin name ends with 'theme' or 'skin'.",
                InvolvedProperties = new List<string> { "Name" },
                SuggestedValues = new Dictionary<string, object?>
                    { { "Name", Regex.Replace(name, @"\b(Theme|Skin)$", "").Trim() } }
            });

        return messages;
    }

    private string SanitizeName(string name)
    {
        // Allow letters, digits, spaces, hyphens, underscores, dots
        var sanitized =
            new string(name.Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_' || c == '.')
                .ToArray()).Trim();
        return sanitized.Length >= 3 ? sanitized : "Custom Skin";
    }
}