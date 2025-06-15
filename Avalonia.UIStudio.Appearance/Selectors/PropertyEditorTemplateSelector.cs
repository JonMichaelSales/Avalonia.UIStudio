using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Avalonia.UIStudio.Appearance.ViewModels;

namespace Avalonia.UIStudio.Appearance.Selectors;

public class PropertyEditorTemplateSelector : IDataTemplate
{
    [Content] public Dictionary<Type, IDataTemplate> Templates { get; } = new();

    public bool Match(object? data)
    {
        return data is PropertyViewModel;
    }

    public Control Build(object? param)
    {
        if (param is PropertyViewModel propertyVm)
        {
            var vmType = propertyVm.GetType();
            if (Templates.TryGetValue(vmType, out var template)) return template.Build(param);
        }

        // fallback
        return new TextBlock { Text = "Unknown Property Type" };
    }
}