using Newtonsoft.Json;

namespace Avalonia.Accelerate.Appearance.Model
{
    /// <summary>
    /// Represents the application settings for the Avalonia Skin Manager.
    /// Provides functionality to load, save, and manage skin-related settings.
    /// </summary>
    public class AppSettings
    {
        private const string SettingsFileName = "appsettings.json";
        private static readonly string SettingsFilePath;
        private static AppSettings? _instance;

        /// <summary>
        /// Gets or sets the name of the currently selected skin.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the skin name. The default value is "Dark".
        /// </value>
        /// <remarks>
        /// This property is used to store the name of the skin selected by the user.
        /// It can be updated dynamically and is persisted using the <see cref="AppSettings.Save"/> method.
        /// </remarks>
        public string? Skin { get; set; } = "Dark";

        // Add other settings as needed
        /// <summary>
        /// Gets or sets a value indicating whether the application should use the system's default skin.
        /// </summary>
        /// <value>
        /// <c>true</c> if the application should use the system's skin; otherwise, <c>false</c>.
        /// </value>
        public bool UseSystemskin { get; set; } = false;

        // Update the static constructor in AppSettings.cs
        static AppSettings()
        {
            string appDataFolder;

            try
            {
                // Try to use a more appropriate app data folder
                appDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Avalonia.Accelerate.Appearance");
            }
            catch
            {
                // Fallback to current directory if permissions issue
                appDataFolder = Path.Combine(AppContext.BaseDirectory, "Settings");
            }

            if (!Directory.Exists(appDataFolder))
            {
                try
                {
                    Directory.CreateDirectory(appDataFolder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Could not create settings directory: {ex.Message}");
                    // Fallback to temp directory
                    appDataFolder = Path.GetTempPath();
                }
            }

            SettingsFilePath = Path.Combine(appDataFolder, SettingsFileName);
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="AppSettings"/> class.
        /// </summary>
        /// <remarks>
        /// This property ensures that only one instance of <see cref="AppSettings"/> exists throughout the application.
        /// If the instance is not already initialized, it will be loaded using the <c>Load</c> method.
        /// </remarks>
        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Load();
                }
                return _instance;
            }
        }

        private static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    var settings = JsonConvert.DeserializeObject<AppSettings>(json);
                    if (settings != null)
                        return settings;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }

            // Return default settings if loading fails
            return new AppSettings();
        }

        /// <summary>
        /// Saves the current application settings to a file.
        /// </summary>
        /// <remarks>
        /// This method serializes the current instance of <see cref="AppSettings"/> into a JSON format
        /// and writes it to the file specified by the settings file path. If an error occurs during
        /// the save operation, it is logged to the console.
        /// </remarks>
        /// <exception cref="System.IO.IOException">
        /// Thrown when an I/O error occurs while writing to the file.
        /// </exception>
        /// <exception cref="Newtonsoft.Json.JsonException">
        /// Thrown when an error occurs during JSON serialization.
        /// </exception>
        public void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}