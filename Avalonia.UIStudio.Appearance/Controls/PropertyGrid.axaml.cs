using System.Collections.ObjectModel;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;
using Avalonia.UIStudio.Appearance.ViewModels;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class PropertyGrid : UserControl
{
    public static readonly StyledProperty<object?> SelectedObjectProperty =
        AvaloniaProperty.Register<PropertyGrid, object?>(nameof(SelectedObject));

    public static readonly StyledProperty<bool> IsEditModeProperty =
        AvaloniaProperty.Register<PropertyGrid, bool>(nameof(IsEditMode));


    public PropertyGrid()
    {
        InitializeComponent();
        DataContext = this;

        this.GetObservable(SelectedObjectProperty).Subscribe(OnSelectedObjectChanged);
    }

    public object? SelectedObject
    {
        get => GetValue(SelectedObjectProperty);
        set => SetValue(SelectedObjectProperty, value);
    }

    public bool IsEditMode
    {
        get => GetValue(IsEditModeProperty);
        set => SetValue(IsEditModeProperty, value);
    }


    public ObservableCollection<PropertyViewModel> Properties { get; } = new();

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnSelectedObjectChanged(object? obj)
    {
        Properties.Clear();
        if (obj == null) return;

        var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite);

        foreach (var prop in props)
        {
            PropertyViewModel vm = CreateViewModelForProperty(obj, prop);
            Properties.Add(vm);
        }
    }

    private static PropertyViewModel CreateViewModelForProperty(object obj, PropertyInfo prop)
    {
        var type = prop.PropertyType;

        if (type == typeof(Color))
            return new ColorPropertyViewModel(obj, prop);
        if (type == typeof(double) || type == typeof(int))
            return new NumericPropertyViewModel(obj, prop);
        if (type == typeof(bool))
            return new BoolPropertyViewModel(obj, prop);
        if (type.IsEnum)
            return new EnumPropertyViewModel(obj, prop);
        if (type == typeof(string))
            return new StringPropertyViewModel(obj, prop);
        if (type == typeof(DateTime))
            return new DateTimePropertyViewModel(obj, prop);
        if (type == typeof(TimeSpan))
            return new TimeSpanPropertyViewModel(obj, prop);
        if (type == typeof(Uri))
            return new UriPropertyViewModel(obj, prop);
        if (type == typeof(FontFamily))
            return new FontFamilyPropertyViewModel(obj, prop);
        if (type == typeof(Thickness))
            return new ThicknessPropertyViewModel(obj, prop);
        if (type == typeof(SerializableTypography))
            return new TypographyPropertyViewModel(obj, prop);
        if (type == typeof(SerializableSkin))
            return new SkinPropertyViewModel(obj, prop);
        if (TypeHelpers.IsListOfStrings(type))
            return new StringListPropertyViewModel(obj, prop);
        if (TypeHelpers.IsListOfObjects(type))
            return new ObjectListPropertyViewModel(obj, prop);
        else
            return new ObjectPropertyViewModel(obj, prop);
    }

}