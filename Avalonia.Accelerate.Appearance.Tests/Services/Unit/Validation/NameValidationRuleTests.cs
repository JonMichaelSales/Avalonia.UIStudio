using Avalonia;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services.ValidationRules;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Xunit;
using System.Linq;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit.Validation
{
    public class NameValidationRuleTests
    {
        private static Skin CreateSkinWithName(string? name)
        {
            return new Skin
            {
                Name = name,
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
        public void Validate_AddsError_WhenNameIsNullOrEmpty()
        {
            var rule = new NameValidationRule();

            var skin = CreateSkinWithName(null);
            var messages1 = rule.Validate(skin);
            Assert.Contains(messages1.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("empty or whitespace"));

            skin = CreateSkinWithName("");
            var messages2 = rule.Validate(skin);
            Assert.Contains(messages2.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("empty or whitespace"));

            skin = CreateSkinWithName("   ");
            var messages3 = rule.Validate(skin);
            Assert.Contains(messages3.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("empty or whitespace"));
        }

        [AvaloniaFact]
        public void Validate_AddsError_WhenNameHasInvalidCharacters()
        {
            var rule = new NameValidationRule();
            var skin = CreateSkinWithName("Name!@#");

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("contains invalid characters"));
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameHasLeadingOrTrailingWhitespace()
        {
            var rule = new NameValidationRule();
            var skin = CreateSkinWithName(" Name ");

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("leading or trailing whitespace"));
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameHasMultipleConsecutiveSpaces()
        {
            var rule = new NameValidationRule();
            var skin = CreateSkinWithName("My  Skin");

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("multiple consecutive spaces"));
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameStartsWithSpecialCharacter()
        {
            var rule = new NameValidationRule();

            var skin1 = CreateSkinWithName("-MySkin");
            var skin2 = CreateSkinWithName("_MySkin");
            var skin3 = CreateSkinWithName(".MySkin");

            Assert.Contains(rule.Validate(skin1).Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("starts with a special character"));
            Assert.Contains(rule.Validate(skin2).Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("starts with a special character"));
            Assert.Contains(rule.Validate(skin3).Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("starts with a special character"));
        }

        [AvaloniaFact]
        public void Validate_AddsError_WhenNameIsTooShortOrTooLong()
        {
            var rule = new NameValidationRule();

            var skin1 = CreateSkinWithName("A"); // too short
            var messages1 = rule.Validate(skin1);
            Assert.Contains(messages1.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("too short"));

            var longName = new string('A', 60); // too long
            var skin2 = CreateSkinWithName(longName);
            var messages2 = rule.Validate(skin2);
            Assert.Contains(messages2.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("is too long"));
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameIsQuiteLong()
        {
            var rule = new NameValidationRule();
            var skin = CreateSkinWithName(new string('A', 35));

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("quite long"));
        }

        [AvaloniaFact]
        public void Validate_AddsError_WhenNameIsReserved()
        {
            var rule = new NameValidationRule();
            string?[] reservedNames = new[] { "Default", "System", "Auto", "None", "Null", "Empty" };

            foreach (var reserved in reservedNames)
            {
                var skin = CreateSkinWithName(reserved);
                var messages = rule.Validate(skin);

                Assert.Contains(messages.Where(m => m.IsError).Select(m => m.Message), e => e.Contains("reserved name"));
            }
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameIsProblematic()
        {
            var rule = new NameValidationRule();
            string?[] problematicNames = new[] { "Test", "Debug", "Temp", "Sample" };

            foreach (var problematic in problematicNames)
            {
                var skin = CreateSkinWithName(problematic);
                var messages = rule.Validate(skin);

                Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("might be confusing"));
            }
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameHasFileSystemConflictPatterns()
        {
            var rule = new NameValidationRule();
            string?[] conflictNames = new[] { "con", "prn", "aux", "nul", "com1", "lpt1" };

            foreach (var conflict in conflictNames)
            {
                var skin = CreateSkinWithName(conflict);
                var messages = rule.Validate(skin);

                Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("file systems"));
            }
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenShortNameNotCapitalized()
        {
            var rule = new NameValidationRule();
            var skin = CreateSkinWithName("ab");

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("should be capitalized"));
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameContainsVersionNumbers()
        {
            var rule = new NameValidationRule();
            var skin = CreateSkinWithName("Skin v1.2");

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("version numbers"));
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameHasExcessiveCapitalization()
        {
            var rule = new NameValidationRule();
            var skin = CreateSkinWithName("BIGTHEMENAME");

            var messages = rule.Validate(skin);

            Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("excessive capitalization"));
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameEndsWithThemeOrSkin()
        {
            var rule = new NameValidationRule();

            var skin1 = CreateSkinWithName("MyTheme");
            var skin2 = CreateSkinWithName("MySkin");

            Assert.Contains(rule.Validate(skin1).Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("ends with 'theme' or 'skin'"));
            Assert.Contains(rule.Validate(skin2).Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("ends with 'theme' or 'skin'"));
        }

        [AvaloniaFact]
        public void Validate_AddsWarning_WhenNameIsGeneric()
        {
            var rule = new NameValidationRule();
            string?[] genericNames = new[] { "Theme", "Skin", "Custom", "New" };

            foreach (var generic in genericNames)
            {
                var skin = CreateSkinWithName(generic);
                var messages = rule.Validate(skin);

                Assert.Contains(messages.Where(m => !m.IsError).Select(m => m.Message), w => w.Contains("too generic"));
            }
        }
    }
}
