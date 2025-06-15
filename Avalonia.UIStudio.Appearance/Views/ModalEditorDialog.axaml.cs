using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Avalonia.UIStudio.Appearance.Views;

public partial class ModalEditorDialog : Window
{
    public ModalEditorDialog(object data)
    {
        DataContext = data;
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        PropertyGridMain.SelectedObject = DataContext;
    }
}