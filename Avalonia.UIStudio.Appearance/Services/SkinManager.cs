using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Services;

/// <summary>
///     Manages the skins for an Avalonia application, providing functionality to register, retrieve, and apply skins.
/// </summary>
/// <remarks>
///     This class serves as a singleton instance to manage the available skins and the currently applied skin.
///     It provides methods to register new skins, retrieve existing skins by name, and apply a specific skin.
///     Additionally, it raises events when the skin is changed, allowing other components to react to skin updates.
/// </remarks>
public class SkinManager : ISkinManager
{
    private readonly ISkinRegistryService _registry;
    private readonly ISkinApplierService _applier;
    private readonly ISkinPersistenceService _persistence;

    public static SkinManager? Instance { get; set; }

    public SkinManager(ISkinRegistryService registry, ISkinApplierService applier, ISkinPersistenceService persistence)
    {
        _registry = registry;
        _applier = applier;
        _persistence = persistence;

        _applier.SkinChanged += (_, _) => SkinChanged?.Invoke(this, EventArgs.Empty);
    }

    public Skin? CurrentSkin => _applier.CurrentSkin;


    public event EventHandler? SkinChanged;

    public void RegisterSkin(string name, Skin skin) => _registry.RegisterSkin(name, skin);

    public Skin? GetSkin(string? name) => _registry.GetSkin(name);

    public List<string> GetAvailableSkinNames() => _registry.GetAvailableSkinNames();

    public void ApplySkin(string? skinName)
    {
        try
        {
            var skin = _registry.GetSkin(skinName);
            if (skin == null)
            {
                Console.WriteLine($"Skin not found: {skinName}");
                return;
            }

            _applier.ApplySkin(skin);
            _persistence.SaveSelectedSkin(skinName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying skin {skinName}: {ex.Message}");
        }
    }
    public void ApplySkin(Skin? skin)
    {
        _applier.ApplySkin(skin);
    }

    public string GetSkinFilePath(Skin skin)
    {
        return $"./Skins/{skin.Name}/skin.json";
    }



    public void ReloadSkins() => _registry.ReloadSkins();

    public void LoadSavedSkin() => _persistence.LoadSavedSkin();

    public void SaveSelectedSkin(string skinName) => _persistence.SaveSelectedSkin(skinName);
}
