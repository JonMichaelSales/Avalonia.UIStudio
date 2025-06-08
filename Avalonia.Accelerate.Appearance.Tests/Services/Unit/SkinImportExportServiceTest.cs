using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Media;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit
{
    public class SkinImportExportTests
    {
        private Skin CreateTestSkin()
        {
            return new Skin
            {
                Name = "TestSkin",
                PrimaryColor = Color.Parse("#FFFFFF"), // white
                SecondaryColor = Color.Parse("#EEEEEE"), // light gray
                AccentColor = Color.Parse("#0078D4"), // blue
                PrimaryBackground = Color.Parse("#000000"), // black → guarantees contrast
                SecondaryBackground = Color.Parse("#222222"), // dark gray
                PrimaryTextColor = Color.Parse("#FFFFFF"), // white → full contrast
                SecondaryTextColor = Color.Parse("#BBBBBB"), // light gray, but passes 3:1
                BorderColor = Color.Parse("#888888"), // sufficient contrast
                ErrorColor = Color.Parse("#FF0000"), // strong red → passes contrast
                WarningColor = Color.Parse("#FFA500"), // orange
                SuccessColor = Color.Parse("#00FF00"), // green
                FontFamily = new FontFamily("Segoe UI"),
                FontSizeSmall = 12, // >= 12px required!
                FontSizeMedium = 14,
                FontSizeLarge = 18,
                FontWeight = FontWeight.Normal,
                BorderRadius = 4,
                BorderThickness = new Thickness(1)
            };
        }


        [Fact]
        public async Task ExportSkinAsync_WritesFileAndReturnsTrue()
        {
            var skin = CreateTestSkin();
            var filePath = Path.GetTempFileName();

            var result = await SkinImportExport.ExportSkinAsync(skin, filePath, "desc", "author");

            Assert.True(result);
            var json = await File.ReadAllTextAsync(filePath);
            Assert.Contains("TestSkin", json);
            File.Delete(filePath);
        }

        [Fact]
        public async Task ExportSkinAsync_ReturnsFalseOnException()
        {
            var skin = CreateTestSkin();
            // Invalid path to force exception
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "file.json");

            var result = await SkinImportExport.ExportSkinAsync(skin, filePath);

            Assert.False(result);
        }

        [Fact]
        public async Task ExportAdvancedSkinAsync_WritesFileAndReturnsTrue()
        {
            var skin = CreateTestSkin();
            skin.Typography = new TypographyScale();
            skin.HeaderFontFamily = new FontFamily("Arial");
            skin.BodyFontFamily = new FontFamily("Arial");
            skin.MonospaceFontFamily = new FontFamily("Consolas");
            skin.LineHeight = 1.2;
            skin.LetterSpacing = 0.1;
            skin.EnableLigatures = true;

            var filePath = Path.GetTempFileName();

            var result = await SkinImportExport.ExportAdvancedSkinAsync(skin, filePath, "desc", "author");

            Assert.True(result);
            var json = await File.ReadAllTextAsync(filePath);
            Assert.Contains("TestSkin", json);
            Assert.Contains("advancedTypography", json);
            File.Delete(filePath);
        }

        [Fact]
        public async Task ExportInheritableSkinAsync_WritesFileAndReturnsTrue()
        {
            var skin = new InheritableSkin
            {
                Name = "InheritSkin",
                BaseSkinName = "Base",
                PropertyOverrides = new Dictionary<string, object> { { "PrimaryColor", "#FF0000" } }
            };
            var filePath = Path.GetTempFileName();

            var result = await SkinImportExport.ExportInheritableSkinAsync(skin, filePath, "desc", "author");

            Assert.True(result);
            var json = await File.ReadAllTextAsync(filePath);
            Assert.Contains("InheritSkin", json);
            Assert.Contains("Base", json);
            File.Delete(filePath);
        }

        [Fact]
        public async Task ImportSkinAsync_ReturnsSkinImportResult_Success()
        {
            var skin = CreateTestSkin();
            var filePath = Path.GetTempFileName();
            await SkinImportExport.ExportSkinAsync(skin, filePath);

            var result = await SkinImportExport.ImportSkinAsync(filePath);
            if (!result.Success)
            {
                Console.WriteLine($"Error Message: {result.ErrorMessage}");
                Console.WriteLine($"Validation Warnings: {string.Join(", ", result.Warnings)}");
            }
            Assert.True(result.Success);
            Assert.NotNull(result.Skin);
            Assert.Equal("TestSkin", result.Skin.Name);
            File.Delete(filePath);
        }

        [Fact]
        public async Task ImportSkinAsync_ReturnsError_WhenFileMissing()
        {
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "nofile.json");

            var result = await SkinImportExport.ImportSkinAsync(filePath);
            if (!result.Success)
            {
                Console.WriteLine($"Error Message: {result.ErrorMessage}");
                Console.WriteLine($"Validation Warnings: {string.Join(", ", result.Warnings)}");
            }
            Assert.False(result.Success);
            Assert.Contains("does not exist", result.ErrorMessage);
        }

        [Fact]
        public async Task ImportAdvancedSkinAsync_ReturnsSkin_WhenValid()
        {
            var skin = CreateTestSkin();
            skin.Typography = new TypographyScale();
            skin.HeaderFontFamily = new FontFamily("Arial");
            skin.BodyFontFamily = new FontFamily("Arial");
            skin.MonospaceFontFamily = new FontFamily("Consolas");
            skin.LineHeight = 1.2;
            skin.LetterSpacing = 0.1;
            skin.EnableLigatures = true;

            var filePath = Path.GetTempFileName();
            await SkinImportExport.ExportAdvancedSkinAsync(skin, filePath);

            var result = await SkinImportExport.ImportAdvancedSkinAsync(filePath);

            Assert.NotNull(result);
            Assert.Equal("TestSkin", result.Name);
            File.Delete(filePath);
        }

        [Fact]
        public async Task ImportInheritableSkinAsync_ReturnsInheritableSkin_WhenValid()
        {
            var skin = new InheritableSkin
            {
                Name = "InheritSkin",
                BaseSkinName = "Base",
                PropertyOverrides = new Dictionary<string, object> { { "PrimaryColor", "#FF0000" } }
            };
            var filePath = Path.GetTempFileName();
            await SkinImportExport.ExportInheritableSkinAsync(skin, filePath);

            var result = await SkinImportExport.ImportInheritableSkinAsync(filePath);

            Assert.NotNull(result);
            Assert.Equal("InheritSkin", result.Name);
            Assert.Equal("Base", result.BaseSkinName);
            File.Delete(filePath);
        }

        [Fact]
        public async Task ValidateSkinFileAsync_ReturnsValidResult_WhenValid()
        {
            var skin = CreateTestSkin();
            var filePath = Path.GetTempFileName();
            await SkinImportExport.ExportSkinAsync(skin, filePath);

            var result = await SkinImportExport.ValidateSkinFileAsync(filePath);

            Assert.True(result.IsValid);
            File.Delete(filePath);
        }

        [Fact]
        public async Task ValidateSkinFileAsync_ReturnsError_WhenFileMissing()
        {
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "nofile.json");

            var result = await SkinImportExport.ValidateSkinFileAsync(filePath);

            Assert.False(result.IsValid);
            Assert.Contains("does not exist", result.Errors[0]);
        }

        [Fact]
        public async Task ExportSkinPackAsync_WritesFileAndReturnsTrue()
        {
            var skin1 = CreateTestSkin();
            var skin2 = CreateTestSkin();
            skin2.Name = "TestSkin2";
            var themes = new Dictionary<string, Skin>
            {
                { skin1.Name, skin1 },
                { skin2.Name, skin2 }
            };
            var filePath = Path.GetTempFileName();

            var result = await SkinImportExport.ExportSkinPackAsync(themes, filePath, "PackName", "desc");

            Assert.True(result);
            var json = await File.ReadAllTextAsync(filePath);
            Assert.Contains("PackName", json);
            Assert.Contains("TestSkin2", json);
            File.Delete(filePath);
        }
    }

}