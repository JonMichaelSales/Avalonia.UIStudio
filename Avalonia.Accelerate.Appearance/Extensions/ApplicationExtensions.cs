using Avalonia.Markup.Xaml.Styling;

namespace Avalonia.Accelerate.Appearance.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Avalonia.Application"/> class to integrate AvaloniaThemeManager functionality.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Include AvaloniaThemeManager themes in your application
        /// </summary>
        /// <param name="app">The Application instance</param>
        /// <returns>The Application instance for method chaining</returns>
        public static Application IncludeThemeManagerStyles(this Application app)
        {
            // Include the theme manager's default styles
            app.Styles.Add(new StyleInclude(new System.Uri("avares://Avalonia.Accelerate.Appearance/"))
            {
                Source = new System.Uri("avares://Avalonia.Accelerate.Appearance/Skins/CustomThemes.axaml")
            });

            return app;
        }
    }
}