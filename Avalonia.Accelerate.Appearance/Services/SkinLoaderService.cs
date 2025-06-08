using System.Text.Json;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Platform;

namespace Avalonia.Accelerate.Appearance.Services
{
    /// <summary>
    /// Defines a service for loading themes in an Avalonia application.
    /// </summary>
    /// <remarks>
    /// This interface provides methods to load theme skins from a specified directory.
    /// Implementations of this interface are responsible for parsing and managing theme-related resources.
    /// </remarks>
    public interface IThemeLoaderService
    {
        /// <summary>
        /// Loads a collection of theme skins from the specified root directory.
        /// </summary>
        /// <param name="themesRoot">
        /// The root directory containing theme skin definitions.
        /// </param>
        /// <returns>
        /// A list of <see cref="Skin"/> objects representing the loaded theme skins.
        /// </returns>
        List<Skin> LoadSkins(string themesRoot);
    }

    /// <summary>
    /// Provides functionality to load and manage themes for the Avalonia application.
    /// </summary>
    /// <remarks>
    /// This service is responsible for loading theme configurations and resources from a specified directory structure.
    /// It processes theme definitions, control themes, and styles, making them available for use within the application.
    /// </remarks>
    public class SkinLoaderService : IThemeLoaderService
    {

        // List of known embedded themes (keep in sync with package)
        private readonly string[] _embeddedSkins = new[]
        {
            "Dark", "Light", "Ocean Blue", "Cyberpunk",
            "RetroTerminal", "Purple Haze", "Forest Green", "High Contrast", "ModernIce", "Windows 11 Modern", "Zen Garden","Material Design 3"
        };


        public SkinLoaderService()
        {
            
            
        }

        /// <summary>
        /// Loads a collection of <see cref="Skin"/> objects from the specified root directory.
        /// </summary>
        /// <returns>
        /// A list of <see cref="Skin"/> objects representing the loaded themes.
        /// </returns>
        /// <remarks>
        /// This method scans the specified directory for subdirectories containing theme definitions.
        /// Each theme is expected to have a "theme.json" file and optionally "ControlThemes" and "Styles" directories
        /// containing .axaml files. The method parses these resources and constructs <see cref="Skin"/> objects
        /// with appropriate URIs for control themes and styles.
        /// </remarks>
        public List<Skin> LoadSkins(string themesRoot = "avares://Avalonia.Accelerate.Appearance/Skins/")
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var skins = new List<Skin>();

            // Get folders under themesRoot
            var baseUri = new Uri(themesRoot);

            try
            {
                var themeJsonAssets = AssetLoader.GetAssets(new Uri(themesRoot), null)
                    .Where(uri => uri.ToString().EndsWith("/theme.json", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                foreach (var themeJsonUri in themeJsonAssets)
                {
                    try
                    {
                        using var stream = AssetLoader.Open(themeJsonUri);
                        using var reader = new StreamReader(stream);
                        var json = reader.ReadToEnd();

                        var serializableTheme = JsonSerializer.Deserialize<SerializableSkin>(json, jsonOptions);
                        if (serializableTheme is null) continue;

                        var skin = serializableTheme.ToSkin();

                        skins.Add(skin);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load theme at '{themeJsonUri}': {ex.Message}");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to enumerate theme folders: {ex.Message}");
            }

            // Inheritance resolution
            foreach (var skin in skins)
            {
                if (skin.BaseSkin is { } baseskin)
                {
                    if (!string.IsNullOrWhiteSpace(baseskin.BaseSkinName))
                    {
                        var baseSkin = skins.FirstOrDefault(s => s.Name != null && s.Name.Equals(baseskin.BaseSkinName, StringComparison.OrdinalIgnoreCase));
                        if (baseSkin != null)
                        {
                            skin.InheritFrom(baseSkin);
                        }
                    }
                }
            }

            return skins;
        }
    }
}
