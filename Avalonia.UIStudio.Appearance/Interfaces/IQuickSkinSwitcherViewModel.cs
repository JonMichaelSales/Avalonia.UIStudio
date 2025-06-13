using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;
using Avalonia.UIStudio.Appearance.ViewModels;
using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.Interfaces;

public interface IQuickSkinSwitcherViewModel
{
    /// <summary>
    /// Gets the collection of available skins that can be selected and applied
    /// within the application.
    /// </summary>
    /// <remarks>
    /// This property is populated by the <see cref="LoadAvailableskins"/> method,
    /// which retrieves the skins from the <see cref="SkinManager"/>. The collection
    /// is updated dynamically to reflect the available skins.
    /// </remarks>
    ObservableCollection<SkinSummaryInfo> AvailableSkins { get; }

    /// <summary>
    /// Gets or sets the currently selected skin.
    /// </summary>
    /// <remarks>
    /// When a new skin is selected, the corresponding skin is applied automatically.
    /// The selected skin is synchronized with the <see cref="AvailableSkins"/> collection.
    /// </remarks>
    SkinSummaryInfo? SelectedSkin { get; set; }

    IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing { get; }
    IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed { get; }
    IObservable<Exception> ThrownExceptions { get; }

    /// <summary>
    /// Releases all resources used by the <see cref="ViewModelBase"/> instance.
    /// </summary>
    /// <remarks>
    /// This method calls the <see cref="Dispose(bool)"/> method with a value of <c>true</c> 
    /// to release managed resources and suppresses finalization of the object.
    /// </remarks>
    void Dispose();

    IDisposable SuppressChangeNotifications();
    bool AreChangeNotificationsEnabled();
    IDisposable DelayChangeNotifications();
    event PropertyChangingEventHandler? PropertyChanging;
    event PropertyChangedEventHandler? PropertyChanged;
}