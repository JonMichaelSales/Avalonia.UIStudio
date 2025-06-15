// Controls/ValidatableEditorControlBase.cs

using Avalonia.Controls;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Controls;

public abstract class ValidatableEditorControlBase<TControl> : UserControl where TControl : UserControl
{
    public static readonly StyledProperty<ValidatedProperty?> ValidatedPropertyProperty =
        AvaloniaProperty.Register<TControl, ValidatedProperty?>(nameof(ValidatedProperty));

    public static readonly StyledProperty<string?> PropertyNameProperty =
        AvaloniaProperty.Register<TControl, string?>(nameof(PropertyName));

    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<TControl, string>(nameof(Label));

    public static readonly StyledProperty<bool> IsEditModeProperty =
        AvaloniaProperty.Register<TControl, bool>(nameof(IsEditMode));

    public ValidatedProperty? ValidatedProperty
    {
        get => GetValue(ValidatedPropertyProperty);
        set => SetValue(ValidatedPropertyProperty, value);
    }

    public string? PropertyName
    {
        get => GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public bool IsEditMode
    {
        get => GetValue(IsEditModeProperty);
        set => SetValue(IsEditModeProperty, value);
    }
}