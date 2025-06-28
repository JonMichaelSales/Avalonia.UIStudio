using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.UIStudio.Appearance.ViewModels;
using System.Threading.Tasks;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Views;

public partial class SkinEditorDialog : Window
{
    public SkinEditorDialog()
    {
        InitializeComponent();
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

}