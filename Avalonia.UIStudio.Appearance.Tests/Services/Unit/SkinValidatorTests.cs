using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;

namespace Avalonia.UIStudio.Appearance.Tests.Services.Unit;

public class SkinValidatorTests
{
    private Skin CreateTestSkinWithBadContrast()
    {
        return new Skin
        {
            Name = null, // should be fixed
            PrimaryColor = Colors.Black,
            SecondaryColor = Colors.Gray,
            AccentColor = Colors.Blue,
            PrimaryBackground = Colors.Black,
            SecondaryBackground = Colors.Black,
            PrimaryTextColor = Colors.Black, // bad contrast → should be fixed
            SecondaryTextColor = Colors.Black, // bad contrast → should be fixed
            BorderColor = Colors.Gray,
            ErrorColor = Colors.Red,
            WarningColor = Colors.Orange,
            SuccessColor = Colors.Green,
            FontSizeSmall = 4, // should be clamped to 8
            FontSizeMedium = 50, // should be clamped to 24
            FontSizeLarge = 100, // should be clamped to 32
            BorderRadius = -5, // should be clamped to 0
            BorderThickness = new Thickness(1)
        };
    }

    [AvaloniaFact]
    public void ValidateSkin_ReturnsExpectedResult()
    {
        var skin = CreateTestSkinWithBadContrast();
        var validator = new SkinValidator();

        var result = validator.ValidateSkin(skin);

        Assert.NotNull(result);
        Assert.False(result.IsValid); // Should have errors
    }

    [AvaloniaFact]
    public void AutoFixSkin_FixesIssues()
    {
        var skin = CreateTestSkinWithBadContrast();
        var validator = new SkinValidator();

        var fixedSkin = validator.AutoFixSkin(skin);

        Assert.NotNull(fixedSkin);
        Assert.NotSame(skin, fixedSkin); // CloneSkin used

        // Name should be set
        Assert.Equal("Custom Skin", fixedSkin.Name);

        // Font sizes should be clamped
        Assert.InRange(fixedSkin.FontSizeSmall, 8, 20);
        Assert.InRange(fixedSkin.FontSizeMedium, 10, 24);
        Assert.InRange(fixedSkin.FontSizeLarge, 12, 32);

        // BorderRadius should be >= 0
        Assert.True(fixedSkin.BorderRadius >= 0);

        // PrimaryTextColor contrast should be >= 4.5
        var primaryContrast = validator.CalculateContrastRatio(fixedSkin.PrimaryTextColor, fixedSkin.PrimaryBackground);
        Assert.True(primaryContrast >= 4.5);

        // SecondaryTextColor contrast should be >= 3.0
        var secondaryContrast = validator.CalculateContrastRatio(fixedSkin.SecondaryTextColor, fixedSkin.SecondaryBackground);
        Assert.True(secondaryContrast >= 3.0);
    }

    [AvaloniaFact]
    public void CalculateContrastRatio_ReturnsExpectedValues()
    {
        var validator = new SkinValidator();

        // Black on white → max contrast
        var contrast = validator.CalculateContrastRatio(Colors.Black, Colors.White);
        Assert.True(contrast > 10);

        // Identical colors → ratio = 1
        var contrast2 = validator.CalculateContrastRatio(Colors.Gray, Colors.Gray);
        Assert.Equal(1.0, contrast2);
    }

    [AvaloniaFact]
    public void AdjustColorForContrast_ImprovesContrast()
    {
        var validator = new SkinValidator();

        var badColor = Colors.Black;
        var background = Colors.Black;

        var adjusted = validator.AutoFixSkin(new Skin
        {
            Name = "FixContrastTest",
            PrimaryBackground = background,
            PrimaryTextColor = badColor,
            SecondaryBackground = background,
            SecondaryTextColor = badColor,
            FontSizeSmall = 12,
            FontSizeMedium = 14,
            FontSizeLarge = 18,
            BorderRadius = 4,
            BorderThickness = new Thickness(1),
            BorderColor = Colors.Gray,
            ErrorColor = Colors.Red,
            WarningColor = Colors.Orange,
            SuccessColor = Colors.Green
        });

        // After AutoFixTheme, contrast should be fixed
        var primaryContrast = validator.CalculateContrastRatio(adjusted.PrimaryTextColor, adjusted.PrimaryBackground);
        Assert.True(primaryContrast >= 4.5);

        var secondaryContrast = validator.CalculateContrastRatio(adjusted.SecondaryTextColor, adjusted.SecondaryBackground);
        Assert.True(secondaryContrast >= 3.0);
    }
}
