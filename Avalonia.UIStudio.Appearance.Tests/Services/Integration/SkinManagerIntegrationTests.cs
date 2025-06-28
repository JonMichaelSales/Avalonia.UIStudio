using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;

namespace Avalonia.UIStudio.Appearance.Tests.Services.Integration
{
    public class SkinManagerIntegrationTests
    {

        private readonly SkinManager? _skinManager;


        public SkinManagerIntegrationTests()
        {
            var app = Application.Current!;
            var skinLoaderService = new SkinLoaderService();
            var registry = new SkinRegistryService(skinLoaderService);
            var applier = new SkinApplierService((IApplication)Application.Current);
            var persistence = new SkinPersistenceService(registry, applier);

            _skinManager = new SkinManager(registry, applier, persistence);
        }

        [AvaloniaFact]
        public void ApplySkin_PopulatesRequiredResourceKeys()
        {
            // Arrange
            var skin = new Skin
            {
                Name = "TestSkin",
                PrimaryColor = Colors.Red,
                SecondaryColor = Colors.Blue,
                AccentColor = Colors.Green,
                PrimaryBackground = Colors.Gray,
                SecondaryBackground = Colors.LightGray,
                PrimaryTextColor = Colors.White,
                SecondaryTextColor = Colors.Black,
                BorderColor = Colors.Yellow,
                ErrorColor = Colors.Orange,
                WarningColor = Colors.Purple,
                SuccessColor = Colors.Teal,
                FontFamily = "Arial",
                FontSizeSmall = 10,
                FontSizeMedium = 12,
                FontSizeLarge = 14,
                BorderThickness = new Thickness(1,1,1,1),
                BorderRadius = 4,
                Typography = new TypographyScale()
                {
                    BodyLarge = 16, 
                    BodyMedium = 14, 
                    BodySmall = 12,
                    DisplayLarge = 57,
                    DisplayMedium = 45,
                    DisplaySmall = 36,
                    HeadlineLarge = 32,
                    HeadlineMedium = 28,
                    HeadlineSmall = 24,
                    TitleLarge = 22,
                    TitleMedium = 16,
                    TitleSmall = 14,
                    LabelLarge = 14,
                    LabelMedium = 12,
                    LabelSmall = 11,
                }
            };

            _skinManager?.RegisterSkin(skin.Name, skin);

            // Act
            _skinManager?.ApplySkin(skin);

            // Assert — verify keys exist in Application.Current.Resources
            var requiredKeys = new[]
            {
                "PrimaryColorBrush",
                "SecondaryColorBrush",
                "AccentBlueBrush",
                "BackgroundBrush",
                "BackgroundLightBrush",
                "BackgroundDarkBrush",
                "TextPrimaryBrush",
                "TextSecondaryBrush",
                "BorderBrush",
                "ErrorBrush",
                "WarningBrush",
                "SuccessBrush",
                "DefaultFontFamily",
                "FontSizeSmall",
                "FontSizeMedium",
                "FontSizeLarge",
                "DefaultFontWeight",
                "BorderThickness",
                "CornerRadius",
                "BodyLargeFontSize", 
                "BodyMediumFontSize", 
                "BodySmallFontSize",
                "DisplayLargeFontSize", 
                "DisplayMediumFontSize", 
                "DisplaySmallFontSize", 
                "HeadlineLargeFontSize",
                "HeadlineMediumFontSize", 
                "HeadlineSmallFontSize", 
                "TitleLargeFontSize", 
                "TitleMediumFontSize",
                "TitleSmallFontSize", 
                "LabelLargeFontSize", 
                "LabelMediumFontSize", 
                "LabelSmallFontSize",
                "BodyLargeFontSize", 
                "BodyMediumFontSize", 
                "BodySmallFontSize",
            };

            foreach (var key in requiredKeys)
            {
                Assert.True(Application.Current != null && Application.Current.Resources.ContainsKey(key), $"Missing resource key: {key}");
            }
        }

        [AvaloniaFact]
        public void ApplySkin_SetsExpectedPrimaryColorBrush()
        {
            // Arrange
            var skin = new Skin
            {
                Name = "TestSkin",
                PrimaryColor = Colors.Red,
                SecondaryColor = Colors.Blue,
                AccentColor = Colors.Green,
                PrimaryBackground = Colors.Gray,
                SecondaryBackground = Colors.LightGray,
                PrimaryTextColor = Colors.White,
                SecondaryTextColor = Colors.Black,
                BorderColor = Colors.Yellow,
                ErrorColor = Colors.Orange,
                WarningColor = Colors.Purple,
                SuccessColor = Colors.Teal,
                FontFamily = "Arial",
                FontSizeSmall = 10,
                FontSizeMedium = 12,
                FontSizeLarge = 14,
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderRadius = 4,
                Typography = new TypographyScale()
                {
                    BodyLarge = 16,
                    BodyMedium = 14,
                    BodySmall = 12,
                    DisplayLarge = 57,
                    DisplayMedium = 45,
                    DisplaySmall = 36,
                    HeadlineLarge = 32,
                    HeadlineMedium = 28,
                    HeadlineSmall = 24,
                    TitleLarge = 22,
                    TitleMedium = 16,
                    TitleSmall = 14,
                    LabelLarge = 14,
                    LabelMedium = 12,
                    LabelSmall = 11,
                }
            };


            _skinManager?.RegisterSkin(skin.Name, skin);

            // Act
            _skinManager?.ApplySkin(skin);

            // Assert
            Assert.True(Application.Current != null && Application.Current.Resources.ContainsKey("PrimaryColorBrush"), "Missing PrimaryColorBrush");

            var brush = Application.Current.Resources["PrimaryColorBrush"] as SolidColorBrush;
            Assert.NotNull(brush);
            Assert.Equal(Colors.Red, brush.Color);
        }

        [AvaloniaFact]
        public void ApplySkin_UpdatesPrimaryColorBrush()
        {
            // Arrange
            var skin1 = new Skin
            {
                Name = "Skin1",
                PrimaryColor = Colors.Red,
                SecondaryColor = Colors.Blue,
                AccentColor = Colors.Green,
                PrimaryBackground = Colors.Gray,
                SecondaryBackground = Colors.LightGray,
                PrimaryTextColor = Colors.White,
                SecondaryTextColor = Colors.Black,
                BorderColor = Colors.Yellow,
                ErrorColor = Colors.Orange,
                WarningColor = Colors.Purple,
                SuccessColor = Colors.Teal,
                FontFamily = "Arial",
                FontSizeSmall = 10,
                FontSizeMedium = 12,
                FontSizeLarge = 14,
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderRadius = 4,
                Typography = new TypographyScale()
                {
                    BodyLarge = 16,
                    BodyMedium = 14,
                    BodySmall = 12,
                    DisplayLarge = 57,
                    DisplayMedium = 45,
                    DisplaySmall = 36,
                    HeadlineLarge = 32,
                    HeadlineMedium = 28,
                    HeadlineSmall = 24,
                    TitleLarge = 22,
                    TitleMedium = 16,
                    TitleSmall = 14,
                    LabelLarge = 14,
                    LabelMedium = 12,
                    LabelSmall = 11,
                }
            };


            var skin2 = new Skin
            {
                Name = "Skin2",
                PrimaryColor = Colors.Blue,
                SecondaryColor = Colors.Red,
                AccentColor = Colors.Green,
                PrimaryBackground = Colors.Gray,
                SecondaryBackground = Colors.LightGray,
                PrimaryTextColor = Colors.White,
                SecondaryTextColor = Colors.Black,
                BorderColor = Colors.Yellow,
                ErrorColor = Colors.Orange,
                WarningColor = Colors.Purple,
                SuccessColor = Colors.Teal,
                FontFamily = "Arial",
                FontSizeSmall = 10,
                FontSizeMedium = 12,
                FontSizeLarge = 14,
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderRadius = 4,
                Typography = new TypographyScale()
                {
                    BodyLarge = 16,
                    BodyMedium = 14,
                    BodySmall = 12,
                    DisplayLarge = 57,
                    DisplayMedium = 45,
                    DisplaySmall = 36,
                    HeadlineLarge = 32,
                    HeadlineMedium = 28,
                    HeadlineSmall = 24,
                    TitleLarge = 22,
                    TitleMedium = 16,
                    TitleSmall = 14,
                    LabelLarge = 14,
                    LabelMedium = 12,
                    LabelSmall = 11,
                }
            };


            _skinManager?.RegisterSkin(skin1.Name, skin1);
            _skinManager?.RegisterSkin(skin2.Name, skin2);

            // Act — Apply first skin
            _skinManager?.ApplySkin(skin1);
            var firstBrush = Application.Current?.Resources["PrimaryColorBrush"] as SolidColorBrush;
            Assert.NotNull(firstBrush);
            Assert.Equal(Colors.Red, firstBrush.Color);

            // Act — Apply second skin
            _skinManager?.ApplySkin(skin2);
            var secondBrush = Application.Current?.Resources["PrimaryColorBrush"] as SolidColorBrush;
            Assert.NotNull(secondBrush);
            Assert.Equal(Colors.Blue, secondBrush.Color);
        }


    }
}
