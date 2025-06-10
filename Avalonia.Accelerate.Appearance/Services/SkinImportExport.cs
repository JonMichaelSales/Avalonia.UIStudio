// Skin/SkinImportExport.cs
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Media;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Avalonia.Accelerate.Appearance.Services
{
    /// <summary>
    /// Handles theme import and export operations.
    /// </summary>
    public static class SkinImportExport
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Exports a skin to a JSON file.
        /// </summary>
        public static async Task<bool> ExportSkinAsync(Skin skin, string filePath, string? description = null, string? author = null)
        {
            try
            {
                var serializableTheme = ConvertToSerializable(skin, description, author);
                var json = JsonSerializer.Serialize(serializableTheme, JsonOptions);

                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting skin: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Exports an advanced skin with typography to a JSON file.
        /// </summary>
        public static async Task<bool> ExportAdvancedSkinAsync(Skin skin, string filePath, string? description = null, string? author = null)
        {
            try
            {
                var serializableTheme = ConvertToSerializable(skin, description, author);

                // Add advanced typography
                if (skin.Typography != null)
                {
                    serializableTheme.AdvancedTypography = new SerializableTypography
                    {
                        DisplayLarge = skin.Typography.DisplayLarge,
                        DisplayMedium = skin.Typography.DisplayMedium,
                        DisplaySmall = skin.Typography.DisplaySmall,
                        HeadlineLarge = skin.Typography.HeadlineLarge,
                        HeadlineMedium = skin.Typography.HeadlineMedium,
                        HeadlineSmall = skin.Typography.HeadlineSmall,
                        TitleLarge = skin.Typography.TitleLarge,
                        TitleMedium = skin.Typography.TitleMedium,
                        TitleSmall = skin.Typography.TitleSmall,
                        LabelLarge = skin.Typography.LabelLarge,
                        LabelMedium = skin.Typography.LabelMedium,
                        LabelSmall = skin.Typography.LabelSmall,
                        BodyLarge = skin.Typography.BodyLarge,
                        BodyMedium = skin.Typography.BodyMedium,
                        BodySmall = skin.Typography.BodySmall,
                        HeaderFontFamily = skin.HeaderFontFamily?.ToString(),
                        BodyFontFamily = skin.BodyFontFamily?.ToString(),
                        MonospaceFontFamily = skin.MonospaceFontFamily?.ToString(),
                        LineHeight = skin.LineHeight,
                        LetterSpacing = skin.LetterSpacing,
                        EnableLigatures = skin.EnableLigatures
                    };
                }

                var json = JsonSerializer.Serialize(serializableTheme, JsonOptions);
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting advanced skin: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Exports an inheritable skin to a JSON file.
        /// </summary>
        public static async Task<bool> ExportInheritableSkinAsync(InheritableSkin skin, string filePath, string? description = null, string? author = null)
        {
            try
            {
                var serializableTheme = ConvertToSerializable(skin, description, author);

                // Add inheritance information
                serializableTheme.BaseSkinName = skin.BaseSkinName;
                serializableTheme.PropertyOverrides = skin.PropertyOverrides;

                var json = JsonSerializer.Serialize(serializableTheme, JsonOptions);
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting inheritable skin: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Imports a theme from a JSON file.
        /// </summary>
        public static async Task<SkinImportResult> ImportSkinAsync(string filePath)
        {
            var result = new SkinImportResult();

            try
            {
                if (!File.Exists(filePath))
                {
                    result.ErrorMessage = $"Skin file does not exist: {filePath}";
                    return result;
                }

                var json = await File.ReadAllTextAsync(filePath);
                var serializableTheme = JsonSerializer.Deserialize<SerializableSkin>(json, JsonOptions);

                if (serializableTheme == null)
                {
                    result.ErrorMessage = "Invalid theme file format";
                    return result;
                }

                // Validate before converting
                var validation = await ValidateSkinFileAsync(filePath);
                if (!validation.IsValid)
                {
                    result.ErrorMessage = $"Skin validation failed: {string.Join(", ", validation.Errors)}";
                    result.Warnings.AddRange(validation.Warnings);
                    return result;
                }

                result.Skin = ConvertFromSerializable(serializableTheme);
                result.Success = true;
            }
            catch (JsonException ex)
            {
                result.ErrorMessage = $"JSON parsing error: {ex.Message}";
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"Unexpected error importing theme: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// Imports an advanced theme from a JSON file.
        /// </summary>
        public static async Task<Skin?> ImportAdvancedSkinAsync(string filePath)
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var serializableTheme = JsonSerializer.Deserialize<SerializableSkin>(json, JsonOptions);

                if (serializableTheme == null) return null;

                var baseSkin = ConvertFromSerializable(serializableTheme);
                

                // Apply advanced typography if present
                if (serializableTheme.AdvancedTypography != null)
                {
                    var typography = serializableTheme.AdvancedTypography;

                    baseSkin.Typography = new TypographyScale
                    {
                        DisplayLarge = typography.DisplayLarge,
                        DisplayMedium = typography.DisplayMedium,
                        DisplaySmall = typography.DisplaySmall,
                        HeadlineLarge = typography.HeadlineLarge,
                        HeadlineMedium = typography.HeadlineMedium,
                        HeadlineSmall = typography.HeadlineSmall,
                        TitleLarge = typography.TitleLarge,
                        TitleMedium = typography.TitleMedium,
                        TitleSmall = typography.TitleSmall,
                        LabelLarge = typography.LabelLarge,
                        LabelMedium = typography.LabelMedium,
                        LabelSmall = typography.LabelSmall,
                        BodyLarge = typography.BodyLarge,
                        BodyMedium = typography.BodyMedium,
                        BodySmall = typography.BodySmall
                    };

                    if (typography.HeaderFontFamily != null)
                        baseSkin.HeaderFontFamily = new FontFamily(typography.HeaderFontFamily);
                    if (typography.BodyFontFamily != null)
                        baseSkin.BodyFontFamily = new FontFamily(typography.BodyFontFamily);
                    if (typography.MonospaceFontFamily != null)
                        baseSkin.MonospaceFontFamily = new FontFamily(typography.MonospaceFontFamily);
                    baseSkin.LineHeight = typography.LineHeight;
                    baseSkin.LetterSpacing = typography.LetterSpacing;
                    baseSkin.EnableLigatures = typography.EnableLigatures;
                }

                return baseSkin;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing advanced theme: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Imports an inheritable theme from a JSON file.
        /// </summary>
        public static async Task<InheritableSkin?> ImportInheritableSkinAsync(string filePath)
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var serializableTheme = JsonSerializer.Deserialize<SerializableSkin>(json, JsonOptions);

                if (serializableTheme == null) return null;

                var baseSkin = ConvertFromSerializable(serializableTheme);
                var inheritableSkin = new InheritableSkin();

                // Copy all properties from base skin
                CopyPropertiesToInheritable(inheritableSkin, baseSkin);

                // Set inheritance properties
                inheritableSkin.BaseSkinName = serializableTheme.BaseSkinName;
                inheritableSkin.PropertyOverrides = serializableTheme.PropertyOverrides ?? new Dictionary<string, object>();

                return inheritableSkin;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing inheritable theme: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Validates a theme file before importing.
        /// </summary>
        public static async Task<SkinValidationResult> ValidateSkinFileAsync(string filePath)
        {
            var result = new SkinValidationResult();

            try
            {
                if (!File.Exists(filePath))
                {
                    result.AddError("Skin file does not exist");
                    return result;
                }

                var json = await File.ReadAllTextAsync(filePath);
                var serializableTheme = JsonSerializer.Deserialize<SerializableSkin>(json, JsonOptions);

                if (serializableTheme == null)
                {
                    result.AddError("Invalid JSON format");
                    return result;
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(serializableTheme.Name))
                {
                    result.AddError("Skin name is required");
                }

                // Try to convert to validate color formats
                try
                {
                    var skin = ConvertFromSerializable(serializableTheme);
                    var validator = new SkinValidator();
                    var validationResult = validator.ValidateTheme(skin);

                    result.Errors.AddRange(validationResult.Errors);
                    result.Warnings.AddRange(validationResult.Warnings);
                    result.IsValid = validationResult.IsValid && result.Errors.Count == 0;
                }
                catch (Exception ex)
                {
                    result.AddError($"Invalid theme data: {ex.Message}");
                }
            }
            catch (JsonException)
            {
                result.AddError("Invalid JSON format");
            }
            catch (Exception ex)
            {
                result.AddError($"Error reading theme file: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Exports multiple themes to a theme pack file.
        /// </summary>
        public static async Task<bool> ExportSkinPackAsync(Dictionary<string, Skin> themes, string filePath, string packName, string? description = null)
        {
            try
            {
                var themePack = new
                {
                    Name = packName,
                    Description = description,
                    Version = "1.0",
                    CreatedDate = DateTime.Now,
                    Themes = themes.Select(kvp => ConvertToSerializable(kvp.Value, null, null)).ToArray()
                };

                var json = JsonSerializer.Serialize(themePack, JsonOptions);
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting theme pack: {ex.Message}");
                return false;
            }
        }

        private static SerializableSkin ConvertToSerializable(Skin theme, string? description, string? author)
        {
            return new SerializableSkin
            {
                Name = theme.Name ?? "Unnamed Skin",
                Description = description ?? "",
                Author = author ?? "",
                PrimaryColor = theme.PrimaryColor.ToString(),
                SecondaryColor = theme.SecondaryColor.ToString(),
                AccentColor = theme.AccentColor.ToString(),
                PrimaryBackground = theme.PrimaryBackground.ToString(),
                SecondaryBackground = theme.SecondaryBackground.ToString(),
                PrimaryTextColor = theme.PrimaryTextColor.ToString(),
                SecondaryTextColor = theme.SecondaryTextColor.ToString(),
                BorderColor = theme.BorderColor.ToString(),
                ErrorColor = theme.ErrorColor.ToString(),
                WarningColor = theme.WarningColor.ToString(),
                SuccessColor = theme.SuccessColor.ToString(),
                FontFamily = theme.FontFamily.ToString(),
                FontSizeSmall = theme.FontSizeSmall,
                FontSizeMedium = theme.FontSizeMedium,
                FontSizeLarge = theme.FontSizeLarge,
                FontWeight = theme.FontWeight.ToString(),
                BorderRadius = theme.BorderRadius,
                BorderThickness = new SerializableThickness
                {
                    Left = theme.BorderThickness.Left,
                    Top = theme.BorderThickness.Top,
                    Right = theme.BorderThickness.Right,
                    Bottom = theme.BorderThickness.Bottom
                }
            };
        }

        private static Skin ConvertFromSerializable(SerializableSkin serializableSkin)
        {
            var fontWeight = Enum.TryParse<FontWeight>(serializableSkin.FontWeight, out var weight)
                ? weight
                : FontWeight.Normal;

            return new Skin
            {
                Name = serializableSkin.Name,
                PrimaryColor = Color.Parse(serializableSkin.PrimaryColor),
                SecondaryColor = Color.Parse(serializableSkin.SecondaryColor),
                AccentColor = Color.Parse(serializableSkin.AccentColor),
                PrimaryBackground = Color.Parse(serializableSkin.PrimaryBackground),
                SecondaryBackground = Color.Parse(serializableSkin.SecondaryBackground),
                PrimaryTextColor = Color.Parse(serializableSkin.PrimaryTextColor),
                SecondaryTextColor = Color.Parse(serializableSkin.SecondaryTextColor),
                BorderColor = Color.Parse(serializableSkin.BorderColor),
                ErrorColor = Color.Parse(serializableSkin.ErrorColor),
                WarningColor = Color.Parse(serializableSkin.WarningColor),
                SuccessColor = Color.Parse(serializableSkin.SuccessColor),
                FontFamily = new FontFamily(serializableSkin.FontFamily),
                FontSizeSmall = serializableSkin.FontSizeSmall,
                FontSizeMedium = serializableSkin.FontSizeMedium,
                FontSizeLarge = serializableSkin.FontSizeLarge,
                FontWeight = fontWeight,
                BorderRadius = serializableSkin.BorderRadius,
                BorderThickness = new Thickness(
                    serializableSkin.BorderThickness.Left,
                    serializableSkin.BorderThickness.Top,
                    serializableSkin.BorderThickness.Right,
                    serializableSkin.BorderThickness.Bottom
                )
            };
        }

        private static void CopyPropertiesToInheritable(InheritableSkin target, Skin source)
        {
            target.PrimaryColor = source.PrimaryColor;
            target.SecondaryColor = source.SecondaryColor;
            target.AccentColor = source.AccentColor;
            target.PrimaryBackground = source.PrimaryBackground;
            target.SecondaryBackground = source.SecondaryBackground;
            target.PrimaryTextColor = source.PrimaryTextColor;
            target.SecondaryTextColor = source.SecondaryTextColor;
            target.FontFamily = source.FontFamily;
            target.FontSizeSmall = source.FontSizeSmall;
            target.FontSizeMedium = source.FontSizeMedium;
            target.FontSizeLarge = source.FontSizeLarge;
            target.FontWeight = source.FontWeight;
            target.BorderColor = source.BorderColor;
            target.BorderThickness = source.BorderThickness;
            target.BorderRadius = source.BorderRadius;
            target.ErrorColor = source.ErrorColor;
            target.WarningColor = source.WarningColor;
            target.SuccessColor = source.SuccessColor;
            target.Name = source.Name;
        }
    }
}