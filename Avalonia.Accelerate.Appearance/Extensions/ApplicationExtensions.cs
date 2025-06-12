using Avalonia.Markup.Xaml.Styling;

namespace Avalonia.Accelerate.Appearance.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Avalonia.Application"/> class to integrate AvaloniaSkinManager functionality.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Include AvaloniaSkinManager skins in your application
        /// </summary>
        /// <param name="app">The Application instance</param>
        /// <returns>The Application instance for method chaining</returns>
        public static Application IncludeSkinManagerStyles(this Application app)
        {
            app.Resources.MergedDictionaries.Add(new ResourceInclude(new Uri("avares://Avalonia.Accelerate.Appearance/"))
            {
                Source = new Uri("avares://Avalonia.Accelerate.Appearance/Skins/AppSkin.axaml")
            });

            return app;
        }
    }
}