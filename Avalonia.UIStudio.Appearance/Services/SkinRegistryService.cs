using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.Services
{
    public class SkinRegistryService : ISkinRegistryService
    {
        private readonly ISkinLoaderService _skinLoader;
        private readonly Dictionary<string, Skin?> _availableSkins = new();

        public SkinRegistryService(ISkinLoaderService loader)
        {
            _skinLoader = loader;
            ReloadSkins();
        }

        public void RegisterSkin(string name, Skin skin)
        {
            if (!string.IsNullOrWhiteSpace(name) && skin != null)
            {
                skin.Name = name;
                _availableSkins[name] = skin;
            }
        }

        public Skin? GetSkin(string? name)
        {
            if (name != null && _availableSkins.TryGetValue(name, out var skin))
                return skin;
            return null;
        }

        public List<string> GetAvailableSkinNames() => _availableSkins.Keys.ToList();

        public void ReloadSkins()
        {
            _availableSkins.Clear();
            var skins = _skinLoader.LoadSkins("avares://Avalonia.UIStudio.Appearance/Skins/");
            foreach (var skin in skins)
                RegisterSkin(skin.Name, skin);
        }
    }

}
