using Avalonia;
using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using System.Text.Json;
using Xunit;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit;

public class SkinImportExportServiceTests
{
    private readonly ISkinImportExportService _skinImportExportService;
    public SkinImportExportServiceTests()
    {
        _skinImportExportService = new SkinImportExportService();
    }
    private Skin CreateTestSkin()
    {
        return new Skin
        {
            Name = "TestSkin",
            PrimaryColor = Color.Parse("#FFFFFF"),
            SecondaryColor = Color.Parse("#EEEEEE"),
            AccentColor = Color.Parse("#0078D4"),
            PrimaryBackground = Color.Parse("#000000"),
            SecondaryBackground = Color.Parse("#222222"),
            PrimaryTextColor = Color.Parse("#FFFFFF"),
            SecondaryTextColor = Color.Parse("#BBBBBB"),
            BorderColor = Color.Parse("#888888"),
            ErrorColor = Color.Parse("#FF0000"),
            WarningColor = Color.Parse("#FFA500"),
            SuccessColor = Color.Parse("#00FF00"),
            FontFamily = new FontFamily("Segoe UI"),
            FontSizeSmall = 12,
            FontSizeMedium = 14,
            FontSizeLarge = 18,
            FontWeight = FontWeight.Normal,
            BorderRadius = 4,
            BorderThickness = new Thickness(1)
        };
    }

    [AvaloniaFact]
    public async Task ExportSkinAsync_WritesFileAndReturnsTrue()
    {
        var skin = CreateTestSkin();
        var filePath = Path.GetTempFileName();

        var result = await _skinImportExportService.ExportSkinAsync(skin, filePath, "desc", "author");

        Assert.True(result);
        var json = await File.ReadAllTextAsync(filePath);
        Assert.Contains("TestSkin", json);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ExportSkinAsync_ReturnsFalseOnException()
    {
        var skin = CreateTestSkin();
        var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "file.json");

        var result = await _skinImportExportService.ExportSkinAsync(skin, filePath);

        Assert.False(result);
    }

    [AvaloniaFact]
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

        var result = await _skinImportExportService.ExportAdvancedSkinAsync(skin, filePath, "desc", "author");

        Assert.True(result);
        var json = await File.ReadAllTextAsync(filePath);
        Assert.Contains("TestSkin", json);
        Assert.Contains("advancedTypography", json);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ExportInheritableSkinAsync_WritesFileAndReturnsTrue()
    {
        var skin = new InheritableSkin
        {
            Name = "InheritSkin",
            BaseSkinName = "Base",
            PropertyOverrides = new Dictionary<string, object> { { "PrimaryColor", "#FF0000" } }
        };
        var filePath = Path.GetTempFileName();

        var result = await _skinImportExportService.ExportInheritableSkinAsync(skin, filePath, "desc", "author");

        Assert.True(result);
        var json = await File.ReadAllTextAsync(filePath);
        Assert.Contains("InheritSkin", json);
        Assert.Contains("Base", json);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ImportSkinAsync_ReturnsSkinImportResult_Success()
    {
        var skin = CreateTestSkin();
        var filePath = Path.GetTempFileName();
        await _skinImportExportService.ExportSkinAsync(skin, filePath);

        var result = await _skinImportExportService.ImportSkinAsync(filePath);
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

    [AvaloniaFact]
    public async Task ImportSkinAsync_ReturnsError_WhenFileMissing()
    {
        var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "nofile.json");

        var result = await _skinImportExportService.ImportSkinAsync(filePath);
        if (!result.Success)
        {
            Console.WriteLine($"Error Message: {result.ErrorMessage}");
            Console.WriteLine($"Validation Warnings: {string.Join(", ", result.Warnings)}");
        }
        Assert.False(result.Success);
        Assert.Contains("does not exist", result.ErrorMessage);
    }

    [AvaloniaFact]
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
        await _skinImportExportService.ExportAdvancedSkinAsync(skin, filePath);

        var result = await _skinImportExportService.ImportAdvancedSkinAsync(filePath);

        Assert.NotNull(result);
        Assert.Equal("TestSkin", result.Name);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ImportInheritableSkinAsync_ReturnsInheritableSkin_WhenValid()
    {
        var skin = new InheritableSkin
        {
            Name = "InheritSkin",
            BaseSkinName = "Base",
            PropertyOverrides = new Dictionary<string, object> { { "PrimaryColor", "#FF0000" } }
        };
        var filePath = Path.GetTempFileName();
        await _skinImportExportService.ExportInheritableSkinAsync(skin, filePath);

        var result = await _skinImportExportService.ImportInheritableSkinAsync(filePath);

        Assert.NotNull(result);
        Assert.Equal("InheritSkin", result.Name);
        Assert.Equal("Base", result.BaseSkinName);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ValidateSkinFileAsync_ReturnsValidResult_WhenValid()
    {
        var skin = CreateTestSkin();
        var filePath = Path.GetTempFileName();
        await _skinImportExportService.ExportSkinAsync(skin, filePath);

        var result = await _skinImportExportService.ValidateSkinFileAsync(filePath);

        Assert.True(result.IsValid);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ValidateSkinFileAsync_ReturnsError_WhenFileMissing()
    {
        var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "nofile.json");

        var result = await _skinImportExportService.ValidateSkinFileAsync(filePath);

        Assert.False(result.IsValid);
        Assert.Contains("does not exist", result.Errors[0]);
    }

    [AvaloniaFact]
    public async Task ExportSkinPackAsync_WritesFileAndReturnsTrue()
    {
        var skin1 = CreateTestSkin();
        var skin2 = CreateTestSkin();
        skin2.Name = "TestSkin2";
        if (skin1.Name != null)
        {
            var themes = new Dictionary<string, Skin>
            {
                { skin1.Name, skin1 },
                { skin2.Name, skin2 }
            };
            var filePath = Path.GetTempFileName();

            var result = await _skinImportExportService.ExportSkinPackAsync(themes, filePath, "PackName", "desc");

            Assert.True(result);
            var json = await File.ReadAllTextAsync(filePath);
            Assert.Contains("PackName", json);
            Assert.Contains("TestSkin2", json);
            File.Delete(filePath);
        }
    }

    [AvaloniaFact]
    public async Task ImportAdvancedSkinAsync_ReturnsNull_OnInvalidJson()
    {
        var filePath = Path.GetTempFileName();
        await File.WriteAllTextAsync(filePath, "Not a JSON!");

        var result = await _skinImportExportService.ImportAdvancedSkinAsync(filePath);

        Assert.Null(result);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ImportInheritableSkinAsync_ReturnsNull_WhenFileMissing()
    {
        var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "nofile.json");

        var result = await _skinImportExportService.ImportInheritableSkinAsync(filePath);

        Assert.Null(result);
    }

    [AvaloniaFact]
    public async Task ExportAdvancedSkinAsync_Works_WhenTypographyIsNull()
    {
        var skin = CreateTestSkin(); // Your existing CreateTestSkin method
        skin.Typography = null;

        var filePath = Path.GetTempFileName();

        var result = await _skinImportExportService.ExportAdvancedSkinAsync(skin, filePath);

        Assert.True(result);
        var json = await File.ReadAllTextAsync(filePath);
        Assert.Contains("TestSkin", json);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ExportInheritableSkinAsync_Works_WhenPropertyOverridesIsEmpty()
    {
        var skin = new InheritableSkin
        {
            Name = "InheritSkin",
            BaseSkinName = "Base",
            PropertyOverrides = new Dictionary<string, object>()
        };
        var filePath = Path.GetTempFileName();

        var result = await _skinImportExportService.ExportInheritableSkinAsync(skin, filePath);

        Assert.True(result);
        var json = await File.ReadAllTextAsync(filePath);
        Assert.Contains("InheritSkin", json);
        File.Delete(filePath);
    }


    [AvaloniaFact]
    public async Task ValidateSkinFileAsync_ReturnsError_OnInvalidJson()
    {
        var filePath = Path.GetTempFileName();
        await File.WriteAllTextAsync(filePath, "Invalid JSON content");

        var result = await _skinImportExportService.ValidateSkinFileAsync(filePath);

        Assert.False(result.IsValid);
        Assert.Contains("Invalid JSON format", result.Errors.FirstOrDefault());
        File.Delete(filePath);
    }


    [AvaloniaFact]
    public async Task ImportSkinAsync_ReturnsError_OnInvalidJson()
    {
        var filePath = Path.GetTempFileName();
        await File.WriteAllTextAsync(filePath, "Invalid JSON content");

        var result = await _skinImportExportService.ImportSkinAsync(filePath);

        Assert.False(result.Success);
        Assert.Contains("JSON parsing error", result.ErrorMessage);
        File.Delete(filePath);
    }

    [AvaloniaFact]
    public async Task ImportSkinAsync_ReturnsValidationError_WhenNameMissing()
    {
        var badSkin = new SerializableSkin
        {
            Name = null, // Should trigger "Skin name is required"
            PrimaryColor = "#FFFFFF",
            SecondaryColor = "#FFFFFF",
            AccentColor = "#FFFFFF",
            PrimaryBackground = "#FFFFFF",
            SecondaryBackground = "#FFFFFF",
            PrimaryTextColor = "#FFFFFF",
            SecondaryTextColor = "#FFFFFF",
            BorderColor = "#FFFFFF",
            ErrorColor = "#FFFFFF",
            WarningColor = "#FFFFFF",
            SuccessColor = "#FFFFFF",
            FontFamily = "Segoe UI",
            FontSizeSmall = 12,
            FontSizeMedium = 14,
            FontSizeLarge = 18,
            FontWeight = "Normal",
            BorderRadius = 4,
            BorderThickness = new SerializableThickness { Left = 1, Top = 1, Right = 1, Bottom = 1 }
        };

        var filePath = Path.GetTempFileName();
        var json = JsonSerializer.Serialize(badSkin);
        await File.WriteAllTextAsync(filePath, json);

        var result = await _skinImportExportService.ImportSkinAsync(filePath);

        Assert.False(result.Success);
        Assert.Contains("Skin validation failed", result.ErrorMessage);
        File.Delete(filePath);
    }


}
