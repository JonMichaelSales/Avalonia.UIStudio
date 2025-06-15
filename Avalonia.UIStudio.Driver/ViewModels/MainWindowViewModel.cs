using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.UIStudio.Appearance.Extensions;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.ViewModels;
using Avalonia.UIStudio.Appearance.Views;
using Avalonia.UIStudio.Dialogs.Interfaces;
using Avalonia.UIStudio.Dialogs.Services;
using Avalonia.UIStudio.Driver.Views;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Avalonia.UIStudio.Driver.ViewModels
{
    /// <summary>
    /// Main window view model demonstrating integration of all Avalonia.Accelerate libraries:
    /// Icons, Appearance (Skins), and Dialogs
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ISkinManager _skinManager;
        private readonly IDialogService _dialogService;
        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly ISkinImportExportService _importExportService;
        private readonly SkinSettingsDialog _skinSettingsDialog;
        private string _currentSkinName = "Unknown";
        private int _availableSkinCount = 0;
        public QuickSkinSwitcherViewModel? QuickSkinSwitcherViewModel { get; }

        /// <summary>
        /// Initializes a new instance of MainWindowViewModel with dependency injection
        /// </summary>
        public MainWindowViewModel(
            IDialogService dialogService,
            ISkinManager skinManager,
            ILogger<MainWindowViewModel> logger,
            ISkinImportExportService _importExportService,SkinSettingsDialog skinSettingsDialog)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _skinManager = skinManager ?? throw new ArgumentNullException(nameof(skinManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._importExportService = _importExportService;
            _skinSettingsDialog = skinSettingsDialog;

            InitializeCommands();
            InitializeSkinData();
            SubscribeToSkinChanges();
            CurrentSkinName = _skinManager.CurrentSkin?.Name ?? "No Active Skin";
            _logger.LogInformation("MainWindowViewModel initialized successfully");
        }

        

        #region Properties

        /// <summary>
        /// Gets or sets the name of the currently active skin
        /// </summary>
        public string CurrentSkinName
        {
            get => _currentSkinName;
            set => this.RaiseAndSetIfChanged(ref _currentSkinName, value);
        }

        /// <summary>
        /// Gets or sets the count of available skins
        /// </summary>
        public int AvailableSkinCount
        {
            get => _availableSkinCount;
            set => this.RaiseAndSetIfChanged(ref _availableSkinCount, value);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command to open the skin settings dialog
        /// </summary>
        public ReactiveCommand<Unit, Unit> OpenSkinSettingsCommand { get; private set; } = null!;
        public ReactiveCommand<Unit, Unit> OpenPropertyGridWindowCommand { get; private set; } = null!;

        /// <summary>
        /// Command to show an information dialog
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowInfoDialogCommand { get; private set; } = null!;

        /// <summary>
        /// Command to show a warning dialog
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowWarningDialogCommand { get; private set; } = null!;

        /// <summary>
        /// Command to show an error dialog
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowErrorDialogCommand { get; private set; } = null!;

        /// <summary>
        /// Command to show a confirmation dialog
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowConfirmationDialogCommand { get; private set; } = null!;

        /// <summary>
        /// Command to show a validation dialog with sample errors and warnings
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowValidationDialogCommand { get; private set; } = null!;

        /// <summary>
        /// Command to export the current skin to a file
        /// </summary>
        public ReactiveCommand<Unit, Unit> ExportSkinCommand { get; private set; } = null!;

        /// <summary>
        /// Command to import a skin from a file
        /// </summary>
        public ReactiveCommand<Unit, Unit> ImportSkinCommand { get; private set; } = null!;

        /// <summary>
        /// Command to run an integration demo showing all libraries working together
        /// </summary>
        public ReactiveCommand<Unit, Unit> RunIntegrationDemoCommand { get; private set; } = null!;
        public ReactiveCommand<Unit, Unit> ShowOpenFileDialogCommand { get; private set; } = null!;
        public ReactiveCommand<Unit, Unit> ShowSaveFileDialogCommand { get; private set; } = null!;
        public ReactiveCommand<Unit, Unit> ShowOpenFolderDialogCommand { get; private set; } = null!;

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            OpenSkinSettingsCommand = ReactiveCommand.CreateFromTask(OpenSkinSettingsAsync);
            OpenPropertyGridWindowCommand =ReactiveCommand.CreateFromTask(OpenPropertyGridWindowAsync);
            ShowInfoDialogCommand = ReactiveCommand.CreateFromTask(ShowInfoDialogAsync);
            ShowWarningDialogCommand = ReactiveCommand.CreateFromTask(ShowWarningDialogAsync);
            ShowErrorDialogCommand = ReactiveCommand.CreateFromTask(ShowErrorDialogAsync);
            ShowConfirmationDialogCommand = ReactiveCommand.CreateFromTask(ShowConfirmationDialogAsync);
            ShowValidationDialogCommand = ReactiveCommand.CreateFromTask(ShowValidationDialogAsync);
            ExportSkinCommand = ReactiveCommand.CreateFromTask(ExportSkinAsync);
            ImportSkinCommand = ReactiveCommand.CreateFromTask(ImportSkinAsync);
            RunIntegrationDemoCommand = ReactiveCommand.CreateFromTask(RunIntegrationDemoAsync);
            ShowOpenFileDialogCommand = ReactiveCommand.CreateFromTask(ShowOpenFileDialogAsync);
            ShowSaveFileDialogCommand = ReactiveCommand.CreateFromTask(ShowSaveFileDialogAsync);
            ShowOpenFolderDialogCommand = ReactiveCommand.CreateFromTask(ShowOpenFolderDialogAsync);
        }

        private void InitializeSkinData()
        {
            try
            {
                if (_skinManager.CurrentSkin == null)
                {
                    _skinManager.ApplySkin("Dark");
                }
                CurrentSkinName = _skinManager.CurrentSkin?.Name ?? "No Active Skin";
                AvailableSkinCount = _skinManager.GetAvailableSkinNames().Count;

                _logger.LogDebug("Initialized skin data - Current: {CurrentSkin}, Available: {Count}",
                    CurrentSkinName, AvailableSkinCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize skin data");
                CurrentSkinName = "Error Loading";
                AvailableSkinCount = 0;
            }
        }

        private void SubscribeToSkinChanges()
        {
            _skinManager.SkinChanged += OnSkinChanged;
        }

        private void OnSkinChanged(object? sender, EventArgs e)
        {
            try
            {
                CurrentSkinName = _skinManager?.CurrentSkin?.Name ?? "Unknown";
                _logger.LogInformation("Skin changed to: {SkinName}", CurrentSkinName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling skin change event");
            }
        }

        private async Task OpenSkinSettingsAsync()
        {
            try
            {
                _logger.LogDebug("Opening skin settings dialog");

                var skinSettingsDialog = Application.Current.GetRequiredService<SkinSettingsDialog>();
                if (GetMainWindow() is { } mainWindow)
                {
                    await skinSettingsDialog.ShowDialog(mainWindow);
                }
                else
                {
                    skinSettingsDialog.Show((Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime).MainWindow);
                }

                _logger.LogInformation("Skin settings dialog closed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open skin settings dialog");
                await ShowErrorAsync("Settings Error", "Failed to open skin settings dialog", ex);
            }
        }
        
        private async Task OpenPropertyGridWindowAsync()
        {
            try
            {
                
                _logger.LogDebug("Opening property grid window");

                PropertyGridWindow propertyGridWindow = new PropertyGridWindow(_skinManager.CurrentSkin);
                

                if (GetMainWindow() is { } mainWindow)
                {
                    await propertyGridWindow.ShowDialog(mainWindow);
                }
                else
                {
                    propertyGridWindow.Show((Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime).MainWindow);
                }

                _logger.LogInformation("Property grid window closed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open property grid window");
                await ShowErrorAsync("Property Grid Error", "Failed to open property grid window", ex);
            }
        }

        private async Task ShowInfoDialogAsync()
        {
            try
            {
                _logger.LogDebug("Showing information dialog");

                await _dialogService.ShowInfoAsync(
                    "Information Demo",
                    "This is an information dialog from the Avalonia.UIStudio.Dialogs library. " +
                    "It demonstrates how to show informational messages to users with consistent styling " +
                    "that adapts to the current skin.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show information dialog");
            }
        }

        private async Task ShowWarningDialogAsync()
        {
            try
            {
                _logger.LogDebug("Showing warning dialog");

                await _dialogService.ShowWarningAsync(
                    "Warning Demo",
                    "This is a warning dialog that alerts users to potential issues. " +
                    "Notice how the styling automatically adapts to the current skin's warning color scheme.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show warning dialog");
            }
        }

        private async Task ShowErrorDialogAsync()
        {
            try
            {
                _logger.LogDebug("Showing error dialog");

                var sampleException = new InvalidOperationException(
                    "This is a sample exception to demonstrate error dialog functionality. " +
                    "In a real application, this would be an actual exception with stack trace details.");

                await _dialogService.ShowErrorAsync(
                    "Error Demo",
                    "This demonstrates how errors are presented to users with technical details " +
                    "that can be expanded for troubleshooting. The dialog automatically uses the " +
                    "current skin's error color scheme.",
                    sampleException);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show error dialog");
            }
        }

        private async Task ShowConfirmationDialogAsync()
        {
            try
            {
                _logger.LogDebug("Showing confirmation dialog");

                var result = await _dialogService.ShowConfirmationAsync(
                    "Confirmation Demo",
                    "This is a confirmation dialog that asks users to make a decision. " +
                    "Would you like to proceed with this demo action?",
                    "Yes, Proceed",
                    "Cancel");

                _logger.LogInformation("Confirmation dialog result: {Result}", result);

                if (result)
                {
                    await _dialogService.ShowInfoAsync(
                        "Demo Result",
                        "You confirmed the action! This demonstrates the flow of user interaction " +
                        "through multiple dialogs.");
                }
                else
                {
                    await _dialogService.ShowInfoAsync(
                        "Demo Result",
                        "You cancelled the action. This shows how to handle user cancellation gracefully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show confirmation dialog");
            }
        }

        private async Task ShowValidationDialogAsync()
        {
            try
            {
                _logger.LogDebug("Showing validation dialog");

                var sampleErrors = new List<string>
                {
                    "Primary text contrast ratio (3.2:1) is below WCAG AA standard (4.5:1)",
                    "Border color has insufficient contrast against secondary background",
                    "Skin name contains invalid characters that may cause file system issues"
                };

                var sampleWarnings = new List<string>
                {
                    "Primary text contrast ratio (5.1:1) is below WCAG AAA standard (7.0:1)",
                    "Medium and large font sizes are very similar, reducing visual hierarchy",
                    "Skin name is quite long (45 characters) and may be truncated in UI displays",
                    "Accent color is similar to primary color, which may reduce emphasis effectiveness"
                };

                await _dialogService.ShowValidationErrorsAsync(
                    "Skin Validation Results",
                    sampleErrors,
                    sampleWarnings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show validation dialog");
            }
        }

        private async Task ShowOpenFileDialogAsync()
        {
            try
            {
                _logger.LogDebug("Showing open file dialog");

                // Using IDialogService
                var serviceResults = await _dialogService.ShowOpenFileDialogAsync("Open File (Service)", allowMultiple: true);

                if (serviceResults.Length > 0)
                {
                    await _dialogService.ShowInfoAsync("Files Selected (Service)", string.Join("\n", serviceResults));
                }
                else
                {
                    await _dialogService.ShowInfoAsync("No Files Selected (Service)", "Operation was cancelled.");
                }

                // Using MessageBox (same result)
                var messageBoxResults = await MessageBox.ShowOpenFileDialogAsync("Open File (MessageBox)", allowMultiple: true);

                if (messageBoxResults.Length > 0)
                {
                    await _dialogService.ShowInfoAsync("Files Selected (MessageBox)", string.Join("\n", messageBoxResults));
                }
                else
                {
                    await _dialogService.ShowInfoAsync("No Files Selected (MessageBox)", "Operation was cancelled.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show open file dialog");
            }
        }

        private async Task ShowSaveFileDialogAsync()
        {
            try
            {
                _logger.LogDebug("Showing save file dialog");

                // Using IDialogService
                var serviceResult = await _dialogService.ShowSaveFileDialogAsync("Save File (Service)", "MyDocument.txt");

                if (!string.IsNullOrEmpty(serviceResult))
                {
                    await _dialogService.ShowInfoAsync("File Selected (Service)", serviceResult);
                }
                else
                {
                    await _dialogService.ShowInfoAsync("No File Selected (Service)", "Operation was cancelled.");
                }

                // Using MessageBox (same result)
                var messageBoxResult = await MessageBox.ShowSaveFileDialogAsync("Save File (MessageBox)", "MyDocument.txt");

                if (!string.IsNullOrEmpty(messageBoxResult))
                {
                    await _dialogService.ShowInfoAsync("File Selected (MessageBox)", messageBoxResult);
                }
                else
                {
                    await _dialogService.ShowInfoAsync("No File Selected (MessageBox)", "Operation was cancelled.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show save file dialog");
            }
        }

        private async Task ShowOpenFolderDialogAsync()
        {
            try
            {
                _logger.LogDebug("Showing open folder dialog");

                // Using IDialogService
                var serviceResult = await _dialogService.ShowOpenFolderDialogAsync("Open Folder (Service)");

                if (!string.IsNullOrEmpty(serviceResult))
                {
                    await _dialogService.ShowInfoAsync("Folder Selected (Service)", serviceResult);
                }
                else
                {
                    await _dialogService.ShowInfoAsync("No Folder Selected (Service)", "Operation was cancelled.");
                }

                // Using MessageBox (same result)
                var messageBoxResult = await MessageBox.ShowOpenFolderDialogAsync("Open Folder (MessageBox)");

                if (!string.IsNullOrEmpty(messageBoxResult))
                {
                    await _dialogService.ShowInfoAsync("Folder Selected (MessageBox)", messageBoxResult);
                }
                else
                {
                    await _dialogService.ShowInfoAsync("No Folder Selected (MessageBox)", "Operation was cancelled.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show open folder dialog");
            }
        }


        private async Task ExportSkinAsync()
        {
            try
            {
                _logger.LogDebug("Starting skin export");

                var currentSkin = _skinManager?.CurrentSkin;
                if (currentSkin == null)
                {
                    await _dialogService.ShowWarningAsync(
                        "Export Warning",
                        "No skin is currently active to export.");
                    return;
                }

                var mainWindow = GetMainWindow();
                if (mainWindow?.StorageProvider == null)
                {
                    await ShowErrorAsync("Storage Error", "Storage provider is not available for file operations.");
                    return;
                }

                var fileTypeChoices = new FilePickerFileType[]
                {
                    new("Skin Files") { Patterns = new[] { "*.json" } },
                    FilePickerFileTypes.All
                };

                var saveOptions = new FilePickerSaveOptions
                {
                    Title = "Export Current Skin",
                    FileTypeChoices = fileTypeChoices,
                    SuggestedFileName = $"{currentSkin.Name}_Skin.json",
                    DefaultExtension = "json"
                };

                var result = await mainWindow.StorageProvider.SaveFilePickerAsync(saveOptions);
                if (result != null)
                {
                    var filePath = result.Path.LocalPath;
                    var success = await _importExportService.ExportSkinAsync(
                        currentSkin,
                        filePath,
                        $"Exported from Avalonia.UIStudio Demo on {DateTime.Now:yyyy-MM-dd}",
                        "Demo User"
                    );

                    if (success)
                    {
                        _logger.LogInformation("Skin exported successfully to: {FilePath}", filePath);
                        await _dialogService.ShowInfoAsync(
                            "Export Successful",
                            $"Skin '{currentSkin.Name}' has been exported successfully to:\n{filePath}");
                    }
                    else
                    {
                        _logger.LogError("Skin export failed to: {FilePath}", filePath);
                        await ShowErrorAsync("Export Failed", "Failed to export the current skin. Please check the file path and permissions.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during skin export");
                await ShowErrorAsync("Export Error", "An error occurred while exporting the skin", ex);
            }
        }

        private async Task ImportSkinAsync()
        {
            try
            {
                _logger.LogDebug("Starting skin import");

                var mainWindow = GetMainWindow();
                if (mainWindow?.StorageProvider == null)
                {
                    await ShowErrorAsync("Storage Error", "Storage provider is not available for file operations.");
                    return;
                }

                var fileTypeChoices = new FilePickerFileType[]
                {
                    new("Skin Files") { Patterns = new[] { "*.json" } },
                    FilePickerFileTypes.All
                };

                var openOptions = new FilePickerOpenOptions
                {
                    Title = "Import Skin",
                    FileTypeFilter = fileTypeChoices,
                    AllowMultiple = false
                };

                var result = await mainWindow.StorageProvider.OpenFilePickerAsync(openOptions);
                if (result.Count > 0)
                {
                    var filePath = result[0].Path.LocalPath;
                    var importResult = await _importExportService.ImportSkinAsync(filePath);

                    if (importResult.Success && importResult.Skin != null)
                    {
                        _skinManager?.RegisterSkin(importResult.Skin.Name, importResult.Skin);

                        _logger.LogInformation("Skin imported successfully from: {FilePath}", filePath);

                        var shouldApply = await _dialogService.ShowConfirmationAsync(
                            "Import Successful",
                            $"Skin '{importResult.Skin.Name}' has been imported successfully. " +
                            "Would you like to apply it now?",
                            "Apply Now",
                            "Keep Current");

                        if (shouldApply)
                        {
                            _skinManager?.ApplySkin(importResult.Skin.Name);
                        }
                    }
                    else
                    {
                        _logger.LogError("Skin import failed from: {FilePath}. Error: {Error}", filePath, importResult.ErrorMessage);

                        if (importResult.Warnings.Any())
                        {
                            await _dialogService.ShowValidationErrorsAsync(
                                "Import Issues",
                                new List<string> { importResult.ErrorMessage ?? "Unknown error" },
                                importResult.Warnings);
                        }
                        else
                        {
                            await ShowErrorAsync("Import Failed", importResult.ErrorMessage ?? "Failed to import the skin file.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during skin import");
                await ShowErrorAsync("Import Error", "An error occurred while importing the skin", ex);
            }
        }

        private async Task RunIntegrationDemoAsync()
        {
            try
            {
                _logger.LogDebug("Running integration demo");

                // Step 1: Show info about what we're going to demonstrate
                await _dialogService.ShowInfoAsync(
                    "Integration Demo Starting",
                    "This demo will show how all three Avalonia.UIStudio libraries work together:\n\n" +
                    "1. Icons - Consistent iconography\n" +
                    "2. Appearance - Dynamic skin switching\n" +
                    "3. Dialogs - User interaction and feedback");

                // Step 2: Show current skin info (Appearance library)
                var currentSkin = _skinManager?.CurrentSkin;
                await _dialogService.ShowInfoAsync(
                    "Current Skin Information",
                    $"Active Skin: {currentSkin?.Name ?? "None"}\n" +
                    $"Available Skins: {AvailableSkinCount}\n" +
                    $"Icons: Dynamically colored based on skin\n" +
                    $"Dialogs: Automatically styled to match skin");

                // Step 3: Demonstrate skin switching with confirmation
                var availableSkins = _skinManager?.GetAvailableSkinNames() ?? new List<string>();
                if (availableSkins.Any())
                {
                    var shouldSwitch = await _dialogService.ShowConfirmationAsync(
                        "Skin Switching Demo",
                        "Would you like to demonstrate dynamic skin switching? " +
                        "This will temporarily change to a different skin and then back.",
                        "Yes, Demo Switching",
                        "Skip This Demo");

                    if (shouldSwitch)
                    {
                        // Find a different skin to switch to
                        var currentSkinName = currentSkin?.Name;
                        var demoSkin = availableSkins.FirstOrDefault(s => s != currentSkinName) ?? availableSkins.First();

                        // Switch to demo skin
                        _skinManager?.ApplySkin(demoSkin);

                        await _dialogService.ShowInfoAsync(
                            "Skin Changed!",
                            $"Notice how:\n" +
                            $"• All colors updated automatically\n" +
                            $"• Icons maintain proper contrast\n" +
                            $"• This dialog uses the new skin colors\n" +
                            $"• All controls are consistently styled\n\n" +
                            $"Current skin: {demoSkin}");

                        // Switch back
                        if (!string.IsNullOrEmpty(currentSkinName))
                        {
                            _skinManager?.ApplySkin(currentSkinName);

                            await _dialogService.ShowInfoAsync(
                                "Skin Restored",
                                $"Switched back to: {currentSkinName}\n\n" +
                                "This demonstrates the real-time skin switching capability!");
                        }
                    }
                }

                // Step 4: Final summary
                await _dialogService.ShowInfoAsync(
                    "Integration Demo Complete",
                    "Demo completed! This showcased:\n\n" +
                    "✓ Icons library providing consistent vector graphics\n" +
                    "✓ Appearance library managing skins and real-time switching\n" +
                    "✓ Dialogs library providing styled user interactions\n" +
                    "✓ All libraries working together seamlessly\n\n" +
                    "Each library can be used independently or together for maximum flexibility.");

                _logger.LogInformation("Integration demo completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to run integration demo");
                await ShowErrorAsync("Demo Error", "An error occurred during the integration demo", ex);
            }
        }

        private async Task ShowErrorAsync(string title, string message, Exception? exception = null)
        {
            try
            {
                await _dialogService.ShowErrorAsync(title, message, exception);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to show error dialog");
            }
        }

        private static Avalonia.Controls.Window? GetMainWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _skinManager.SkinChanged -= OnSkinChanged;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
