using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.Services
{
    public class SkinPersistenceService : ISkinPersistenceService
    {
        private readonly ISkinRegistryService _registry;
        private readonly ISkinApplierService _applier;

        public SkinPersistenceService(ISkinRegistryService registry, ISkinApplierService applier)
        {
            _registry = registry;
            _applier = applier;
        }

        public void SaveSelectedSkin(string skinName)
        {
            AppSettings.Instance.Skin = skinName;
            AppSettings.Instance.Save();
        }

        public void LoadSavedSkin()
        {
            var skinName = AppSettings.Instance.Skin;
            var skin = _registry.GetSkin(skinName);
            if (skin != null)
                _applier.ApplySkin(skin);
        }
    }

}
