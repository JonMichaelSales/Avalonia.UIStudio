using System.Text.Json;
using Avalonia.Platform;
using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Services
{
    /// <summary>
    /// Defines a service for loading skins in an Avalonia application.
    /// </summary>
    /// <remarks>
    /// This interface provides methods to load skins skins from a specified directory.
    /// Implementations of this interface are responsible for parsing and managing skin-related resources.
    /// </remarks>
    public interface ISkinLoaderService
    {
        /// <summary>
        /// Loads a collection of skins from the specified root directory.
        /// </summary>
        /// <param name="skinsRoot">
        /// The root directory containing skin definitions.
        /// </param>
        /// <returns>
        /// A list of <see cref="Skin"/> objects representing the loaded skins.
        /// </returns>
        List<Skin> LoadSkins(string skinsRoot);
    }

    /// <summary>
    /// Provides functionality to load and manage skins for the Avalonia application.
    /// </summary>
    /// <remarks>
    /// This service is responsible for loading skins configurations and resources from a specified directory structure.
    /// It processes skins definitions, controlthemes, and styles, making them available for use within the application.
    /// </remarks>
    public class SkinLoaderService : ISkinLoaderService
    {

        // List of known embedded skins (keep in sync with package)
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
        /// A list of <see cref="Skin"/> objects representing the loaded skins.
        /// </returns>
        /// <remarks>
        /// This method scans the specified directory for subdirectories containing skin definitions.
        /// Each skin is expected to have a "skin.json" file and optionally "ControlThemes" and "Styles" directories
        /// containing .axaml files. The method parses these resources and constructs <see cref="Skin"/> objects
        /// with appropriate URIs for controlthemes and styles.
        /// </remarks>
        public List<Skin> LoadSkins(string skinsRoot = "avares://Avalonia.UIStudio.Appearance/Skins/")
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var skins = new List<Skin>();

            var baseUri = new Uri(skinsRoot);

            // Build skin names list from avares assets
            List<string> skinNames = new List<string>();

            try
            {
                var skinJsonAssets = AssetLoader.GetAssets(new Uri(skinsRoot), null)
                    .Where(uri => uri.ToString().EndsWith("/skin.json", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // Extract skin names from avares uris
                foreach (var skinJsonUri in skinJsonAssets)
                {
                    var parts = skinJsonUri.ToString().Split('/');
                    if (parts.Length >= 2)
                    {
                        var skinName = parts[^2];  // the folder name before /skin.json
                        if (!string.IsNullOrWhiteSpace(skinName) && !skinNames.Contains(skinName))
                            skinNames.Add(skinName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to enumerate skin folders: {ex.Message}");
            }

            // Now load each skin, UserOverrides first
            foreach (var skinName in skinNames)
            {
                bool loadedFromOverride = false;

                var userOverridePath = $"./Skins/UserOverrides/{skinName}/skin.json";
                if (File.Exists(userOverridePath))
                {
                    try
                    {
                        var json = File.ReadAllText(userOverridePath);
                        var serializableSkin = JsonSerializer.Deserialize<SerializableSkin>(json, jsonOptions);
                        if (serializableSkin != null)
                        {
                            var skin = serializableSkin.ToSkin();
                            skins.Add(skin);
                            loadedFromOverride = true;
                            Console.WriteLine($"Loaded skin '{skinName}' from UserOverrides.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load user override skin '{skinName}': {ex.Message}");
                    }
                }

                if (!loadedFromOverride)
                {
                    try
                    {
                        var avaresPath = $"{skinsRoot}{skinName}/skin.json";
                        var avaresUri = new Uri(avaresPath);

                        using var stream = AssetLoader.Open(avaresUri);
                        using var reader = new StreamReader(stream);
                        var json = reader.ReadToEnd();

                        var serializableSkin = JsonSerializer.Deserialize<SerializableSkin>(json, jsonOptions);
                        if (serializableSkin is null) continue;

                        var skin = serializableSkin.ToSkin();
                        skins.Add(skin);
                        Console.WriteLine($"Loaded built-in skin '{skinName}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load built-in skin '{skinName}': {ex.Message}");
                    }
                }
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
