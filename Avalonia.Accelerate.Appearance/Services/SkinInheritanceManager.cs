using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;

namespace Avalonia.Accelerate.Appearance.Services
{
    /// <summary>
    /// Manages theme inheritance and variant creation with dependency injection support.
    /// </summary>
    public class SkinInheritanceManager
    {
        private readonly Dictionary<string, InheritableSkin> _inheritableSkins = new();
        private readonly Dictionary<string, Skin> _resolvedCache = new();
        private readonly ISkinManager _skinManager;

        /// <summary>
        /// Initializes a new instance of the SkinInheritanceManager class.
        /// </summary>
        /// <param name="skinManager">The skin manager to use for resolving base themes.</param>
        /// <exception cref="ArgumentNullException">Thrown when skinManager is null.</exception>
        public SkinInheritanceManager(ISkinManager skinManager)
        {
            _skinManager = skinManager ?? throw new ArgumentNullException(nameof(skinManager));
        }

        /// <summary>
        /// Registers an inheritable theme.
        /// </summary>
        public void RegisterInheritableSkin(string name, InheritableSkin theme)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Skin name cannot be null or empty", nameof(name));
            if (theme == null)
                throw new ArgumentNullException(nameof(theme));

            theme.Name = name;
            _inheritableSkins[name] = theme;
            _resolvedCache.Remove(name); // Clear cache
        }

        /// <summary>
        /// Gets a resolved theme with inheritance applied.
        /// </summary>
        public Skin? GetResolvedTheme(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (_resolvedCache.TryGetValue(name, out var cached))
            {
                return cached;
            }

            if (!_inheritableSkins.TryGetValue(name, out var inheritableSkin))
            {
                return null;
            }

            var baseTheme = GetBaseTheme(inheritableSkin);
            var resolved = inheritableSkin.CreateResolvedSkin(baseTheme);

            _resolvedCache[name] = resolved;
            return resolved;
        }

        private Skin? GetBaseTheme(InheritableSkin skin)
        {
            if (string.IsNullOrEmpty(skin.BaseSkinName))
            {
                return null;
            }

            // Handle recursive inheritance
            if (_inheritableSkins.TryGetValue(skin.BaseSkinName, out var baseInheritable))
            {
                return GetResolvedTheme(skin.BaseSkinName);
            }

            // Fall back to skin manager (now uses injected dependency)
            return _skinManager.GetSkin(skin.BaseSkinName);
        }

        /// <summary>
        /// Creates a theme variant by overriding specific properties.
        /// </summary>
        public InheritableSkin CreateVariant(string baseName, string variantName, Dictionary<string, object> overrides)
        {
            if (string.IsNullOrEmpty(baseName))
                throw new ArgumentException("Base theme name cannot be null or empty", nameof(baseName));
            if (string.IsNullOrEmpty(variantName))
                throw new ArgumentException("Variant theme name cannot be null or empty", nameof(variantName));
            if (overrides == null)
                throw new ArgumentNullException(nameof(overrides));

            var variant = new InheritableSkin
            {
                Name = variantName,
                BaseSkinName = baseName,
                PropertyOverrides = overrides
            };

            RegisterInheritableSkin(variantName, variant);
            return variant;
        }

        /// <summary>
        /// Clears the resolved theme cache.
        /// </summary>
        public void ClearCache()
        {
            _resolvedCache.Clear();
        }
    }
}