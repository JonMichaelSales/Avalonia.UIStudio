using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Services
{
    /// <summary>
    /// Manages the skins for an Avalonia application, providing functionality to register, retrieve, and apply skins.
    /// </summary>
    /// <remarks>
    /// This class serves as a singleton instance to manage the available skins and the currently applied skin. 
    /// It provides methods to register new skins, retrieve existing skins by name, and apply a specific skin.
    /// Additionally, it raises events when the skin is changed, allowing other components to react to skin updates.
    /// </remarks>
    public class SkinManager : ISkinManager
    {
        private readonly ISkinLoaderService _skinLoaderService;
        private readonly IApplication _application;
        private readonly Dictionary<string, Skin?> _availableSkins = new();
        private Skin? _currentSkin;
        private readonly IStylesCollection _styles;
        private readonly List<IStyle> _appliedControlThemes = new();
        private readonly List<IResourceProvider> _appliedControlThemeDictionaries = new();

        /// <summary>
        /// Gets the singleton instance of the <see cref="SkinManager"/> class, 
        /// which is responsible for managing skins in an Avalonia application.
        /// </summary>
        /// <value>
        /// The singleton instance of <see cref="SkinManager"/>.
        /// </value>
        /// <remarks>
        /// This property ensures that only one instance of <see cref="SkinManager"/> exists throughout the application.
        /// It provides centralized access to skin management functionality, including registering, retrieving, and applying skins.
        /// </remarks>
        public static SkinManager? Instance { get; set; }

        /// <summary>
        /// Gets the currently applied <see cref="Skin"/> in the application.
        /// </summary>
        /// <value>
        /// The <see cref="Skin"/> instance representing the current skin, or <c>null</c> if no skin is applied.
        /// </value>
        /// <remarks>
        /// Use this property to retrieve or monitor the active skin in the application. 
        /// Changes to the current skin can be handled through the <see cref="SkinChanged"/> event.
        /// </remarks>
        public Skin? CurrentSkin
        {
            get => _currentSkin;
            private set => _currentSkin = value;
        }

        /// <summary>
        /// Event that is raised when the skin is changed.
        /// </summary>
        public event EventHandler? SkinChanged;


        /// <summary>
        /// Initializes a new instance of the <see cref="SkinManager"/> class with dependency injection.
        /// </summary>
        /// <param name="skinLoaderService">The skin loader service for loading available skins.</param>
        /// <param name="application">The application abstraction for accessing resources and styles.</param>
        public SkinManager(ISkinLoaderService skinLoaderService, IApplication application)
        {
            _skinLoaderService = skinLoaderService ?? throw new ArgumentNullException(nameof(skinLoaderService));
            _application = application ?? throw new ArgumentNullException(nameof(application));
            _styles = application.AppStyles ?? throw new InvalidOperationException("Application.AppStyles is null.");

            RegisterDefaultSkins();
        }


        private void RegisterDefaultSkins()
        {
            string skinsPath = "avares://Avalonia.UIStudio.Appearance/Skins/";
            var skins = _skinLoaderService.LoadSkins(skinsPath);
            foreach (var skin in skins)
            {
                RegisterSkin(skin.Name, skin);
            }
                
        }

        /// <summary>
        /// Registers a new skin with the specified name.
        /// </summary>
        /// <param name="name">The name of the skin to register. This value must not be <c>null</c>.</param>
        /// <param name="skin">The <see cref="Skin"/> instance to register. This value must not be <c>null</c>.</param>
        /// <remarks>
        /// If both <paramref name="name"/> and <paramref name="skin"/> are not <c>null</c>, the skin is added to the collection of available skins.
        /// </remarks>
        public void RegisterSkin(string? name, Skin? skin)
        {
            if (skin != null && name != null)
            {
                skin.Name = name;
                _availableSkins[name] = skin;
            }
        }

        /// <summary>
        /// Retrieves a <see cref="Skin"/> instance by its name.
        /// </summary>
        /// <param name="name">The name of the skin to retrieve. If <c>null</c>, the current skin is returned.</param>
        /// <returns>
        /// The <see cref="Skin"/> instance associated with the specified name, 
        /// or the current skin if the name is not found or is <c>null</c>.
        /// </returns>
        public Skin? GetSkin(string? name)
        {
            if (name != null && _availableSkins.TryGetValue(name, out Skin? skin))
                return skin;
            return _currentSkin;
        }

        /// <summary>
        /// Retrieves the names of all available skins registered in the <see cref="SkinManager"/>.
        /// </summary>
        /// <returns>A list of strings representing the names of the available skins.</returns>
        public List<string> GetAvailableSkinNames() => _availableSkins.Keys.ToList();

        /// <summary>
        /// Applies a skin to the application by its name.
        /// </summary>
        /// <param name="skinName">
        /// The name of the skin to apply. If the skin with the specified name is not found, 
        /// an error message will be logged.
        /// </param>
        /// <remarks>
        /// If the specified skin exists, it will be applied, and the selected skin will be saved.
        /// If the skin does not exist or an error occurs during the application, an appropriate 
        /// message will be logged.
        /// </remarks>
        /// <exception cref="Exception">
        /// Logs any exception that occurs while applying the skin.
        /// </exception>
        public void ApplySkin(string? skinName)
        {
            try
            {
                if (skinName != null && _availableSkins.TryGetValue(skinName, out Skin? skin))
                {
                    ApplySkin(skin);
                    SaveSelectedSkin(skinName);
                }
                else
                {
                    Console.WriteLine($"Skin not found: {skinName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying skin {skinName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies the specified skin to the application, updating resources and triggering the <see cref="SkinChanged"/> event.
        /// </summary>
        /// <param name="skin">The <see cref="Skin"/> to be applied. If <c>null</c>, a default skin will be applied.</param>
        /// <remarks>
        /// This method updates the application's resources and typography settings based on the provided skin.
        /// If an exception occurs during the application of the skin, it will be logged to the console.
        /// </remarks>
        public void ApplySkin(Skin? skin)
        {
            if (skin == null)
                skin = new Skin();

            _currentSkin = skin;

            try
            {
                UpdateResources();
                UpdateTypographyResources(skin);
                SkinChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying custom skin: {ex.Message}");
            }
        }

        private void UpdateTypographyResources(Skin skin)
        {
            var resources = _application.Resources;
            if (resources == null) return;

            try
            {
                // Typography scale
                if (skin.Typography != null)
                {
                    resources["DisplayLargeFontSize"] = skin.Typography.DisplayLarge;
                    resources["DisplayMediumFontSize"] = skin.Typography.DisplayMedium;
                    resources["DisplaySmallFontSize"] = skin.Typography.DisplaySmall;
                    resources["HeadlineLargeFontSize"] = skin.Typography.HeadlineLarge;
                    resources["HeadlineMediumFontSize"] = skin.Typography.HeadlineMedium;
                    resources["HeadlineSmallFontSize"] = skin.Typography.HeadlineSmall;
                    resources["TitleLargeFontSize"] = skin.Typography.TitleLarge;
                    resources["TitleMediumFontSize"] = skin.Typography.TitleMedium;
                    resources["TitleSmallFontSize"] = skin.Typography.TitleSmall;
                    resources["LabelLargeFontSize"] = skin.Typography.LabelLarge;
                    resources["LabelMediumFontSize"] = skin.Typography.LabelMedium;
                    resources["LabelSmallFontSize"] = skin.Typography.LabelSmall;
                    resources["BodyLargeFontSize"] = skin.Typography.BodyLarge;
                    resources["BodyMediumFontSize"] = skin.Typography.BodyMedium;
                    resources["BodySmallFontSize"] = skin.Typography.BodySmall;
                }

                // Font families
                resources["HeaderFontFamily"] = skin.HeaderFontFamily;
                resources["BodyFontFamily"] = skin.BodyFontFamily;
                resources["MonospaceFontFamily"] = skin.MonospaceFontFamily;

                // Text properties
                resources["DefaultLineHeight"] = skin.LineHeight;
                resources["DefaultLetterSpacing"] = skin.LetterSpacing;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating typography resources: {ex.Message}");
            }
        }

        private void UpdateResources()
        {
            if (_currentSkin == null)
                return;

            var resources = _application.Resources;
            if (resources == null)
                return;

            UpdateBrush(resources, "PrimaryColorBrush", _currentSkin.PrimaryColor);
            UpdateBrush(resources, "SecondaryColorBrush", _currentSkin.SecondaryColor);
            UpdateBrush(resources, "AccentBlueBrush", _currentSkin.AccentColor);
            UpdateBrush(resources, "GunMetalDarkBrush", _currentSkin.PrimaryColor);
            UpdateBrush(resources, "GunMetalMediumBrush", _currentSkin.SecondaryColor);
            UpdateBrush(resources, "GunMetalLightBrush", _currentSkin.SecondaryBackground);
            UpdateBrush(resources, "BackgroundBrush", _currentSkin.PrimaryBackground);
            UpdateBrush(resources, "BackgroundLightBrush", _currentSkin.SecondaryBackground);
            var dark = new Color(_currentSkin.PrimaryBackground.A, (byte)(_currentSkin.PrimaryBackground.R * 0.8), (byte)(_currentSkin.PrimaryBackground.G * 0.8), (byte)(_currentSkin.PrimaryBackground.B * 0.8));
            UpdateBrush(resources, "BackgroundDarkBrush", dark);
            UpdateBrush(resources, "TextPrimaryBrush", _currentSkin.PrimaryTextColor);
            UpdateBrush(resources, "TextSecondaryBrush", _currentSkin.SecondaryTextColor);
            UpdateBrush(resources, "BorderBrush", _currentSkin.BorderColor);
            UpdateBrush(resources, "ErrorBrush", _currentSkin.ErrorColor);
            UpdateBrush(resources, "WarningBrush", _currentSkin.WarningColor);
            UpdateBrush(resources, "SuccessBrush", _currentSkin.SuccessColor);

            resources["DefaultFontFamily"] = _currentSkin.FontFamily;
            resources["FontSizeSmall"] = _currentSkin.FontSizeSmall;
            resources["FontSizeMedium"] = _currentSkin.FontSizeMedium;
            resources["FontSizeLarge"] = _currentSkin.FontSizeLarge;
            resources["DefaultFontWeight"] = _currentSkin.FontWeight;
            resources["BorderThickness"] = _currentSkin.BorderThickness;
            resources["CornerRadius"] = new CornerRadius(_currentSkin.BorderRadius);

            ApplyControlThemes(_currentSkin);
            ForceVisualUpdate();
        }

        private void ApplyControlThemes(Skin skin)
        {
            // Remove previously applied styles from Styles collection
            foreach (var style in _appliedControlThemes)
            {
                if (_styles.Contains(style))
                    _styles.Remove(style);
            }

            _appliedControlThemes.Clear();

            // Clear previously applied control themes from MergedDictionaries
            foreach (var controlThemeDict in _appliedControlThemeDictionaries)
            {
                _application.Resources.MergedDictionaries.Remove(controlThemeDict);
            }

            _appliedControlThemeDictionaries.Clear();
            
                // ✅ Add new ControlTheme dictionaries
                foreach (var kvp in skin.ControlThemeUris)
                {
                    Uri baseUri = new Uri($"avares://Avalonia.UIStudio.Appearance/Skins/{skin.Name}/", UriKind.RelativeOrAbsolute);

                    // Create a ResourceInclude instance, specifying the base URI and the source file
                    ResourceInclude myResourceInclude = new ResourceInclude(baseUri)
                    {
                        Source = new Uri(kvp.Value, UriKind.Relative)
                    };
                try
                {
                    _application.Resources.MergedDictionaries.Add(myResourceInclude);
                    _appliedControlThemeDictionaries.Add(myResourceInclude);
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
            }
            


            // ✅ Add new styles (normal StyleInclude)
            foreach (var kvp in skin.StyleUris)
            {
                if (kvp.Value != null)
                {
                    var style = new StyleInclude(new Uri("avares://Avalonia.UIStudio.Appearance/"))
                    {
                        Source = new Uri(kvp.Value)
                    };

                    _styles.Add(style);
                    _appliedControlThemes.Add(style);
                }
            }
        }


        private void UpdateBrush(IResourceDictionary dict, string key, Color color)
        {
            if (dict.TryGetValue(key, out var existingBrush) && existingBrush is SolidColorBrush brush)
                brush.Color = color;
            else
                dict[key] = new SolidColorBrush(color);
        }

        private void ForceVisualUpdate()
        {
            if (_application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (var window in desktop.Windows)
                {
                    window.InvalidateVisual();
                    InvalidateRecursive(window);
                }
            }
        }

        private void InvalidateRecursive(Control control)
        {
            control.InvalidateVisual();

            if (control is Panel panel)
            {
                foreach (var child in panel.Children)
                    InvalidateRecursive(child);
            }
            else if (control is ContentControl cc && cc.Content is Control c)
                InvalidateRecursive(c);
            else if (control is ItemsControl ic && ic.ItemsPanelRoot is Control ip)
                InvalidateRecursive(ip);
        }

        /// <summary>
        /// Saves the name of the currently selected skin to the application settings.
        /// </summary>
        /// <param name="skinName">
        /// The name of the skin to save. If <c>null</c>, no action is performed.
        /// </param>
        /// <remarks>
        /// This method updates the skin name in the application settings and persists the changes.
        /// It is typically called after applying a new skin to ensure the selected skin is remembered
        /// across application sessions.
        /// </remarks>
        public void SaveSelectedSkin(string? skinName)
        {
            if (skinName != null)
            {
                AppSettings.Instance.Skin = skinName;
                AppSettings.Instance.Save();
            }
        }

        public string GetSkinFilePath(Skin skin)
        {
            // You may want to make this more configurable
            return $"./Skins/{skin.Name}/skin.json";
        }

        public void ReloadSkins()
        {
            _availableSkins.Clear();
            RegisterDefaultSkins();
            LoadSavedSkin();
        }

        /// <summary>
        /// Loads the previously saved skin and applies it to the application.
        /// </summary>
        /// <remarks>
        /// This method retrieves the saved skin name from the application settings and applies it if it exists
        /// in the list of available skins. If no saved skin is found or the saved skin is not available,
        /// no changes are made to the current skin.
        /// </remarks>
        /// <example>
        /// Example usage:
        /// <code>
        /// var skinManager = SkinManager.Instance;
        /// skinManager.LoadSavedSkin();
        /// </code>
        /// </example>
        public void LoadSavedSkin()
        {
            var skinName = AppSettings.Instance.Skin;
            if (!string.IsNullOrEmpty(skinName) && _availableSkins.ContainsKey(skinName))
                ApplySkin(skinName);
        }
    }
}

