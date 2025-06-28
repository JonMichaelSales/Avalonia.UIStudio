using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.ViewModels;

public abstract class PropertyViewModel : ReactiveObject
{
    public string DisplayName { get; set; } = string.Empty;
    public abstract object? Value { get; set; }
    public virtual string GroupName => GetType().Name.Replace("PropertyViewModel", string.Empty);
}