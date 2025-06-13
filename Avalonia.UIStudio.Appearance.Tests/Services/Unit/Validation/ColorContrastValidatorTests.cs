using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services.ValidationRules;

namespace Avalonia.UIStudio.Appearance.Tests.Services.Unit.Validation;

public class ColorContrastValidationRuleTests
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
            BorderThickness = new Thickness(1),
            BorderRadius = 5,
            ErrorColor = Colors.Red,
            WarningColor = Colors.Orange,
            SuccessColor = Colors.Green
        };
    }

    [AvaloniaFact]
    public void Validate_ReturnsNoErrorsOrWarnings_ForGoodContrast()
    {
        var rule = new ColorContrastValidationRule();
        var skin = CreateDefaultSkin();
        skin.PrimaryTextColor = Colors.Black;
        skin.PrimaryBackground = Colors.White;
        skin.SecondaryTextColor = Colors.Black;
        skin.SecondaryBackground = Colors.WhiteSmoke;
        skin.AccentColor = Color.FromRgb(0, 200, 255); // bright cyan → excellent contrast

        var messages = rule.Validate(skin);

        Assert.Empty(messages.Where(m => m.IsError));
        Assert.Empty(messages.Where(m => !m.IsError));
    }

    [AvaloniaFact]
    public void ValidatePrimaryTextContrast_AddsError_WhenBelowAA()
    {
        var rule = new ColorContrastValidationRule();
        var skin = CreateDefaultSkin();
        skin.PrimaryTextColor = Colors.Gray;
        skin.PrimaryBackground = Colors.LightGray;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Primary text contrast ratio") && e.Contains("below WCAG AA"));
    }

    [AvaloniaFact]
    public void ValidatePrimaryTextContrast_AddsWarning_WhenBelowAAA()
    {
        var rule = new ColorContrastValidationRule();
        var skin = CreateDefaultSkin();

        // Known stable pair for 4.5–7.0 contrast
        skin.PrimaryTextColor = Color.FromRgb(85, 85, 85); // Medium-dark gray
        skin.PrimaryBackground = Color.FromRgb(230, 230, 230);

        skin.SecondaryTextColor = Colors.Black;
        skin.SecondaryBackground = Colors.White;
        skin.AccentColor = Colors.Orange;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message),
            w => w.Contains("Primary text contrast ratio") && w.Contains("below WCAG AAA"));
    }

    [AvaloniaFact]
    public void ValidateSecondaryTextContrast_AddsError_WhenBelowMinimum()
    {
        var rule = new ColorContrastValidationRule();
        var skin = CreateDefaultSkin();
        skin.SecondaryTextColor = Colors.Gray;
        skin.SecondaryBackground = Colors.LightGray;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Secondary text contrast ratio") && e.Contains("below minimum"));
    }

    [AvaloniaFact]
    public void ValidateAccentContrast_AddsWarning_WhenLowContrastWithPrimaryText()
    {
        var rule = new ColorContrastValidationRule();
        var skin = CreateDefaultSkin();
        skin.PrimaryTextColor = Color.FromRgb(55, 55, 55);
        skin.AccentColor = Color.FromRgb(80, 80, 80);

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message),
            w => w.Contains("Accent color contrast") && w.Contains("may be difficult to read"));
    }
}
