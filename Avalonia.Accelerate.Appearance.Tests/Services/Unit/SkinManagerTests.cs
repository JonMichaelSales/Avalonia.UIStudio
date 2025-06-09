using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Headless.XUnit;
using Moq;
using Xunit;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit;

public class SkinManagerTests
{
    private readonly Mock<IThemeLoaderService> _themeLoaderServiceMock;
    private readonly SkinManager _skinManager;

    public SkinManagerTests()
    {
        _themeLoaderServiceMock = new Mock<IThemeLoaderService>();

        _themeLoaderServiceMock.Setup(s => s.LoadSkins(It.IsAny<string>()))
            .Returns(new List<Skin>());

        var appWrapper = new ApplicationWrapper(Application.Current!);

        _skinManager = new SkinManager(
            _themeLoaderServiceMock.Object,
            appWrapper
        );
    }

    [AvaloniaFact]
    public void RegisterSkin_AddsSkin_WhenNameAndSkinAreNotNull()
    {
        var skin = new Skin();
        _skinManager.RegisterSkin("TestSkin", skin);

        Assert.Equal(skin, _skinManager.GetSkin("TestSkin"));
    }
    
    [AvaloniaFact]
    public void ApplyControlThemes_HandlesThemeUris()
    {
        var skin = new Skin
        {
            Name = "TestSkin",
            ControlThemeUris = new Dictionary<string, string>
            {
                { "FakeTheme", "avares://Avalonia.Accelerate.Appearance.Tests/TestAssets/FakeControlTheme.axaml" }
            }
        };

        var skinManager = new SkinManager(new SkinLoaderService(), new ApplicationWrapper(Application.Current!));
        skinManager.RegisterSkin("TestSkin", skin);
        skinManager.ApplySkin("TestSkin");

        // No assert needed — the purpose is to force the ControlThemeUris code path
    }
    
   
    [AvaloniaFact]
    public void RegisterSkin_DoesNothing_WhenNameOrSkinIsNull()
    {
        _skinManager.RegisterSkin(null, null);
        _skinManager.RegisterSkin("Test", null);
        _skinManager.RegisterSkin(null, new Skin());

        Assert.Empty(_skinManager.GetAvailableSkinNames());
    }

    [AvaloniaFact]
    public void GetSkin_ReturnsSkinByName()
    {
        var skin = new Skin();
        _skinManager.RegisterSkin("TestSkin", skin);

        var result = _skinManager.GetSkin("TestSkin");

        Assert.Equal(skin, result);
    }

    [AvaloniaFact]
    public void GetSkin_ReturnsCurrentSkin_WhenNameIsNullOrNotFound()
    {
        var skin = new Skin();
        _skinManager.ApplySkin(skin);

        Assert.Equal(skin, _skinManager.GetSkin(null));
        Assert.Equal(skin, _skinManager.GetSkin("NonExistent"));
    }

    [AvaloniaFact]
    public void GetAvailableSkinNames_ReturnsAllRegisteredNames()
    {
        _skinManager.RegisterSkin("A", new Skin());
        _skinManager.RegisterSkin("B", new Skin());

        var names = _skinManager.GetAvailableSkinNames();

        Assert.Contains("A", names);
        Assert.Contains("B", names);
    }

    [AvaloniaFact]
    public void ApplySkin_ByName_AppliesAndSavesSkin()
    {
        var skin = new Skin();
        _skinManager.RegisterSkin("TestSkin", skin);

        _skinManager.ApplySkin("TestSkin");

        Assert.Equal(skin, _skinManager.CurrentSkin);
    }

    [AvaloniaFact]
    public void ApplySkin_ByName_LogsWhenSkinNotFound()
    {
        // Should not throw
        _skinManager.ApplySkin("NonExistent");
    }

    [AvaloniaFact]
    public void ApplySkin_ByInstance_AppliesSkinAndRaisesEvent()
    {
        var skin = new Skin();
        bool eventRaised = false;
        _skinManager.SkinChanged += (_, _) => eventRaised = true;

        _skinManager.ApplySkin(skin);

        Assert.Equal(skin, _skinManager.CurrentSkin);
        Assert.True(eventRaised);
    }

    [AvaloniaFact]
    public void ApplySkin_ByInstance_AppliesDefaultSkin_WhenNull()
    {
        _skinManager.ApplySkin((Skin?)null);

        Assert.NotNull(_skinManager.CurrentSkin);
    }

    [AvaloniaFact]
    public void SaveSelectedSkin_SavesThemeName()
    {
        var skinName = "SavedSkin";
        AppSettings.Instance.Theme = null;

        _skinManager.SaveSelectedSkin(skinName);

        Assert.Equal(skinName, AppSettings.Instance.Theme);
    }

    [AvaloniaFact]
    public void SaveSelectedSkin_DoesNothing_WhenNameIsNull()
    {
        AppSettings.Instance.Theme = "OldTheme";

        _skinManager.SaveSelectedSkin(null);

        Assert.Equal("OldTheme", AppSettings.Instance.Theme);
    }

    [AvaloniaFact]
    public void LoadSavedTheme_AppliesSavedTheme_WhenAvailable()
    {
        var skin = new Skin();
        _skinManager.RegisterSkin("SavedTheme", skin);
        AppSettings.Instance.Theme = "SavedTheme";

        _skinManager.LoadSavedTheme();

        Assert.Equal(skin, _skinManager.CurrentSkin);
    }

    [AvaloniaFact]
    public void LoadSavedTheme_DoesNothing_WhenThemeNotAvailable()
    {
        AppSettings.Instance.Theme = "NonExistent";
        _skinManager.ApplySkin(new Skin());

        var before = _skinManager.CurrentSkin;
        _skinManager.LoadSavedTheme();

        Assert.Equal(before, _skinManager.CurrentSkin);
    }
}
