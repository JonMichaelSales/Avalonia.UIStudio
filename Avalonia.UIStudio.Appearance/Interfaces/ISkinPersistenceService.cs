using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.Interfaces
{
    public interface ISkinPersistenceService
    {
        void SaveSelectedSkin(string skinName);
        void LoadSavedSkin();
    }

}
