using System.Reflection;
using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.ViewModels;

public abstract class ReflectedPropertyViewModelBase<T> : PropertyViewModel
{
    private readonly object _target;
    private readonly PropertyInfo _property;

    protected ReflectedPropertyViewModelBase(object target, PropertyInfo property)
    {
        _target = target;
        _property = property;
        DisplayName = property.Name;
    }

    public override object? Value
    {
        get => _property.GetValue(_target);
        set
        {
            _property.SetValue(_target, value);
            this.RaisePropertyChanged();
        }
    }

    public T? TypedValue
    {
        get => (T?)_property.GetValue(_target);
        set
        {
            _property.SetValue(_target, value);
            this.RaisePropertyChanged(nameof(Value));
            this.RaisePropertyChanged(nameof(TypedValue));
        }
    }
}