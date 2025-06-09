using Avalonia;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Accelerate.Appearance.Services.ValidationRules;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Xunit;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit.Validation;

public class FontSizeValidationRuleTests
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
            FontSizeSmall = 12,
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
    public void Validate_ReturnsNoErrors_ForValidFontSizes()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();

        var result = rule.Validate(skin);

        Assert.Empty(result.Errors);
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenSmallFontTooSmall()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeSmall = 6; // Below 8

        var result = rule.Validate(skin);

        Assert.Contains(result.Errors, e => e.Contains("Small font size") && e.Contains("between 8 and 20"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenSmallFontTooLarge()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeSmall = 22; // Above 20

        var result = rule.Validate(skin);

        Assert.Contains(result.Errors, e => e.Contains("Small font size") && e.Contains("between 8 and 20"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenMediumFontTooSmall()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeMedium = 8; // Below 10

        var result = rule.Validate(skin);

        Assert.Contains(result.Errors, e => e.Contains("Medium font size") && e.Contains("between 10 and 24"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenMediumFontTooLarge()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeMedium = 26; // Above 24

        var result = rule.Validate(skin);

        Assert.Contains(result.Errors, e => e.Contains("Medium font size") && e.Contains("between 10 and 24"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenLargeFontTooSmall()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeLarge = 10; // Below 12

        var result = rule.Validate(skin);

        Assert.Contains(result.Errors, e => e.Contains("Large font size") && e.Contains("between 12 and 32"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenLargeFontTooLarge()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeLarge = 40; // Above 32

        var result = rule.Validate(skin);

        Assert.Contains(result.Errors, e => e.Contains("Large font size") && e.Contains("between 12 and 32"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenSmallFontNotSmallerThanMedium()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeSmall = 16; // Equal to medium

        var result = rule.Validate(skin);

        Assert.Contains(result.Errors, e => e.Contains("Small font size should be smaller than medium font size"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenMediumFontNotSmallerThanLarge()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeMedium = 24; // Equal to large

        var result = rule.Validate(skin);

        Assert.Contains(result.Errors, e => e.Contains("Medium font size should be smaller than large font size"));
    }
}
