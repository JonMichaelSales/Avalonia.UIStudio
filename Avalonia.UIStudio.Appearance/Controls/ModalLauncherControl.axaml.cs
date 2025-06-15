using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using Avalonia.UIStudio.Appearance.Views;

namespace Avalonia.UIStudio.Appearance.Controls;

public partial class ModalEditorLauncherControl : ValidatableEditorControlBase<ModalEditorLauncherControl>
{
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<ModalEditorLauncherControl, object?>(nameof(Value));

    public static readonly StyledProperty<string> LaunchLabelProperty =
        AvaloniaProperty.Register<ModalEditorLauncherControl, string>(nameof(LaunchLabel), defaultValue: "Edit...");

    public ModalEditorLauncherControl()
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

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void LaunchEditor_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Value == null)
            return;

        var window = new ModalEditorDialog(Value);
        await window.ShowDialog((Window)VisualRoot!);
    }
}