using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Media;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Avalonia.Accelerate.Appearance.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing theme settings within the application.
    /// </summary>
    /// <remarks>
    /// This class provides functionality to load available themes, apply a selected theme, 
    /// and reset to a default theme. It interacts with the UI to allow users to preview and 
    /// change themes dynamically. Logging is utilized to track theme changes and operations.
    /// </remarks>
    public class SkinSettingsViewModel : ViewModelBase
    {
        private readonly ILogger _logger;
        private ThemeInfo? _selectedTheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinSettingsViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the ViewModel with a default logger instance. It initializes
        /// the collection of available themes, the command for applying themes, and loads the
        /// current theme and available themes.
        /// </remarks>
        public SkinSettingsViewModel() : this(
            Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinSettingsViewModel"/> class with the specified logger.
        /// </summary>
        /// <param name="logger">
        /// An instance of <see cref="ILogger"/> used for logging theme-related operations and errors.
        /// </param>
        /// <remarks>
        /// This constructor allows dependency injection of a logger instance, enabling detailed logging
        /// of theme management operations. It initializes the collection of available themes, sets up
        /// the command for applying themes, and loads the current theme and available themes.
        /// </remarks>
        public SkinSettingsViewModel(ILogger logger)
        {
            _logger = logger;

            AvailableThemes = new ObservableCollection<ThemeInfo>();
            ApplyThemeCommand = ReactiveCommand.Create(ApplyTheme);

            LoadAvailableThemes();
            LoadCurrentTheme();
        }

        /// <summary>
        /// Gets the collection of available themes that can be applied within the application.
        /// </summary>
        /// <remarks>
        /// This property provides a list of <see cref="ThemeInfo"/> objects, each representing a theme 
        /// with its name, description, and preview color. The collection is populated by the 
        /// <c>LoadAvailableThemes</c> method and is used to display theme options in the UI.
        /// </remarks>
        public ObservableCollection<ThemeInfo> AvailableThemes { get; }
        /// <summary>
        /// Gets the command used to apply the currently selected theme.
        /// </summary>
        /// <remarks>
        /// This command executes the logic to apply the theme selected by the user in the UI.
        /// It ensures that the application's appearance is updated dynamically to reflect the chosen theme.
        /// </remarks>
        public ReactiveCommand<Unit, Unit> ApplyThemeCommand { get; }

        /// <summary>
        /// Gets or sets the currently selected theme.
        /// </summary>
        /// <remarks>
        /// When a new theme is selected, it is immediately applied to the application for preview purposes.
        /// The selected theme is logged for tracking purposes. If the theme is set to <c>null</c>, no changes are applied.
        /// </remarks>
        public ThemeInfo? SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _selectedTheme, value) != null)
                {
                    // Apply theme immediately for preview
                    if (value != null)
                    {
                        SkinManager.Instance?.ApplySkin(value.Name);
                        _logger.LogInformation("Skin changed to: {ThemeName}", value.Name);
                    }
                }
            }
        }

        private void LoadAvailableThemes()
        {
            try
            {
                var skinManager = SkinManager.Instance;
                var themeNames = skinManager?.GetAvailableSkinNames();

                AvailableThemes.Clear();

                if (themeNames != null)
                    foreach (var themeName in themeNames)
                    {
                        var skin = skinManager?.GetSkin(themeName);
                        if (skin != null)
                        {
                            var themeInfo = new ThemeInfo
                            {
                                Name = themeName,
                                Description = GetThemeDescription(themeName),
                                PreviewColor = new SolidColorBrush(skin.AccentColor)
                            };
                            AvailableThemes.Add(themeInfo);
                        }
                    }

                _logger.LogInformation("Loaded {ThemeCount} available themes", AvailableThemes.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load available themes");
            }
        }

        private void LoadCurrentTheme()
        {
            try
            {
                var currentSkin = SkinManager.Instance?.CurrentSkin;
                if (currentSkin?.Name != null)
                {
                    SelectedTheme = AvailableThemes.FirstOrDefault(t => t.Name == currentSkin.Name);
                }

                // Fallback to Dark theme if current theme not found
                SelectedTheme ??= AvailableThemes.FirstOrDefault(t => t.Name == "Dark");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load current theme");
            }
        }

        private void ApplyTheme()
        {
            try
            {
                if (SelectedTheme != null)
                {
                    SkinManager.Instance?.ApplySkin(SelectedTheme.Name);
                    _logger.LogInformation("Applied theme: {ThemeName}", SelectedTheme.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to apply theme: {ThemeName}", SelectedTheme?.Name);
            }
        }

        /// <summary>
        /// Resets the theme settings to the default theme.
        /// </summary>
        /// <remarks>
        /// This method attempts to reset the currently selected theme to the default theme, 
        /// which is identified by the name "Dark". If the default theme is found in the 
        /// <see cref="AvailableThemes"/> collection, it is applied as the selected theme. 
        /// Logs information about the operation or errors if the reset fails.
        /// </remarks>
        /// <exception cref="Exception">
        /// Logs any exceptions that occur during the reset operation.
        /// </exception>
        public void ResetToDefault()
        {
            try
            {
                var defaultTheme = AvailableThemes.FirstOrDefault(t => t.Name == "Dark");
                if (defaultTheme != null)
                {
                    SelectedTheme = defaultTheme;
                    _logger.LogInformation("Reset to default theme: Dark");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reset to default theme");
            }
        }

        private static string GetThemeDescription(string themeName)
        {
            return themeName switch
            {
                "Dark" => "Professional dark theme with blue accents. Easy on the eyes for extended use.",
                "Light" => "Clean light theme with dark text. Perfect for bright environments.",
                "Ocean Blue" => "Deep blue theme inspired by ocean depths. Calming and focused.",
                "Forest Green" => "Nature-inspired green theme. Relaxing and earthy.",
                "Purple Haze" => "Rich purple theme with mystical vibes. Creative and bold.",
                "High Contrast" => "Maximum contrast for accessibility. Clear and distinct colors.",
                "Cyberpunk" => "Futuristic neon theme with hot pink accents. Edgy and modern.",
                _ => "Custom theme with unique color combinations."
            };
        }
    }

    /// <summary>
    /// Represents information about a theme, including its name, description, and a preview color.
    /// </summary>
    /// <remarks>
    /// This class is used to encapsulate the details of a theme, which can be displayed in the UI
    /// or used for theme management purposes within the application.
    /// </remarks>
    public class ThemeInfo
    {
        /// <summary>
        /// Gets or sets the name of the theme.
        /// </summary>
        /// <remarks>
        /// The name uniquely identifies the theme and is used for selection and application purposes.
        /// </remarks>
        public string Name { get; set; } = "";
        /// <summary>
        /// Gets or sets the description of the theme.
        /// </summary>
        /// <remarks>
        /// This property provides a textual description of the theme, which can be displayed in the user interface
        /// to give users more context about the theme's purpose or appearance.
        /// </remarks>
        public string Description { get; set; } = "";
        /// <summary>
        /// Gets or sets the brush used to represent the preview color of the theme.
        /// </summary>
        /// <remarks>
        /// This property is typically used to display a visual representation of the theme's accent color
        /// in the user interface, such as in theme selection controls.
        /// </remarks>
        public IBrush PreviewColor { get; set; } = Brushes.Transparent;
    }
}