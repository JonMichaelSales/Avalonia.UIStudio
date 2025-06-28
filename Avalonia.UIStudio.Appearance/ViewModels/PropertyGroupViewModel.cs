using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.ViewModels
{
    public class PropertyGroupViewModel
    {
        public string GroupName { get; }
        public ObservableCollection<PropertyViewModel> Properties { get; }

        public PropertyGroupViewModel(string name, IEnumerable<PropertyViewModel> props)
        {
            GroupName = name;
            Properties = new ObservableCollection<PropertyViewModel>(props);
        }
    }

}
