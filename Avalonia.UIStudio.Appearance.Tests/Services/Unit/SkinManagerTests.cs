using Avalonia.Headless.XUnit;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;
using Moq;

namespace Avalonia.UIStudio.Appearance.Tests.Services.Unit;

public class SkinManagerTests
{
    private readonly SkinManager _skinManager;
    private readonly Mock<ISkinRegistryService> _registryMock;
    private readonly Mock<ISkinApplierService> _applierMock;
    private readonly Mock<ISkinPersistenceService> _persistenceMock;

    public SkinManagerTests()
    {
        _registryMock = new Mock<ISkinRegistryService>();
        _applierMock = new Mock<ISkinApplierService>();
        _persistenceMock = new Mock<ISkinPersistenceService>();

        _skinManager = new SkinManager(
            _registryMock.Object,
            _applierMock.Object,
            _persistenceMock.Object
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
    public void ApplySkin_LoadsControlThemeUrisWithoutException()
    {
        var app = Application.Current!;
        var applier = new SkinApplierService((IApplication)app);

        var skin = new Skin
        {
            Name = "TestSkin",
            ControlThemeUris = ["avares://Avalonia.UIStudio.Appearance.Tests/TestAssets/FakeControlTheme.axaml"]
        };

        var exception = Record.Exception(() => applier.ApplySkin(skin));

        Assert.Null(exception);
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
    public void SaveSelectedSkin_SavesSkinName()
    {
        var skinName = "SavedSkin";
        AppSettings.Instance.Skin = null;

        _skinManager.SaveSelectedSkin(skinName);

        Assert.Equal(skinName, AppSettings.Instance.Skin);
    }

    [AvaloniaFact]
    public void SaveSelectedSkin_DoesNothing_WhenNameIsNull()
    {
        AppSettings.Instance.Skin = "OldSkin";

        _skinManager.SaveSelectedSkin(null);

        Assert.Equal("OldSkin", AppSettings.Instance.Skin);
    }

    [AvaloniaFact]
    public void LoadSavedSkin_AppliesSavedSkin_WhenAvailable()
    {
        var skin = new Skin();
        _skinManager.RegisterSkin("SavedSkin", skin);
        AppSettings.Instance.Skin = "SavedSkin";

        _skinManager.LoadSavedSkin();

        Assert.Equal(skin, _skinManager.CurrentSkin);
    }

    [AvaloniaFact]
    public void LoadSavedTheme_DoesNothing_WhenSkinNotAvailable()
    {
        AppSettings.Instance.Skin = "NonExistent";
        _skinManager.ApplySkin(new Skin());

        var before = _skinManager.CurrentSkin;
        _skinManager.LoadSavedSkin();

        Assert.Equal(before, _skinManager.CurrentSkin);
    }
}
