using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Avalonia.UIStudio.Appearance.Interfaces;

public interface IApplication
{
    IResourceDictionary Resources { get; }
    IApplicationLifetime? ApplicationLifetime { get; }
    IStylesCollection AppStyles { get; }
}