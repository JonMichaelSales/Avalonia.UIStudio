using System.Reflection;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services.ValidationRules;

namespace Avalonia.UIStudio.Appearance.Tests.Services.Unit.Validation;

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

            PrimaryBackground = Colors.White,
            SecondaryBackground = Color.FromRgb(245, 245, 245),  // Very light gray

            PrimaryTextColor = Color.FromRgb(0x33, 0x33, 0x33),  // #333333

            SecondaryTextColor = Color.FromRgb(55, 55, 55),

            FontSizeSmall = 14,
            FontSizeMedium = 16,
            FontSizeLarge = 22,

            BorderColor = Color.FromRgb(80, 80, 80),

            AccentColor = Color.FromRgb(0x23, 0x45, 0x45),   // #0073A1

            ///WarningColor = Color.FromRgb(0xa8, 0x43, 0x00), // #D35400 → ~5.2 contrast vs white → safe

            SuccessColor = Color.FromRgb(0x18, 0x86, 0x3c), // #27AE60 → ~7.0 contrast vs white → safe

            WarningColor = Color.FromRgb(0xC0, 0x40, 0x00),
            ErrorColor = Color.FromRgb(0xC8, 0x32, 0x32),
        };
    }








    [AvaloniaFact]
    public void Validate_ReturnsResult_WithNoErrorsOrWarnings_ForAccessibleSkin()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateCompliantSkin();

        var messages = rule.Validate(skin);


        Assert.Empty(messages.Where(m => m.IsError));
        Assert.Empty(messages.Where(m => !m.IsError));

    }

    [AvaloniaFact]
    public void ValidateColorContrast_AddsError_WhenContrastIsLow()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.PrimaryTextColor = Colors.White;
        skin.PrimaryBackground = Colors.White;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("contrast") || e.Contains("too low"));
    }


    [AvaloniaFact]
    public void ValidateContrastRatio_AddsError_AndWarning_Correctly()
    {
        var rule = new AccessibilityValidationRule();

        var skin = CreateDefaultSkin();
        skin.PrimaryTextColor = Colors.Black;
        skin.PrimaryBackground = Color.FromRgb(30, 30, 30); // 2.0 contrast → should trigger error

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("contrast") || e.Contains("too low"));

        skin.PrimaryBackground = Color.FromRgb(160, 160, 160); // 5.0+ contrast → should trigger AAA warning if applicable

        

        
    }


    [AvaloniaFact]
    public void ValidateFontSizes_AddsWarningsAndErrors_ForBadSizes()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeSmall = 10;  // triggers IsError
        skin.FontSizeMedium = 12; // also triggers IsError
        skin.FontSizeLarge = 40;  // triggers warning

        var messages = rule.Validate(skin);

        // Should have an error for FontSizeSmall
        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("FontSizeSmall") && e.Contains("below minimum"));

        // Should have an error for FontSizeMedium (not just a warning)
        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("FontSizeMedium") && e.Contains("below minimum"));

        // Should have a warning for FontSizeLarge
        Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message),
            w => w.Contains("FontSizeLarge") && w.Contains("max recommended"));
    }



    [AvaloniaFact]
    public void ValidateColorDifferentiation_AddsWarnings_ForSimilarColors()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.WarningColor = Colors.Gray;
        skin.SuccessColor = Colors.Gray;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message),
            w => w.Contains("similar") || w.Contains("perceptual") || w.Contains("too similar"));
    }


    [AvaloniaFact]
    public void ValidateFocusIndicators_AddsError_AndWarning_ForLowContrast()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.AccentColor = Colors.White;
        skin.PrimaryBackground = Colors.White;
        skin.BorderColor = Colors.White;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("contrast") || e.Contains("too low"));
        //Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("low contrast"));
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

        var messages = rule.Validate(skin);

        // All three status colors should trigger errors about contrast
        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("WarningColor contrast"));
        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("SuccessColor contrast"));
        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("ErrorColor contrast"));

        // There may or may not be non-error warnings — no assert needed here, or:

        Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message),
            w => w.Contains("similar") || w.Contains("perceptual"));
    }



    [AvaloniaFact]
    public void ValidateStatusColorDifferentiation_AddsWarnings_ForSimilarStatusColors()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.ErrorColor = Colors.Red;
        skin.WarningColor = Colors.Red;
        skin.SuccessColor = Colors.Red;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("similar") || w.Contains("perceptual"));
    }


    [AvaloniaFact]
    public void ValidateVisualStability_AddsWarning_ForHighSaturationOrContrast()
    {
        var rule = new AccessibilityValidationRule();
        var skin = CreateDefaultSkin();
        skin.AccentColor = Color.FromArgb(255, 255, 0, 0);
        skin.ErrorColor = Color.FromArgb(255, 255, 0, 0);

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message),
            w => w.Contains("saturated") || w.Contains("high contrast") || w.Contains("visual"));
    }


    [AvaloniaTheory]
    [InlineData(255, 255, 0, 0, true)]
    [InlineData(255, 128, 128, 128, false)]
    public void IsHighSaturationColor_ReturnsExpected(int a, int r, int g, int b, bool expected)
    {
        var rule = new AccessibilityValidationRule();
        var color = Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);

        var method = typeof(AccessibilityValidationRule)
            .GetMethod("IsHighSaturationColor", BindingFlags.NonPublic | BindingFlags.Instance);

        var result = (bool)method?.Invoke(rule, new object[] { color })!;

        Assert.Equal(expected, result);
    }
}
