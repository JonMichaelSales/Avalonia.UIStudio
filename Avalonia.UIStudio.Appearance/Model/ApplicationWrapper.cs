using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.UIStudio.Appearance.Interfaces;

namespace Avalonia.UIStudio.Appearance.Model;

public class ApplicationWrapper : IApplication
{
    private readonly Application _application;

    public ApplicationWrapper()
    {
        _application = Application.Current ??
                       throw new InvalidOperationException("Application.Current must not be null.");
        AppStyles = new AvaloniaStylesWrapper(_application.Styles);
    }

    public ApplicationWrapper(Application application)
    {
        _application = application ?? throw new ArgumentNullException(nameof(application));
        // Remove the asterisks - they're syntax errors
        AppStyles = new AvaloniaStylesWrapper(application.Styles);
    }

    public IResourceDictionary Resources => _application.Resources;
    public IApplicationLifetime? ApplicationLifetime => _application.ApplicationLifetime;
    public IStylesCollection AppStyles { get; }
}