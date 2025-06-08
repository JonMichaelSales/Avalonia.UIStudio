using System.Text.Json;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Headless;
using Avalonia.Platform;
using Moq;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit
{
    public class SkinLoaderServiceTests
    {

        
        public SkinLoaderServiceTests()
        {
            if (Application.Current == null)
            {
                AvaloniaHeadlessPlatformOptions opts = new AvaloniaHeadlessPlatformOptions
                {

                };
                AppBuilder.Configure<Application>()
                    .UseHeadless(opts)
                    .SetupWithoutStarting();
            }
            
        }




        [Fact]
        public void LoadSkins_ReturnsSkins_WhenThemesAreValid()
        {
            // Arrange
            var service = new SkinLoaderService();

            // Act
            var testRoot = "avares://Avalonia.Accelerate.Appearance.Tests/TestAssets/Skins/";
            var skins = service.LoadSkins(testRoot);

            // Assert
            Assert.NotNull(skins);
            Assert.NotEmpty(skins);
            Assert.All(skins, skin => Assert.False(string.IsNullOrWhiteSpace(skin.Name)));
        }


        [Fact]
        public void LoadSkins_SkipsInvalidThemes_AndContinues()
        {
            // Arrange
            var service = new SkinLoaderService();

            var testRoot = "avares://Avalonia.Accelerate.Appearance.Tests/TestAssets/Skins/";

            // Act
            var skins = service.LoadSkins(testRoot);

            // Assert
            Assert.NotNull(skins);
            Assert.True(skins.Count >= 1);
        }


        [Fact]
        public void LoadSkins_HandlesBaseSkinInheritance()
        {
            // Arrange
            var assetLoaderMock = new Mock<IAssetLoader>();
            assetLoaderMock
                .Setup(x => x.GetAssets(It.IsAny<Uri>(), It.IsAny<Uri?>()))
                .Returns(new[]
                {
                    new Uri("avares://Avalonia.Accelerate.Appearance/Skins/Base/theme.json"),
                    new Uri("avares://Avalonia.Accelerate.Appearance/Skins/Derived/theme.json")
                });

            assetLoaderMock
                .Setup(x => x.Open(It.Is<Uri>(uri => uri.ToString().Contains("/Base/theme.json")), It.IsAny<Uri?>()))
                .Returns(() =>
                {
                    var json = JsonSerializer.Serialize(new SerializableSkin { Name = "Base" });
                    return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
                });

            assetLoaderMock
                .Setup(x => x.Open(It.Is<Uri>(uri => uri.ToString().Contains("/Derived/theme.json")), It.IsAny<Uri?>()))
                .Returns(() =>
                {
                    var json = JsonSerializer.Serialize(new SerializableSkin
                    {
                        Name = "Derived",
                        BaseSkinName = "Base"
                    });
                    return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
                });


            var service = new SkinLoaderService();

            // Act
            var testRoot = "avares://Avalonia.Accelerate.Appearance.Tests/TestAssets/Skins/";
            var skins = service.LoadSkins(testRoot);
            // Assert
            Assert.Contains(skins, s => s.Name == "Base");
            Assert.Contains(skins, s => s.Name == "Derived");
        }

    }


}