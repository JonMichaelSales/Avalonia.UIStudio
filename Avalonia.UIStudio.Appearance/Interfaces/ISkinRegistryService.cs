using Avalonia.UIStudio.Appearance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.Interfaces
{
    public interface ISkinRegistryService
    {
        void RegisterSkin(string name, Skin skin);
        Skin? GetSkin(string? name);
        List<string> GetAvailableSkinNames();
        void ReloadSkins();
    }

}
