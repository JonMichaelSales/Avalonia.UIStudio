using Avalonia;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services.ValidationRules;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Xunit;
using System.Linq;

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

        var messages = rule.Validate(skin);

        Assert.Empty(messages.Where(m => m.IsError));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenSmallFontTooSmall()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeSmall = 6;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Small font size") && e.Contains("between 8 and 20"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenSmallFontTooLarge()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeSmall = 22;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Small font size") && e.Contains("between 8 and 20"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenMediumFontTooSmall()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeMedium = 8;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Medium font size") && e.Contains("between 10 and 24"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenMediumFontTooLarge()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeMedium = 26;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Medium font size") && e.Contains("between 10 and 24"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenLargeFontTooSmall()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeLarge = 10;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Large font size") && e.Contains("between 12 and 32"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenLargeFontTooLarge()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeLarge = 40;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Large font size") && e.Contains("between 12 and 32"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenSmallFontNotSmallerThanMedium()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeSmall = 16;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Small font size should be smaller than medium font size"));
    }

    [AvaloniaFact]
    public void Validate_AddsError_WhenMediumFontNotSmallerThanLarge()
    {
        var rule = new FontSizeValidationRule();
        var skin = CreateDefaultSkin();
        skin.FontSizeMedium = 24;

        var messages = rule.Validate(skin);

        Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message),
            e => e.Contains("Medium font size should be smaller than large font size"));
    }
}
