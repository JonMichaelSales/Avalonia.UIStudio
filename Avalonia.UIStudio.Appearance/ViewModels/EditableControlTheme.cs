using System.Collections.ObjectModel;
using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.ViewModels;

public class EditableControlTheme : ReactiveObject
{
    // The type of control this theme is for (Button, TextBox, etc.)
    public string ControlType { get; set; } = string.Empty;

    // The base template this control theme is based on (Template1, Template2, etc.)
    public string? BaseTemplateName { get; set; }

    // Editable Setters - Property name → Value (object supports Color, Thickness, etc.)
    public ObservableCollection<EditableSetter> Setters { get; } = new();

    // Convenience: get or set a value by property name
    public void SetOrUpdateSetter(string propertyName, object value)
    {
        var existing = Setters.FirstOrDefault(s => s.PropertyName == propertyName);
        if (existing != null)
            existing.Value = value;
        else
            Setters.Add(new EditableSetter
            {
                PropertyName = propertyName,
                Value = value
            });
    }

    public object? GetSetterValue(string propertyName)
    {
        return Setters.FirstOrDefault(s => s.PropertyName == propertyName)?.Value;
    }
}