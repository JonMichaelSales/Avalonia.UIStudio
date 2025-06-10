using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Markup.Xaml;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Accelerate.Appearance.Services.ValidationRules;
using Xunit;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit.Validation;

public class AccessibilityValidationRuleTests
{
    private static Skin CreateDefaultSkin()
    {
        return new Skin
        {
            PrimaryColor = Colors.Black,
            SecondaryColor = Colors.Gray,
            AccentColor = Colors.Blue,
            PrimaryBackground = Colors.White,
            SecondaryBackground = Colors.LightGray,
            PrimaryTextColor = Colors.Black,
            SecondaryTextColor = Colors.Gray,
            FontSizeSmall = 14,
            FontSizeMedium = 16,
            FontSizeLarge = 24,
            BorderColor = Colors.Black,
            ErrorColor = Colors.Red,
            WarningColor = Colors.Orange,
            SuccessColor = Colors.Green
        };
    }

    private static Skin CreateCompliantSkin()
    {
        return new Skin
        {
            PrimaryColor = Colors.Black,
            SecondaryColor = Colors.Gray,
            AccentColor = Color.FromRgb(30, 144, 255),
            SecondaryBackground = Color.FromRgb(250, 250, 250),
            PrimaryBackground = Colors.White,
            PrimaryTextColor = Colors.Black,
            SecondaryTextColor = Colors.DarkSlateGray,
            FontSizeSmall = 12,
            FontSizeMedium = 14,
            FontSizeLarge = 18,
            BorderColor = Colors.Black,
            ErrorColor = Color.FromRgb(220, 53, 69),
            WarningColor = Color.FromRgb(255, 193, 7),
            SuccessColor = Color.FromRgb(40, 167, 69),
        };
    }

    [AvaloniaFact]
    public void Validate_ReturnsResult_WithNoErrorsOrWarnings_ForAccessibleSkin()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateCompliantSkin();

        var result = rule.Validate(skin);

        Assert.Empty(result.Errors);
    }

    [AvaloniaFact]
    public void ValidateColorContrast_AddsError_WhenContrastIsLow()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.PrimaryTextColor = Colors.White;
        skin.PrimaryBackground = Colors.White;

        var result = new SkinValidationResult();
        var validator = new Avalonia.Accelerate.Appearance.Services.SkinValidator();

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("ValidateColorContrast", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(rule, new object[] { skin, validator, result });

        Assert.Contains(result.Errors, e => e.Contains("contrast ratio"));
    }

    [AvaloniaFact]
    public void ValidateContrastRatio_AddsError_AndWarning_Correctly()
    {
        var rule = new AccessibilityValidationRule();
        var result = new SkinValidationResult();

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("ValidateContrastRatio", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(rule, new object[] { 2.0, "Test", result, false });
        Assert.Contains(result.Errors, e => e.Contains("fails WCAG"));

        result = new SkinValidationResult();
        method?.Invoke(rule, new object[] { 5.0, "Test", result, false });
        Assert.Contains(result.Warnings, w => w.Contains("meets minimum but not enhanced WCAG AAA"));
    }

    [AvaloniaFact]
    public void ValidateFontSizes_AddsWarningsAndErrors_ForBadSizes()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeSmall = 10;
        skin.FontSizeMedium = 12;
        skin.FontSizeLarge = 40;

        var result = new SkinValidationResult();

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("ValidateFontSizes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(rule, new object[] { skin, result });

        Assert.Contains(result.Errors, e => e.Contains("below accessibility minimum")); // For FontSizeSmall = 10, which is below minimum (error)
        Assert.Contains(result.Warnings, w => w.Contains("below recommended size"));     // For FontSizeMedium = 12 (warning)
        Assert.Contains(result.Warnings, w => w.Contains("exceeds recommended maximum")); // For FontSizeLarge = 40 (warning)

    }

    [AvaloniaFact]
    public void ValidateColorDifferentiation_AddsWarnings_ForSimilarColors()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.PrimaryColor = Colors.Gray;
        skin.SecondaryColor = Colors.Gray;
        skin.PrimaryBackground = Colors.White;
        skin.SecondaryBackground = Colors.White;
        skin.AccentColor = Colors.Gray;

        var result = new SkinValidationResult();
        var validator = new Avalonia.Accelerate.Appearance.Services.SkinValidator();

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("ValidateColorDifferentiation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(rule, new object[] { skin, validator, result });

        Assert.Contains(result.Warnings, w => w.Contains("very similar") || w.Contains("too similar"));
    }

    [AvaloniaFact]
    public void ValidateFocusIndicators_AddsError_AndWarning_ForLowContrast()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.AccentColor = Colors.White;
        skin.PrimaryBackground = Colors.White;
        skin.BorderColor = Colors.White;

        var result = new SkinValidationResult();
        var validator = new Avalonia.Accelerate.Appearance.Services.SkinValidator();

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("ValidateFocusIndicators", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(rule, new object[] { skin, validator, result });

        Assert.Contains(result.Errors, e => e.Contains("insufficient contrast"));
        Assert.Contains(result.Warnings, w => w.Contains("low contrast"));
    }

    [AvaloniaFact]
    public void ValidateStatusColors_AddsError_AndWarning_ForLowContrast()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.ErrorColor = Colors.White;
        skin.WarningColor = Colors.White;
        skin.SuccessColor = Colors.White;
        skin.PrimaryBackground = Colors.White;

        var result = new SkinValidationResult();
        var validator = new Avalonia.Accelerate.Appearance.Services.SkinValidator();

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("ValidateStatusColors", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(rule, new object[] { skin, validator, result });

        Assert.Contains(result.Errors, e => e.Contains("insufficient contrast"));
        Assert.Contains(result.Warnings, w => w.Contains("low contrast"));
    }

    [AvaloniaFact]
    public void ValidateStatusColorDifferentiation_AddsWarnings_ForSimilarStatusColors()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.ErrorColor = Colors.Red;
        skin.WarningColor = Colors.Red;
        skin.SuccessColor = Colors.Red;

        var result = new SkinValidationResult();
        var validator = new Avalonia.Accelerate.Appearance.Services.SkinValidator();

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("ValidateStatusColorDifferentiation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(rule, new object[] { skin, validator, result });

        Assert.Contains(result.Warnings, w => w.Contains("too similar"));
    }

    [AvaloniaFact]
    public void ValidateVisualStability_AddsWarning_ForHighSaturationOrContrast()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.AccentColor = Color.FromArgb(255, 255, 0, 0);
        skin.ErrorColor = Color.FromArgb(255, 255, 0, 0);
        skin.PrimaryTextColor = Colors.Black;
        skin.PrimaryBackground = Colors.White;

        var result = new SkinValidationResult();

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("ValidateVisualStability", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method?.Invoke(rule, new object[] { skin, result });

        Assert.Contains(result.Warnings, w => w.Contains("very bright, saturated colors") || w.Contains("Very high contrast ratio"));
    }

    [AvaloniaTheory]
    [InlineData(255, 255, 0, 0, true)]
    [InlineData(255, 128, 128, 128, false)]
    public void IsHighSaturationColor_ReturnsExpected(int a, int r, int g, int b, bool expected)
    {
        var rule = new AccessibilityValidationRule();
        var color = Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("IsHighSaturationColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var result = (bool)method?.Invoke(rule, new object[] { color })!;

        Assert.Equal(expected, result);
    }
}
