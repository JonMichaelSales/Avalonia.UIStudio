// Skin/SkinImportExportService.cs
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Media;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Accelerate.Appearance.Interfaces;

namespace Avalonia.Accelerate.Appearance.Services
{
    /// <summary>
    /// Handles skin import and export operations.
    /// </summary>
    public class SkinImportExportService : ISkinImportExportService
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
        public async Task<bool> ExportSkinAsync(Skin skin, string filePath, string? description = null, string? author = null)
        {
            try
            {
                var serializableSkin = ConvertToSerializable(skin, description, author);
                var json = JsonSerializer.Serialize(serializableSkin, JsonOptions);

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
        public async Task<bool> ExportAdvancedSkinAsync(Skin skin, string filePath, string? description = null, string? author = null)
        {
            try
            {
                var serializableSkin = ConvertToSerializable(skin, description, author);

                // Add advanced typography
                if (skin.Typography != null)
                {
                    serializableSkin.AdvancedTypography = new SerializableTypography
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

                var json = JsonSerializer.Serialize(serializableSkin, JsonOptions);
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
        public async Task<bool> ExportInheritableSkinAsync(InheritableSkin skin, string filePath, string? description = null, string? author = null)
        {
            try
            {
                var serializableSkin = ConvertToSerializable(skin, description, author);

                // Add inheritance information
                serializableSkin.BaseSkinName = skin.BaseSkinName;
                serializableSkin.PropertyOverrides = skin.PropertyOverrides;

                var json = JsonSerializer.Serialize(serializableSkin, JsonOptions);
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
        /// Imports a skin from a JSON file.
        /// </summary>
        public async Task<SkinImportResult> ImportSkinAsync(string filePath)
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
                var serializableSkin = JsonSerializer.Deserialize<SerializableSkin>(json, JsonOptions);

                if (serializableSkin == null)
                {
                    result.ErrorMessage = "Invalid skin file format";
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

                result.Skin = ConvertFromSerializable(serializableSkin);
                result.Success = true;
            }
            catch (JsonException ex)
            {
                result.ErrorMessage = $"JSON parsing error: {ex.Message}";
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"Unexpected error importing skin: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// Imports an advanced skin from a JSON file.
        /// </summary>
        public async Task<Skin?> ImportAdvancedSkinAsync(string filePath)
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var serializableSkin = JsonSerializer.Deserialize<SerializableSkin>(json, JsonOptions);

                if (serializableSkin == null) return null;

                var baseSkin = ConvertFromSerializable(serializableSkin);
                

                // Apply advanced typography if present
                if (serializableSkin.AdvancedTypography != null)
                {
                    var typography = serializableSkin.AdvancedTypography;

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
                Console.WriteLine($"Error importing advanced skin: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Imports an inheritable skin from a JSON file.
        /// </summary>
        public async Task<InheritableSkin?> ImportInheritableSkinAsync(string filePath)
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var serializableSkin = JsonSerializer.Deserialize<SerializableSkin>(json, JsonOptions);

                if (serializableSkin == null) return null;

                var baseSkin = ConvertFromSerializable(serializableSkin);
                var inheritableSkin = new InheritableSkin();

                // Copy all properties from base skin
                CopyPropertiesToInheritable(inheritableSkin, baseSkin);

                // Set inheritance properties
                inheritableSkin.BaseSkinName = serializableSkin.BaseSkinName;
                inheritableSkin.PropertyOverrides = serializableSkin.PropertyOverrides ?? new Dictionary<string, object>();

                return inheritableSkin;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing inheritable skin: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Validates a skin file before importing.
        /// </summary>
        public async Task<SkinValidationResult> ValidateSkinFileAsync(string filePath)
        {
            var result = new SkinValidationResult();

            try
            {
                if (!File.Exists(filePath))
                {
                    result.ValidationMessages.Add(new SkinValidationMessage
                    {
                        IsError = true,
                        Message = "Skin file does not exist",
                        InvolvedProperties = new List<string>(),
                        SuggestedValues = new Dictionary<string, object?>()
                    });
                    result.IsValid = false;
                    return result;
                }

                var json = await File.ReadAllTextAsync(filePath);
                var serializableSkin = JsonSerializer.Deserialize<SerializableSkin>(json, JsonOptions);

                if (serializableSkin == null)
                {
                    result.ValidationMessages.Add(new SkinValidationMessage
                    {
                        IsError = true,
                        Message = "Invalid JSON format",
                        InvolvedProperties = new List<string>(),
                        SuggestedValues = new Dictionary<string, object?>()
                    });
                    return result;
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(serializableSkin.Name))
                {
                    result.ValidationMessages.Add(new SkinValidationMessage
                    {
                        IsError = true,
                        Message = "Skin name is required",
                        InvolvedProperties = new List<string>(),
                        SuggestedValues = new Dictionary<string, object?>()
                    });
                }

                // Try to convert to validate color formats
                try
                {
                    var skin = ConvertFromSerializable(serializableSkin);
                    var validator = new SkinValidator();
                    var validationResult = validator.ValidateSkin(skin);

                    result.Errors.AddRange(validationResult.Errors);
                    result.Warnings.AddRange(validationResult.Warnings);
                    result.IsValid = validationResult.IsValid && result.Errors.Count == 0;
                }
                catch (Exception ex)
                {
                    result.ValidationMessages.Add(new SkinValidationMessage
                    {
                        IsError = true,
                        Message = $"Invalid skin data: {ex.Message}",
                        InvolvedProperties = new List<string>(),
                        SuggestedValues = new Dictionary<string, object?>()
                    });
                }
            }
            catch (JsonException)
            {
                result.IsValid = false;
                result.ValidationMessages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = "Invalid JSON format",
                    InvolvedProperties = new List<string>(),
                    SuggestedValues = new Dictionary<string, object?>()
                });
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ValidationMessages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = $"Error reading skin file: {ex.Message}",
                    InvolvedProperties = new List<string>(),
                    SuggestedValues = new Dictionary<string, object?>()
                });
            }

            return result;
        }

        /// <summary>
        /// Exports multiple skins to a skin pack file.
        /// </summary>
        public async Task<bool> ExportSkinPackAsync(Dictionary<string, Skin> skins, string filePath, string packName, string? description = null)
        {
            try
            {
                var skinPack = new
                {
                    Name = packName,
                    Description = description,
                    Version = "1.0",
                    CreatedDate = DateTime.Now,
                    Skins = skins.Select(kvp => ConvertToSerializable(kvp.Value, null, null)).ToArray()
                };

                var json = JsonSerializer.Serialize(skinPack, JsonOptions);
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting skin pack: {ex.Message}");
                return false;
            }
        }

        private static SerializableSkin ConvertToSerializable(Skin skin, string? description, string? author)
        {
            return new SerializableSkin
            {
                Name = skin.Name ?? "Unnamed Skin",
                Description = description ?? "",
                Author = author ?? "",
                PrimaryColor = skin.PrimaryColor.ToString(),
                SecondaryColor = skin.SecondaryColor.ToString(),
                AccentColor = skin.AccentColor.ToString(),
                PrimaryBackground = skin.PrimaryBackground.ToString(),
                SecondaryBackground = skin.SecondaryBackground.ToString(),
                PrimaryTextColor = skin.PrimaryTextColor.ToString(),
                SecondaryTextColor = skin.SecondaryTextColor.ToString(),
                BorderColor = skin.BorderColor.ToString(),
                ErrorColor = skin.ErrorColor.ToString(),
                WarningColor = skin.WarningColor.ToString(),
                SuccessColor = skin.SuccessColor.ToString(),
                FontFamily = ParseFontFamily(skin.FontFamily.ToString()).ToString(),
                FontSizeSmall = skin.FontSizeSmall,
                FontSizeMedium = skin.FontSizeMedium,
                FontSizeLarge = skin.FontSizeLarge,
                FontWeight = skin.FontWeight.ToString(),
                BorderRadius = skin.BorderRadius,
                BorderThickness = new SerializableThickness
                {
                    Left = skin.BorderThickness.Left,
                    Top = skin.BorderThickness.Top,
                    Right = skin.BorderThickness.Right,
                    Bottom = skin.BorderThickness.Bottom
                }
            };
        }

        private static FontFamily ParseFontFamily(string fontFamily)
        {
            if (string.IsNullOrWhiteSpace(fontFamily))
                return FontFamily.Default;

            // Prevent problematic generic names being interpreted as relative URIs
            var safeFamilies = new[] { "serif", "sans-serif", "monospace" };
            foreach (var fam in safeFamilies)
            {
                if (fontFamily.Contains(fam))
                {
                    return FontFamily.Default;
                }
            }

            if (safeFamilies.Contains(fontFamily.Trim().ToLowerInvariant()))
            {
                return FontFamily.Default;
            }
                

            return new FontFamily(fontFamily);
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
                FontFamily = ParseFontFamily(serializableSkin.FontFamily),
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