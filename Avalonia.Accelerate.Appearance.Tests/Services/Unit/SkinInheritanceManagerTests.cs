using System;
using System.Collections.Generic;
using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Headless.XUnit;
using Moq;
using Xunit;

namespace Avalonia.Accelerate.Appearance.Tests.Services.Unit;

public class SkinInheritanceManagerTests
{
    private readonly Mock<ISkinManager> _skinManagerMock;
    private readonly SkinInheritanceManager _manager;

    public SkinInheritanceManagerTests()
    {
        _skinManagerMock = new Mock<ISkinManager>();
        _manager = new SkinInheritanceManager(_skinManagerMock.Object);
    }

    [AvaloniaFact]
    public void RegisterInheritableSkin_Throws_OnNullName()
    {
        var skin = new InheritableSkin();
        Assert.Throws<ArgumentException>(() => _manager.RegisterInheritableSkin(null, skin));
    }

    [AvaloniaFact]
    public void RegisterInheritableSkin_Throws_OnNullSkin()
    {
        Assert.Throws<ArgumentNullException>(() => _manager.RegisterInheritableSkin("Test", null));
    }

    [AvaloniaFact]
    public void RegisterInheritableSkin_RegistersSkin()
    {
        var skin = new InheritableSkin();
        _manager.RegisterInheritableSkin("Test", skin);
        var resolved = _manager.GetResolvedSkin("Test");
        Assert.NotNull(resolved);
    }

    [AvaloniaFact]
    public void GetResolvedSkin_ReturnsNull_OnNullOrEmptyName()
    {
        Assert.Null(_manager.GetResolvedSkin(null));
        Assert.Null(_manager.GetResolvedSkin(""));
    }

    [AvaloniaFact]
    public void GetResolvedSkin_ReturnsNull_IfNotRegistered()
    {
        Assert.Null(_manager.GetResolvedSkin("Unknown"));
    }

    [AvaloniaFact]
    public void GetResolvedSkin_ReturnsCachedInstance()
    {
        var skin = new InheritableSkin();
        _manager.RegisterInheritableSkin("Test", skin);
        var first = _manager.GetResolvedSkin("Test");
        var second = _manager.GetResolvedSkin("Test");
        Assert.Same(first, second);
    }

    [AvaloniaFact]
    public void GetResolvedSkin_ResolvesBaseSkin_FromSkinManager()
    {
        var baseSkin = new Skin { Name = "Base" };
        _skinManagerMock.Setup(m => m.GetSkin("Base")).Returns(baseSkin);

        var skin = new InheritableSkin { BaseSkinName = "Base" };
        _manager.RegisterInheritableSkin("Child", skin);

        var resolved = _manager.GetResolvedSkin("Child");
        Assert.NotNull(resolved);
        Assert.Equal("Child", resolved.Name);
    }

    [AvaloniaFact]
    public void CreateVariant_Throws_OnNullOrEmptyBaseName()
    {
        Assert.Throws<ArgumentException>(() => _manager.CreateVariant(null, "Variant", new Dictionary<string, object>()));
        Assert.Throws<ArgumentException>(() => _manager.CreateVariant("", "Variant", new Dictionary<string, object>()));
    }

    [AvaloniaFact]
    public void CreateVariant_Throws_OnNullOrEmptyVariantName()
    {
        Assert.Throws<ArgumentException>(() => _manager.CreateVariant("Base", null, new Dictionary<string, object>()));
        Assert.Throws<ArgumentException>(() => _manager.CreateVariant("Base", "", new Dictionary<string, object>()));
    }

    [AvaloniaFact]
    public void CreateVariant_Throws_OnNullOverrides()
    {
        Assert.Throws<ArgumentNullException>(() => _manager.CreateVariant("Base", "Variant", null));
    }

    [AvaloniaFact]
    public void CreateVariant_RegistersAndReturnsVariant()
    {
        var overrides = new Dictionary<string, object> { { "PrimaryColor", "#FFFFFF" } };
        var variant = _manager.CreateVariant("Base", "Variant", overrides);
        Assert.NotNull(variant);
        Assert.Equal("Variant", variant.Name);
        Assert.Equal("Base", variant.BaseSkinName);
        Assert.Equal(overrides, variant.PropertyOverrides);
    }

    [AvaloniaFact]
    public void ClearCache_RemovesCachedThemes()
    {
        var skin = new InheritableSkin();
        _manager.RegisterInheritableSkin("Test", skin);
        var resolved1 = _manager.GetResolvedSkin("Test");
        _manager.ClearCache();
        var resolved2 = _manager.GetResolvedSkin("Test");
        Assert.NotSame(resolved1, resolved2);
    }
}
