using Avalonia.UIStudio.Appearance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.Interfaces
{
    public interface ISkinApplierService
    {
        void ApplySkin(Skin? skin);
        event EventHandler? SkinChanged;
        Skin? CurrentSkin { get; }

    }

}
