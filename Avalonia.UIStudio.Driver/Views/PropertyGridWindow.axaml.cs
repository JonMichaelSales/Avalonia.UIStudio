using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.UIStudio.Appearance.Controls;
using Avalonia.UIStudio.Appearance.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace Avalonia.UIStudio.Driver.Views;

public partial class PropertyGridWindow : Window, INotifyPropertyChanged
{
    private readonly Skin _skin;
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public PropertyGridWindow(Skin skin)
    {
        InitializeComponent();
        _skin = skin;
    }

    private bool _isEdit;
    public bool IsEdit
    {
        get => _isEdit;
        set
        {
            if (_isEdit != value)
            {
                _isEdit = value;
                OnPropertyChanged(nameof(IsEdit));
            }
        }
    }



    
    public void SetSelectedObject(object? obj)
    {
        StackPanel stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        PropertyGrid propertyGrid = new PropertyGrid()
        {
            SelectedObject = obj,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        propertyGrid.Bind(PropertyGrid.IsEditModeProperty, new Binding
        {
            Source = new RelativeSource(RelativeSourceMode.FindAncestor)
            {
                AncestorType = typeof(PropertyGridWindow)
            },
            Path = nameof(IsEdit),
            Mode = BindingMode.TwoWay
        });

        stackPanel.Children.Add(propertyGrid);
        MainPanel.Children.Add(stackPanel);
    }


    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        SetSelectedObject(_skin);
    }
}

public class PersonObj
{
    public string Name { get; set; } = "John Doe";
    public int Age { get; set; } = 30;
    public string Address { get; set; } = "123 Main St";
    public bool IsEmployed { get; set; } = true;
    public double Salary { get; set; } = 50000.0;
}