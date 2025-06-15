using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.ViewModels;

public class EditableSetter : ReactiveObject
{
    private object? _value;
    public string PropertyName { get; set; } = string.Empty;

    public object? Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }
}