using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;
using Avalonia.UIStudio.Appearance.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Avalonia.UIStudio.Appearance.Controls;

public enum PropertyGridSortMode
{
    Alphabetical,
    ByType
}

public partial class PropertyGrid : ReactiveUserControl<PropertyGrid>, IReactiveObject, INotifyPropertyChanged
{
    public static readonly StyledProperty<object?> SelectedObjectProperty =
        AvaloniaProperty.Register<PropertyGrid, object?>(nameof(SelectedObject));

    public static readonly StyledProperty<bool> IsEditModeProperty =
        AvaloniaProperty.Register<PropertyGrid, bool>(nameof(IsEditMode));

    public static readonly StyledProperty<PropertyGridSortMode> SortModeProperty =
        AvaloniaProperty.Register<PropertyGrid, PropertyGridSortMode>(
            nameof(SortMode), defaultValue: PropertyGridSortMode.ByType);

    public PropertyGrid()
    {
        InitializeComponent();
        DataContext = this;

        this.GetObservable(SelectedObjectProperty).Subscribe(OnSelectedObjectChanged);
        this.GetObservable(SortModeProperty).Subscribe(_ => OnSelectedObjectChanged(SelectedObject));
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

    public PropertyGridSortMode SortMode
    {
        get => GetValue(SortModeProperty);
        set => SetValue(SortModeProperty, value);
    }

    public List<PropertyGridSortMode> SortModes { get; } = new()
    {
        PropertyGridSortMode.Alphabetical,
        PropertyGridSortMode.ByType
    };

    private ObservableCollection<PropertyViewModel> _properties = new();
    public ObservableCollection<PropertyViewModel> Properties
    {
        get => _properties;
        set => RaiseAndSetIfChanged(ref _properties, value);
    }

    private ObservableCollection<PropertyGroupViewModel> _groupedProperties = new();
    public ObservableCollection<PropertyGroupViewModel> GroupedProperties
    {
        get => _groupedProperties;
        set => RaiseAndSetIfChanged(ref _groupedProperties, value);
    }

    private void OnSelectedObjectChanged(object? obj)
    {
        Properties = new ObservableCollection<PropertyViewModel>();
        GroupedProperties = new ObservableCollection<PropertyGroupViewModel>();

        if (obj == null) return;

        var props = obj.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .Select(p => CreateViewModelForProperty(obj, p));

        if (SortMode == PropertyGridSortMode.Alphabetical)
        {
            Properties = new ObservableCollection<PropertyViewModel>(
                props.OrderBy(p => p.DisplayName)
            );
        }
        else
        {
            GroupedProperties = new ObservableCollection<PropertyGroupViewModel>(
                props.GroupBy(p => p.GroupName)
                     .OrderBy(g => g.Key)
                     .Select(g => new PropertyGroupViewModel(g.Key, g.OrderBy(p => p.DisplayName)))
            );
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

        return new ObjectPropertyViewModel(obj, prop);
    }

    // ----- ReactiveUI glue -----

    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;

    void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args) =>
        PropertyChanged?.Invoke(this, args);

    void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args) =>
        PropertyChanging?.Invoke(this, args);

    

    protected bool RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
