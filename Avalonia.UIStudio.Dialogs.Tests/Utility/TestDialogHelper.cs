using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.UIStudio.Dialogs.Utility;

namespace Avalonia.UIStudio.Dialogs.Tests.Utility
{
    /// <summary>
    /// Provides helper methods for showing dialogs in integration tests.
    /// </summary>
    public static class TestDialogHelper
    {
        /// <summary>
        /// Shows the specified dialog in a test-safe way.
        /// If MainWindow is not available, opens a temporary dummy window.
        /// </summary>
        /// <param name="dialog">The dialog to show.</param>
        /// <typeparam name="TResult">The dialog result type.</typeparam>
        /// <returns>The result of the dialog.</returns>
        public static async Task<TResult?> ShowTestDialog<TResult>(Window dialog)
        {
            Window? mainWindow = WindowTools.TryGetMainWindow();

            if (mainWindow == null)
            {
                // Create a dummy window for test hosting
                mainWindow = new Window
                {
                    Title = "Test Host Window",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                mainWindow.Show();

                // Give UI thread a moment to render
                await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);
            }

            return await dialog.ShowDialog<TResult?>(mainWindow);
        }

        /// <summary>
        /// Shows a dialog without caring about result (void).
        /// </summary>
        public static async Task ShowTestDialog(Window dialog)
        {
            Window? mainWindow = WindowTools.TryGetMainWindow();

            if (mainWindow == null)
            {
                mainWindow = new Window
                {
                    Title = "Test Host Window",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                mainWindow.Show();

                await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);
            }

            await dialog.ShowDialog(mainWindow);
        }
    }
}
