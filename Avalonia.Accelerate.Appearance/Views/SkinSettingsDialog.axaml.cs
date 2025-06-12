using Avalonia.Accelerate.Appearance.Extensions;
using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.Accelerate.Appearance.Views;

/// <summary>
/// Represents a dialog window for managing skin settings in the application.
/// </summary>
/// <remarks>
/// This class provides a user interface for selecting, applying, and resetting skins.
/// It is backed by the <see cref="SkinSettingsViewModel"/> 
/// to handle the logic and data binding for skin management.
/// </remarks>
public partial class SkinSettingsDialog : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkinSettingsDialog"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor sets up the dialog by initializing its components and 
    /// assigning a new instance of <see cref="SkinSettingsViewModel"/> as its data context.
    /// </remarks>
    ///
    public SkinSettingsDialog()
    {
        InitializeComponent();
        DataContext = Application.Current.GetRequiredService<SkinSettingsViewModel>();
    }
    public SkinSettingsDialog(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        var viewModel = serviceProvider.GetRequiredService<SkinSettingsViewModel>();
        DataContext = viewModel;
    }

    private void ResetButton_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SkinSettingsViewModel viewModel)
        {
            viewModel.ResetToDefault();
        }
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();

    }
}