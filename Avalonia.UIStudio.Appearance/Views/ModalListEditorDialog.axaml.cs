using Avalonia.Controls;

namespace Avalonia.UIStudio.Appearance.Views;

public partial class ModalListEditorDialog : Window
{
    public ModalListEditorDialog(IEnumerable<object> list)
    {
        DataContext = list;
        InitializeComponent();
        ItemsControlMain.ItemsSource = list;
    }

}