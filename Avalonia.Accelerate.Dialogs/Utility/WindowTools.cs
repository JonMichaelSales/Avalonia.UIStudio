using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Avalonia.Accelerate.Dialogs.Utility
{
    /// <summary>
    /// Provides utility methods for working with Avalonia <see cref="Window"/> instances,
    /// including retrieving the main application window.
    /// </summary>
    public static class WindowTools
    {
        /// <summary>
        /// Attempts to retrieve the main application <see cref="Window"/>.
        /// </summary>
        /// <returns>
        /// The main <see cref="Window"/> instance of the application if available; otherwise, <c>null</c>.
        /// </returns>
        public static Window? TryGetMainWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            return null;
        }

        /// <summary>
        /// Retrieves the main application window.
        /// </summary>
        /// <returns>The main <see cref="Window"/> instance of the application.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the main window is not available.
        /// </exception>
        public static Window GetMainWindow()
        {
            var mainWindow = TryGetMainWindow();
            if(mainWindow==null)
            {
                var main = new Window();
                return main;
            }
            return mainWindow;
        }
    }

}
