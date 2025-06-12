using Avalonia;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services.ValidationRules;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Xunit;
using System.Linq;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit.Validation
{
    public class BorderValidationRuleTests
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
                BorderColor = Colors.Blue,
                BorderThickness = new Thickness(1),
                BorderRadius = 5,
                ErrorColor = Colors.Red,
                WarningColor = Colors.Orange,
                SuccessColor = Colors.Green
            };
        }

        [AvaloniaFact]
        public void Validate_ReturnsNoErrorsOrWarnings_ForGoodBorderSettings()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();

            var messages = rule.Validate(skin);

            Assert.Empty(messages.Where(m => m.IsError));
            Assert.Empty(messages.Where(m => !m.IsError));
        }

        [AvaloniaFact]
        public void ValidateBorderThickness_AddsError_WhenNegative()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderThickness = new Thickness(-1, 2, 3, 4);

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("cannot be negative"));
        }

        [AvaloniaFact]
        public void ValidateBorderThickness_AddsWarning_WhenTooLarge()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderThickness = new Thickness(2, 12, 2, 2); // Max is 12 > 10

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("very large"));
        }

        [AvaloniaFact]
        public void ValidateBorderThickness_AddsWarning_WhenZero()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderThickness = new Thickness(0, 0, 0, 0);

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("borders will be invisible"));
        }

        [AvaloniaFact]
        public void ValidateBorderRadius_AddsError_WhenNegative()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderRadius = -5;

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("cannot be negative"));
        }

        [AvaloniaFact]
        public void ValidateBorderRadius_AddsWarning_WhenTooLarge()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderRadius = 60;

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("very large"));
        }

        [AvaloniaFact]
        public void ValidateBorderRadius_AddsWarning_WhenVerySmall()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderRadius = 0.5;

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("very small"));
        }

        [AvaloniaFact]
        public void ValidateBorderColorContrast_AddsError_WhenInsufficientContrast_PrimaryBackground()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderColor = Colors.White;
            skin.PrimaryBackground = Colors.White;

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("insufficient contrast against primary background"));
        }

        [AvaloniaFact]
        public void ValidateBorderColorContrast_AddsError_WhenInsufficientContrast_SecondaryBackground()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderColor = Colors.LightGray;
            skin.SecondaryBackground = Colors.LightGray;

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("insufficient contrast against secondary background"));
        }

        [AvaloniaFact]
        public void ValidateBorderColorContrast_AddsWarning_WhenLowContrast_PrimaryBackground()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderColor = Color.FromRgb(235, 235, 235);
            skin.PrimaryBackground = Color.FromRgb(245, 245, 245);

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("insufficient contrast against primary background"));
        }

        [AvaloniaFact]
        public void ValidateBorderColorContrast_AddsWarning_WhenLowContrast_SecondaryBackground()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderColor = Color.FromRgb(220, 220, 220);
            skin.SecondaryBackground = Color.FromRgb(240, 240, 240);

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("insufficient contrast against secondary background"));
        }

        [AvaloniaFact]
        public void ValidateBorderColorContrast_AddsWarning_WhenSimilarToPrimaryText()
        {
            var rule = new BorderValidationRule();
            var skin = CreateDefaultSkin();
            skin.BorderColor = Colors.Black;
            skin.PrimaryTextColor = Colors.Black;

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("very similar to primary text color"));
        }
    }
}
