using System;
using System.Collections.Generic;
using Avalonia.Accelerate.Icons;
using Avalonia.Accelerate.Icons.Tests;
using Avalonia.Accelerate.TestUtilities.Rendering;
using Avalonia.Media;
using Xunit;

namespace Avalonia.Accelerate.Icons.Tests
{
    [CollectionDefinition("AvaloniaTestCollection")]
    public class AvaloniaTestCollection: ICollectionFixture<TestAppFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}

	[Collection("AvaloniaTestCollection")]
    public class ApplicationIconsTests
    {
        private readonly ApplicationIcons _icons = new();

        [Theory]
        [InlineData("File", ApplicationIcons.File)]
        [InlineData("Folder", ApplicationIcons.Folder)]
        [InlineData("NonExistent", ApplicationIcons.File)] // fallback
        public void GetIcon_Returns_Correct_Geometry(string iconName, string expectedPath)
        {
            var geometry = _icons.GetIcon(iconName);
            Assert.NotNull(geometry);
            Assert.Equal(Geometry.Parse(expectedPath).ToString(), geometry.ToString());
        }

        [Theory]
        [InlineData("test.docx", ApplicationIcons.WordDocument)]
        [InlineData("test.xlsx", ApplicationIcons.ExcelDocument)]
        [InlineData("test.pptx", ApplicationIcons.PowerPointDocument)]
        [InlineData("test.pdf", ApplicationIcons.PdfDocument)]
        [InlineData("test.jpg", ApplicationIcons.Image)]
        [InlineData("test.mp4", ApplicationIcons.Video)]
        [InlineData("test.mp3", ApplicationIcons.Audio)]
        [InlineData("test.zip", ApplicationIcons.Archive)]
        [InlineData("test.cs", ApplicationIcons.Code)]
        [InlineData("test.unknown", ApplicationIcons.File)]
        [InlineData("", ApplicationIcons.File)]
        public void GetFileTypeIcon_Returns_Correct_Geometry(string fileName, string expectedPath)
        {
            var geometry = _icons.GetFileTypeIcon(fileName);
            Assert.NotNull(geometry);
            Assert.Equal(Geometry.Parse(expectedPath).ToString(), geometry.ToString());
        }

        [Fact]
        public void GetFolderIcon_Returns_Folder_When_Accessible()
        {
            var geometry = _icons.GetFolderIcon(true);
            Assert.NotNull(geometry);
            Assert.Equal(Geometry.Parse(ApplicationIcons.Folder).ToString(), geometry.ToString());
        }
        


        [Fact]
        public void GetFolderIcon_Returns_Lock_When_Not_Accessible()
        {
            var geometry = _icons.GetFolderIcon(false);
            Assert.NotNull(geometry);
            Assert.Equal(Geometry.Parse(ApplicationIcons.Lock).ToString(), geometry.ToString());
        }

        [Fact]
        public void GetAvailableIcons_Returns_All_Keys()
        {
            var icons = _icons.GetAvailableIcons();
            Assert.NotNull(icons);
            // Should contain at least the known keys
            Assert.Contains("File", icons);
            Assert.Contains("Folder", icons);
            Assert.Contains("Search", icons);
        }

        [Theory]
        [InlineData("File", true)]
        [InlineData("Folder", true)]
        [InlineData("NonExistent", false)]
        public void HasIcon_Returns_Correct_Value(string iconName, bool expected)
        {
            Assert.Equal(expected, _icons.HasIcon(iconName));
        }

        [Theory]
        [InlineData("test.docx", ApplicationIcons.WordDocument)]
        [InlineData("test.xlsx", ApplicationIcons.ExcelDocument)]
        [InlineData("test.pptx", ApplicationIcons.PowerPointDocument)]
        [InlineData("test.pdf", ApplicationIcons.PdfDocument)]
        [InlineData("test.jpg", ApplicationIcons.Image)]
        [InlineData("test.mp4", ApplicationIcons.Video)]
        [InlineData("test.mp3", ApplicationIcons.Audio)]
        [InlineData("test.zip", ApplicationIcons.Archive)]
        [InlineData("test.cs", ApplicationIcons.Code)]
        [InlineData("test.unknown", ApplicationIcons.File)]
        [InlineData("", ApplicationIcons.File)]
        public void GetFileTypePathData_Returns_Correct_Path(string fileName, string expectedPath)
        {
            var path = ApplicationIcons.GetFileTypePathData(fileName);
            Assert.Equal(expectedPath, path);
        }

        [Fact]
        public void CreateIconPath_Creates_Path_With_Correct_Data()
        {
            var path = _icons.CreateIconPath("File", 24);
            Assert.NotNull(path);
            Assert.Equal(24, path.Width);
            Assert.Equal(24, path.Height);
            Assert.Equal(Geometry.Parse(ApplicationIcons.File).ToString(), path.Data?.ToString());
        }


		[Fact]
		public void Render_All_Icons_As_PNG_Snapshots()
        {
            var outputDir = Path.Combine("TestOutputs", "Icons");
            Directory.CreateDirectory(outputDir);

            foreach (var iconKey in _icons.GetAvailableIcons())
            {
                var path = _icons.CreateIconPath(iconKey, 48);
                var filePath = Path.Combine(outputDir, $"{iconKey}.png");

                VisualUtility.SaveVisualToPng(path, filePath);
                Assert.True(File.Exists(filePath), $"Missing snapshot: {filePath}");
            }
        }

	}
}
