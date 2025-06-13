using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Controls;

public class ValidationDecorator : Decorator
{
    public ValidationDecorator()
    {
        DataContextChanged += ValidationDecorator_DataContextChanged;
    }

    private void ValidationDecorator_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is Dictionary<string, ValidatedProperty> propertys && PropertyName != null && propertys.ContainsKey(PropertyName))
        {
            ValidatedProperty = propertys[PropertyName];
            this.DataContext = ValidatedProperty; // Key fix!
        }
        else
        {
            if (DataContext is ValidatedProperty validatedProperty)
            {
                ValidatedProperty = validatedProperty;
            }
        }
    }

    public static readonly StyledProperty<ValidatedProperty?> ValidatedPropertyProperty =
        AvaloniaProperty.Register<ValidationDecorator, ValidatedProperty?>(
            nameof(ValidatedProperty));

    public ValidatedProperty? ValidatedProperty
    {
        get => GetValue(ValidatedPropertyProperty);
        set => SetValue(ValidatedPropertyProperty, value);
    }

    public static readonly StyledProperty<string?> PropertyNameProperty =
        AvaloniaProperty.Register<ValidationDecorator, string?>(
            nameof(PropertyName));

    public string? PropertyName
    {
        get => GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }
}