using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.ViewModels;

/// <summary>
///     Serves as the base class for all ViewModel classes in the AvaloniaSkinManager application.
/// </summary>
/// <remarks>
///     This class provides common functionality for ViewModel classes, including support for
///     reactive property changes and resource management through the <see cref="IDisposable" /> interface.
/// </remarks>
public class ViewModelBase : ReactiveObject, IDisposable
{
    /// <summary>
    ///     Releases all resources used by the <see cref="ViewModelBase" /> instance.
    /// </summary>
    /// <remarks>
    ///     This method calls the <see cref="Dispose(bool)" /> method with a value of <c>true</c>
    ///     to release managed resources and suppresses finalization of the object.
    /// </remarks>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases all resources used by the <see cref="ViewModelBase" /> instance.
    /// </summary>
    /// <remarks>
    ///     This method is part of the <see cref="IDisposable" /> implementation and ensures that
    ///     both managed and unmanaged resources are properly released. It calls the
    ///     <see cref="Dispose(bool)" /> method with a value of <c>true</c>, and suppresses
    ///     finalization of the object to prevent redundant resource cleanup.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            
        }
    }
}