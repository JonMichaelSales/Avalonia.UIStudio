using Avalonia.Accelerate.Dialogs.Dialogs;
using Avalonia.Accelerate.Dialogs.Interfaces;
using Avalonia.Accelerate.Dialogs.Models;
using Avalonia.Accelerate.Dialogs.Utility;
using Avalonia.Controls;
using Microsoft.Extensions.Logging;

namespace Avalonia.Accelerate.Dialogs.Services
{
    public class DialogService : IDialogService
    {
        private readonly ILogger<DialogService> _logger;
        private readonly bool _testAutoCloseDialogs;

        public DialogService(ILogger<DialogService> logger)
        {
            _logger = logger;
        }

        public DialogService(ILogger<DialogService> logger, bool testAutoCloseDialogs = false)
        {
            _logger = logger;
            _testAutoCloseDialogs = testAutoCloseDialogs;
        }

        public async Task ShowErrorAsync(string title, string message, Exception? exception = null)
        {
            _logger.LogError(exception, "Error dialog shown: {Title} - {Message}", title, message);

            var dialog = new ErrorDialog
            {
                Title = title,
                Message = message,
                Exception = exception
            };

            await ShowDialogSafeAsync(dialog);
        }

        public async Task ShowWarningAsync(string title, string message)
        {
            _logger.LogWarning("Warning dialog shown: {Title} - {Message}", title, message);

            var dialog = new NotificationDialog
            {
                Title = title,
                Message = message,
                DialogType = NotificationDialogType.Warning
            };

            await ShowDialogSafeAsync(dialog);
        }

        public async Task ShowInfoAsync(string title, string message)
        {
            _logger.LogInformation("Info dialog shown: {Title} - {Message}", title, message);

            var dialog = new NotificationDialog
            {
                Title = title,
                Message = message,
                DialogType = NotificationDialogType.Information
            };

            await ShowDialogSafeAsync(dialog);
        }

        public async Task ShowValidationErrorsAsync(string title, IEnumerable<string> errors, IEnumerable<string> warnings)
        {
            var errorList = errors.ToList();
            var warningList = warnings.ToList();

            _logger.LogWarning("Validation dialog shown: {Title} - {ErrorCount} errors, {WarningCount} warnings",
                title, errorList.Count, warningList.Count);

            var dialog = new ValidationErrorDialog
            {
                Title = title,
                Errors = errorList,
                Warnings = warningList
            };

            await ShowDialogSafeAsync(dialog);
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message, string confirmText = "Yes", string cancelText = "No")
        {
            _logger.LogDebug("Confirmation dialog shown: {Title} - {Message}", title, message);

            var dialog = new ConfirmationDialog
            {
                Title = title,
                Message = message,
                ConfirmText = confirmText,
                CancelText = cancelText
            };

            return await ShowDialogSafeAsync<bool>(dialog);
        }

        /// <summary>
        /// Helper to safely show dialogs with proper window visibility handling.
        /// </summary>
        private async Task<TResult?> ShowDialogSafeAsync<TResult>(Window dialog)
        {
            if (WindowTools.TryGetMainWindow() is Window mainWindow)
            {
                if (!mainWindow.IsVisible)
                {
                    mainWindow.Show();
                }

                if (_testAutoCloseDialogs)
                {
                    dialog.Opened += (_, __) =>
                    {
                        if (dialog is ConfirmationDialog confirmationDialog)
                        {
                            confirmationDialog.ConfirmButton.RaiseEvent(
                                new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                        }
                        else
                        {
                            dialog.Close();
                        }
                    };

                    // ✅ Use ShowDialog to correctly block and return DialogResult
                    return await dialog.ShowDialog<TResult?>(mainWindow);
                }

                return await dialog.ShowDialog<TResult?>(mainWindow);
            }
            else
            {
                var fallback = new Window();
                fallback.Show();

                if (_testAutoCloseDialogs)
                {
                    dialog.Opened += (_, __) =>
                    {
                        if (dialog is ConfirmationDialog confirmationDialog)
                        {
                            confirmationDialog.ConfirmButton.RaiseEvent(
                                new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                        }
                        else
                        {
                            dialog.Close();
                        }
                    };

                    // ✅ Use ShowDialog to correctly block and return DialogResult
                    return await dialog.ShowDialog<TResult?>(fallback);
                }

                return await dialog.ShowDialog<TResult?>(fallback);
            }
        }


        private async Task ShowDialogSafeAsync(Window dialog)
        {
            if (WindowTools.TryGetMainWindow() is Window mainWindow)
            {
                if (!mainWindow.IsVisible)
                {
                    mainWindow.Show();
                }

                if (_testAutoCloseDialogs)
                {
                    dialog.Opened += (_, __) => dialog.Close();
                    dialog.Show(mainWindow);
                    return;
                }

                await dialog.ShowDialog<object?>(mainWindow);
            }
            else
            {
                var fallback = new Window();
                fallback.Show();

                if (_testAutoCloseDialogs)
                {
                    dialog.Opened += (_, __) => dialog.Close();
                    dialog.Show(fallback);
                    return;
                }

                await dialog.ShowDialog<object?>(fallback);
            }
        }
    }
}
