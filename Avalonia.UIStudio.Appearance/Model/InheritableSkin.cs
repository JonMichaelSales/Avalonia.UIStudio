using Avalonia.Media;
using System.Text.Json;

namespace Avalonia.UIStudio.Appearance.Model
{
    /// <summary>
    /// Represents a skin that can inherit from a base skin and override specific properties.
    /// </summary>
    public class InheritableSkin : Skin
    {
        private readonly HashSet<string> _setProperties = new();

        /// <summary>
        /// Gets or sets the name of the base skin this skin inherits from.
        /// </summary>
        public string? BaseSkinName { get; set; }

        /// <summary>
        /// Gets or sets the collection of property overrides for this skin.
        /// </summary>
        public Dictionary<string, object>? PropertyOverrides { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the InheritableSkin class.
        /// </summary>
        public InheritableSkin()
        {
            // Don't call base constructor to avoid setting default values
            // Initialize collections that are required
            ControlThemeUris = new Dictionary<string, string>();
            StyleUris = new();
            Typography = new TypographyScale();
            PropertyOverrides = new Dictionary<string, object>();
        }

        // Override property setters to track which properties have been explicitly set
        public new Color PrimaryColor
        {
            get => base.PrimaryColor;
            set
            {
                base.PrimaryColor = value;
                _setProperties.Add(nameof(PrimaryColor));
            }
        }

        public new Color SecondaryColor
        {
            get => base.SecondaryColor;
            set
            {
                base.SecondaryColor = value;
                _setProperties.Add(nameof(SecondaryColor));
            }
        }

        public new Color AccentColor
        {
            get => base.AccentColor;
            set
            {
                base.AccentColor = value;
                _setProperties.Add(nameof(AccentColor));
            }
        }

        public new Color PrimaryBackground
        {
            get => base.PrimaryBackground;
            set
            {
                base.PrimaryBackground = value;
                _setProperties.Add(nameof(PrimaryBackground));
            }
        }

        public new Color SecondaryBackground
        {
            get => base.SecondaryBackground;
            set
            {
                base.SecondaryBackground = value;
                _setProperties.Add(nameof(SecondaryBackground));
            }
        }

        public new Color PrimaryTextColor
        {
            get => base.PrimaryTextColor;
            set
            {
                base.PrimaryTextColor = value;
                _setProperties.Add(nameof(PrimaryTextColor));
            }
        }

        public new Color SecondaryTextColor
        {
            get => base.SecondaryTextColor;
            set
            {
                base.SecondaryTextColor = value;
                _setProperties.Add(nameof(SecondaryTextColor));
            }
        }

        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                base.FontFamily = value;
                _setProperties.Add(nameof(FontFamily));
            }
        }

        public new double FontSizeSmall
        {
            get => base.FontSizeSmall;
            set
            {
                base.FontSizeSmall = value;
                _setProperties.Add(nameof(FontSizeSmall));
            }
        }

        public new double FontSizeMedium
        {
            get => base.FontSizeMedium;
            set
            {
                base.FontSizeMedium = value;
                _setProperties.Add(nameof(FontSizeMedium));
            }
        }

        public new double FontSizeLarge
        {
            get => base.FontSizeLarge;
            set
            {
                base.FontSizeLarge = value;
                _setProperties.Add(nameof(FontSizeLarge));
            }
        }

        public new FontWeight FontWeight
        {
            get => base.FontWeight;
            set
            {
                base.FontWeight = value;
                _setProperties.Add(nameof(FontWeight));
            }
        }

        public new Color BorderColor
        {
            get => base.BorderColor;
            set
            {
                base.BorderColor = value;
                _setProperties.Add(nameof(BorderColor));
            }
        }

        public new Thickness BorderThickness
        {
            get => base.BorderThickness;
            set
            {
                base.BorderThickness = value;
                _setProperties.Add(nameof(BorderThickness));
            }
        }

        public new double BorderRadius
        {
            get => base.BorderRadius;
            set
            {
                base.BorderRadius = value;
                _setProperties.Add(nameof(BorderRadius));
            }
        }

        public new Color ErrorColor
        {
            get => base.ErrorColor;
            set
            {
                base.ErrorColor = value;
                _setProperties.Add(nameof(ErrorColor));
            }
        }

        public new Color WarningColor
        {
            get => base.WarningColor;
            set
            {
                base.WarningColor = value;
                _setProperties.Add(nameof(WarningColor));
            }
        }

        public new Color SuccessColor
        {
            get => base.SuccessColor;
            set
            {
                base.SuccessColor = value;
                _setProperties.Add(nameof(SuccessColor));
            }
        }

        public new string? Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                _setProperties.Add(nameof(Name));
            }
        }

        /// <summary>
        /// Creates a resolved skin by applying inheritance and overrides.
        /// </summary>
        /// <param name="baseSkin">The base skin to inherit from.</param>
        /// <returns>A fully resolved Skin with all properties applied.</returns>
        public Skin CreateResolvedSkin(Skin? baseSkin = null)
        {
            var resolved = new Skin();

            // Start with base skin if provided
            if (baseSkin != null)
            {
                CopyPropertiesFrom(resolved, baseSkin);
            }

            // Apply current skin's explicitly set properties only
            CopySetPropertiesFrom(resolved, this);

            // Apply property overrides
            ApplyOverrides(resolved);

            return resolved;
        }

        private void CopyPropertiesFrom(Skin target, Skin source)
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

        private void CopySetPropertiesFrom(Skin target, InheritableSkin source)
        {
            // Only copy properties that have been explicitly set
            if (_setProperties.Contains(nameof(PrimaryColor)))
                target.PrimaryColor = source.PrimaryColor;
            if (_setProperties.Contains(nameof(SecondaryColor)))
                target.SecondaryColor = source.SecondaryColor;
            if (_setProperties.Contains(nameof(AccentColor)))
                target.AccentColor = source.AccentColor;
            if (_setProperties.Contains(nameof(PrimaryBackground)))
                target.PrimaryBackground = source.PrimaryBackground;
            if (_setProperties.Contains(nameof(SecondaryBackground)))
                target.SecondaryBackground = source.SecondaryBackground;
            if (_setProperties.Contains(nameof(PrimaryTextColor)))
                target.PrimaryTextColor = source.PrimaryTextColor;
            if (_setProperties.Contains(nameof(SecondaryTextColor)))
                target.SecondaryTextColor = source.SecondaryTextColor;
            if (_setProperties.Contains(nameof(FontFamily)))
                target.FontFamily = source.FontFamily;
            if (_setProperties.Contains(nameof(FontSizeSmall)))
                target.FontSizeSmall = source.FontSizeSmall;
            if (_setProperties.Contains(nameof(FontSizeMedium)))
                target.FontSizeMedium = source.FontSizeMedium;
            if (_setProperties.Contains(nameof(FontSizeLarge)))
                target.FontSizeLarge = source.FontSizeLarge;
            if (_setProperties.Contains(nameof(FontWeight)))
                target.FontWeight = source.FontWeight;
            if (_setProperties.Contains(nameof(BorderColor)))
                target.BorderColor = source.BorderColor;
            if (_setProperties.Contains(nameof(BorderThickness)))
                target.BorderThickness = source.BorderThickness;
            if (_setProperties.Contains(nameof(BorderRadius)))
                target.BorderRadius = source.BorderRadius;
            if (_setProperties.Contains(nameof(ErrorColor)))
                target.ErrorColor = source.ErrorColor;
            if (_setProperties.Contains(nameof(WarningColor)))
                target.WarningColor = source.WarningColor;
            if (_setProperties.Contains(nameof(SuccessColor)))
                target.SuccessColor = source.SuccessColor;
            if (_setProperties.Contains(nameof(Name)))
                target.Name = source.Name;
        }

        private void ApplyOverrides(Skin target)
        {
            if (PropertyOverrides != null)
                foreach (var kvp in PropertyOverrides)
                {
                    var property = typeof(Skin).GetProperty(kvp.Key);
                    if (property != null && property.CanWrite)
                    {
                        try
                        {
                            var value = ConvertValue(kvp.Value, property.PropertyType);
                            property.SetValue(target, value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to apply override for {kvp.Key}: {ex.Message}");
                        }
                    }
                }
        }

        // Update the ConvertValue method in InheritableSkin.cs
        private object? ConvertValue(object? value, Type targetType)
        {
            if (value == null) return null;

            if (targetType == typeof(Color) && value is string colorString)
            {
                return Color.Parse(colorString);
            }

            if (targetType == typeof(FontFamily) && value is string fontString)
            {
                return new FontFamily(fontString);
            }

            if (targetType == typeof(FontWeight) && value is string fontWeightString)
            {
                return Enum.TryParse<FontWeight>(fontWeightString, out var weight)
                    ? weight
                    : FontWeight.Normal;
            }

            if (targetType == typeof(Thickness))
            {
                if (value is JsonElement element)
                {
                    if (element.ValueKind == JsonValueKind.Number)
                    {
                        return new Thickness(element.GetDouble());
                    }
                    else if (element.ValueKind == JsonValueKind.String)
                    {
                        return Thickness.Parse(element.GetString() ?? "0");
                    }
                    else if (element.ValueKind == JsonValueKind.Object)
                    {
                        var left = element.TryGetProperty("left", out var leftProp) ? leftProp.GetDouble() : 0;
                        var top = element.TryGetProperty("top", out var topProp) ? topProp.GetDouble() : 0;
                        var right = element.TryGetProperty("right", out var rightProp) ? rightProp.GetDouble() : 0;
                        var bottom = element.TryGetProperty("bottom", out var bottomProp) ? bottomProp.GetDouble() : 0;
                        return new Thickness(left, top, right, bottom);
                    }
                }
                else if (value is string thicknessString)
                {
                    return Thickness.Parse(thicknessString);
                }
            }

            if (targetType == typeof(CornerRadius))
            {
                if (value is JsonElement element)
                {
                    if (element.ValueKind == JsonValueKind.Number)
                    {
                        return new CornerRadius(element.GetDouble());
                    }
                    else if (element.ValueKind == JsonValueKind.String)
                    {
                        var radiusString = element.GetString() ?? "0";
                        return double.TryParse(radiusString, out var radius)
                            ? new CornerRadius(radius)
                            : new CornerRadius(0);
                    }
                }
                else if (value is string radiusString)
                {
                    return double.TryParse(radiusString, out var radius)
                        ? new CornerRadius(radius)
                        : new CornerRadius(0);
                }
            }

            // Try standard type conversion as fallback
            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }
        }
    }
}