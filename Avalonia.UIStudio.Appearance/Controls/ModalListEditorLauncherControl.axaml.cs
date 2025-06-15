using Avalonia.Controls;
using Avalonia.UIStudio.Appearance.Views;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class ModalListEditorLauncherControl : ValidatableEditorControlBase<ModalListEditorLauncherControl>
{
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<ModalListEditorLauncherControl, object?>(nameof(Value));

    public static readonly StyledProperty<string> LaunchLabelProperty =
        AvaloniaProperty.Register<ModalListEditorLauncherControl, string>(nameof(LaunchLabel), defaultValue: "Edit...");

    public ModalListEditorLauncherControl()
    {
        InitializeComponent();
    }

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string LaunchLabel
    {
        get => GetValue(LaunchLabelProperty);
        set => SetValue(LaunchLabelProperty, value);
    }

    private async void LaunchEditor_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Value is not IEnumerable<object> list)
            return;

        var window = new ModalListEditorDialog(list);
        await window.ShowDialog((Window)VisualRoot!);
    }
}