using Avalonia.Accelerate.Dialogs.Interfaces;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Avalonia.Accelerate.Dialogs.Services
{
    /// <summary>
    /// Provides static methods for displaying dialogs similar to MessageBox.Show().
    /// </summary>
    public static class MessageBox
    {
        private static IServiceProvider? _serviceProvider;
        private static IDialogService? _fallbackService;

        /// <summary>
        /// Initializes the MessageBox with a service provider for dependency resolution.
        /// Call this during application startup.
        /// </summary>
        /// <param name="serviceProvider">The service provider containing the IDialogService registration.</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the dialog service instance, creating a fallback if necessary.
        /// </summary>
        private static IDialogService GetDialogService()
        {
            if (_serviceProvider != null)
            {
                try
                {
                    return _serviceProvider.GetRequiredService<IDialogService>();
                }
                catch
                {
                    // Fallback if service not available
                }
            }

            // Create fallback service if DI not available
            if (_fallbackService == null)
            {
                var logger = _serviceProvider?.GetService<ILogger<DialogService>>()
                    ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<DialogService>.Instance;
                _fallbackService = new DialogService(logger);
            }

            return _fallbackService;
        }

        /// <summary>
        /// Shows an informational dialog.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The dialog title. Defaults to "Information".</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ShowInfoAsync(string message, string title = "Information")
        {
            return GetDialogService().ShowInfoAsync(title, message);
        }

        /// <summary>
        /// Shows a warning dialog.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The dialog title. Defaults to "Warning".</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ShowWarningAsync(string message, string title = "Warning")
        {
            return GetDialogService().ShowWarningAsync(title, message);
        }

        /// <summary>
        /// Shows an error dialog.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The dialog title. Defaults to "Error".</param>
        /// <param name="exception">Optional exception details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ShowErrorAsync(string message, string title = "Error", Exception? exception = null)
        {
            return GetDialogService().ShowErrorAsync(title, message, exception);
        }

        /// <summary>
        /// Shows a confirmation dialog.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The dialog title. Defaults to "Confirmation".</param>
        /// <param name="confirmText">Text for the confirm button. Defaults to "Yes".</param>
        /// <param name="cancelText">Text for the cancel button. Defaults to "No".</param>
        /// <returns>A task representing the asynchronous operation with the user's choice.</returns>
        public static Task<bool> ShowConfirmationAsync(string message, string title = "Confirmation",
            string confirmText = "Yes", string cancelText = "No")
        {
            return GetDialogService().ShowConfirmationAsync(title, message, confirmText, cancelText);
        }

        /// <summary>
        /// Shows a validation errors dialog.
        /// </summary>
        /// <param name="errors">Collection of error messages.</param>
        /// <param name="warnings">Collection of warning messages.</param>
        /// <param name="title">The dialog title. Defaults to "Validation Issues".</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ShowValidationErrorsAsync(IEnumerable<string> errors, IEnumerable<string> warnings,
            string title = "Validation Issues")
        {
            return GetDialogService().ShowValidationErrorsAsync(title, errors, warnings);
        }

        // Synchronous versions for compatibility (use carefully - may block UI)

        /// <summary>
        /// Shows an informational dialog synchronously. Use async version when possible.
        /// </summary>
        public static void ShowInfo(string message, string title = "Information")
        {
            ShowInfoAsync(message, title).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Shows a warning dialog synchronously. Use async version when possible.
        /// </summary>
        public static void ShowWarning(string message, string title = "Warning")
        {
            ShowWarningAsync(message, title).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Shows an error dialog synchronously. Use async version when possible.
        /// </summary>
        public static void ShowError(string message, string title = "Error", Exception? exception = null)
        {
            ShowErrorAsync(message, title, exception).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Shows a confirmation dialog synchronously. Use async version when possible.
        /// </summary>
        public static bool ShowConfirmation(string message, string title = "Confirmation",
            string confirmText = "Yes", string cancelText = "No")
        {
            return ShowConfirmationAsync(message, title, confirmText, cancelText).GetAwaiter().GetResult();
        }


        /// <summary>
        /// Shows an open file dialog.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="filters">Optional filters</param>
        /// <param name="allowMultiple">Allow selecting multiple files</param>
        /// <returns>Array of selected file paths or empty array if cancelled</returns>
        public static Task<string[]> ShowOpenFileDialogAsync(string title, FileDialogFilter[]? filters = null, bool allowMultiple = false)
        {
            return GetDialogService().ShowOpenFileDialogAsync(title, filters, allowMultiple);
        }

        /// <summary>
        /// Shows a save file dialog.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="defaultFileName">Default file name</param>
        /// <param name="filters">Optional filters</param>
        /// <returns>Selected file path or null if cancelled</returns>
        public static Task<string?> ShowSaveFileDialogAsync(string title, string? defaultFileName = null, FileDialogFilter[]? filters = null)
        {
            return GetDialogService().ShowSaveFileDialogAsync(title, defaultFileName, filters);
        }

        /// <summary>
        /// Shows an open folder dialog.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <returns>Selected folder path or null if cancelled</returns>
        public static Task<string?> ShowOpenFolderDialogAsync(string title)
        {
            return GetDialogService().ShowOpenFolderDialogAsync(title);
        }

    }
}