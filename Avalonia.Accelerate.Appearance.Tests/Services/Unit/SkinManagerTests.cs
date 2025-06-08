using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Styling;
using Moq;
using Xunit;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit
{
    public class SkinManagerTests
    {
        private readonly Mock<IThemeLoaderService> _themeLoaderServiceMock;
        private readonly Mock<IApplication> _applicationMock;
        private readonly Mock<IStylesCollection> _stylesCollectionMock;
        private readonly Mock<IResourceDictionary> _resourceDictionaryMock;
        private readonly SkinManager _skinManager;

        public SkinManagerTests()
        {

            _themeLoaderServiceMock = new Mock<IThemeLoaderService>();
            _applicationMock = new Mock<IApplication>();
            _stylesCollectionMock = new Mock<IStylesCollection>();
            _resourceDictionaryMock = new Mock<IResourceDictionary>();

            _applicationMock.Setup(a => a.AppStyles).Returns(_stylesCollectionMock.Object);
            var resourceStore = new Dictionary<object, object>();

            _resourceDictionaryMock.Setup(d => d[It.IsAny<object>()])
                .Returns((object key) => resourceStore.ContainsKey(key) ? resourceStore[key] : null);

            _resourceDictionaryMock.SetupSet(d => d[It.IsAny<object>()] = It.IsAny<object>())
                .Callback<object, object>((key, value) => resourceStore[key] = value);

            _applicationMock.Setup(a => a.Resources).Returns(_resourceDictionaryMock.Object);
            _themeLoaderServiceMock.Setup(s => s.LoadSkins(It.IsAny<string>()))
                .Returns(new List<Skin>());

            _skinManager = new SkinManager(
                _themeLoaderServiceMock.Object,
                _applicationMock.Object
            );
        }

        [Fact]
        public void RegisterSkin_AddsSkin_WhenNameAndSkinAreNotNull()
        {
            var skin = new Skin();
            _skinManager.RegisterSkin("TestSkin", skin);

            Assert.Equal(skin, _skinManager.GetSkin("TestSkin"));
        }

        [Fact]
        public void RegisterSkin_DoesNothing_WhenNameOrSkinIsNull()
        {
            _skinManager.RegisterSkin(null, null);
            _skinManager.RegisterSkin("Test", null);
            _skinManager.RegisterSkin(null, new Skin());

            Assert.Empty(_skinManager.GetAvailableSkinNames());
        }

        [Fact]
        public void GetSkin_ReturnsSkinByName()
        {
            var skin = new Skin();
            _skinManager.RegisterSkin("TestSkin", skin);

            var result = _skinManager.GetSkin("TestSkin");

            Assert.Equal(skin, result);
        }

        [Fact]
        public void GetSkin_ReturnsCurrentSkin_WhenNameIsNullOrNotFound()
        {
            var skin = new Skin();
            _skinManager.ApplySkin(skin);

            Assert.Equal(skin, _skinManager.GetSkin(null));
            Assert.Equal(skin, _skinManager.GetSkin("NonExistent"));
        }

        [Fact]
        public void GetAvailableSkinNames_ReturnsAllRegisteredNames()
        {
            _skinManager.RegisterSkin("A", new Skin());
            _skinManager.RegisterSkin("B", new Skin());

            var names = _skinManager.GetAvailableSkinNames();

            Assert.Contains("A", names);
            Assert.Contains("B", names);
        }

        [Fact]
        public void ApplySkin_ByName_AppliesAndSavesSkin()
        {
            var skin = new Skin();
            _skinManager.RegisterSkin("TestSkin", skin);

            _skinManager.ApplySkin("TestSkin");

            Assert.Equal(skin, _skinManager.CurrentSkin);
        }

        [Fact]
        public void ApplySkin_ByName_LogsWhenSkinNotFound()
        {
            // Should not throw
            _skinManager.ApplySkin("NonExistent");
        }

        [Fact]
        public void ApplySkin_ByInstance_AppliesSkinAndRaisesEvent()
        {
            var skin = new Skin();
            bool eventRaised = false;
            _skinManager.SkinChanged += (_, _) => eventRaised = true;

            _skinManager.ApplySkin(skin);

            Assert.Equal(skin, _skinManager.CurrentSkin);
            Assert.True(eventRaised);
        }

        [Fact]
        public void ApplySkin_ByInstance_AppliesDefaultSkin_WhenNull()
        {
            _skinManager.ApplySkin((Skin?)null);

            Assert.NotNull(_skinManager.CurrentSkin);
        }

        [Fact]
        public void SaveSelectedSkin_SavesThemeName()
        {
            var skinName = "SavedSkin";
            AppSettings.Instance.Theme = null;

            _skinManager.SaveSelectedSkin(skinName);

            Assert.Equal(skinName, AppSettings.Instance.Theme);
        }

        [Fact]
        public void SaveSelectedSkin_DoesNothing_WhenNameIsNull()
        {
            AppSettings.Instance.Theme = "OldTheme";

            _skinManager.SaveSelectedSkin(null);

            Assert.Equal("OldTheme", AppSettings.Instance.Theme);
        }

        [Fact]
        public void LoadSavedTheme_AppliesSavedTheme_WhenAvailable()
        {
            var skin = new Skin();
            _skinManager.RegisterSkin("SavedTheme", skin);
            AppSettings.Instance.Theme = "SavedTheme";

            _skinManager.LoadSavedTheme();

            Assert.Equal(skin, _skinManager.CurrentSkin);
        }

        [Fact]
        public void LoadSavedTheme_DoesNothing_WhenThemeNotAvailable()
        {
            AppSettings.Instance.Theme = "NonExistent";
            _skinManager.ApplySkin(new Skin());

            var before = _skinManager.CurrentSkin;
            _skinManager.LoadSavedTheme();

            Assert.Equal(before, _skinManager.CurrentSkin);
        }
    }
}
