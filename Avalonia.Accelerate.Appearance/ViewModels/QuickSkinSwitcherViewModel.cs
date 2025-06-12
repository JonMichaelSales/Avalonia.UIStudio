using System.Collections.ObjectModel;
using Avalonia.Accelerate.Appearance.Interfaces;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Accelerate.Appearance.Services;
using Avalonia.Media;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Avalonia.Accelerate.Appearance.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class QuickSkinSwitcherViewModel : ViewModelBase, IQuickSkinSwitcherViewModel
    {
        private readonly ILogger _logger;
        private readonly ISkinManager _skinManager;
        private SkinSummaryInfo? _selectedSkin;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickSkinSwitcherViewModel"/> class
        /// with a default logger instance.
        /// </summary>
        public QuickSkinSwitcherViewModel(ISkinManager skinManager) : this(
            Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance, skinManager)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickSkinSwitcherViewModel"/> class
        /// with the specified logger instance.
        /// </summary>
        /// <param name="logger">
        /// An instance of <see cref="ILogger"/> used for logging operations within the view model.
        /// </param>
        /// <param name="skinManager"></param>
        public QuickSkinSwitcherViewModel(ILogger logger, ISkinManager skinManager)
        {
            _logger = logger;
            _skinManager = skinManager;
            AvailableSkins = new ObservableCollection<SkinSummaryInfo>();

            LoadAvailableSkins();
            LoadCurrentSkin();

            // Subscribe to skin manager changes to keep in sync
            skinManager.SkinChanged += OnSkinChanged;
        }

        /// <summary>
        /// Gets the collection of available skins that can be selected and applied
        /// within the application.
        /// </summary>
        /// <remarks>
        /// This property is populated by the <see cref="LoadAvailableSkins"/> method,
        /// which retrieves the skins from the <see cref="SkinManager"/>. The collection
        /// is updated dynamically to reflect the available skins.
        /// </remarks>
        public ObservableCollection<SkinSummaryInfo> AvailableSkins { get; }

        /// <summary>
        /// Gets or sets the currently selected skin.
        /// </summary>
        /// <remarks>
        /// When a new skin is selected, the corresponding skin is applied automatically.
        /// The selected skin is synchronized with the <see cref="AvailableSkins"/> collection.
        /// </remarks>
        public SkinSummaryInfo? SelectedSkin
        {
            get => _selectedSkin;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _selectedSkin, value) != null)
                {
                    ApplySkin(value);
                }
            }
        }

        private void LoadAvailableSkins()
        {
            try
            {
                
                var skinNames = _skinManager.GetAvailableSkinNames();

                AvailableSkins.Clear();

                foreach (var skinName in skinNames)
                {
                    var skin = _skinManager.GetSkin(skinName);
                    if (skin != null)
                    {
                        var skinSummaryInfo = new SkinSummaryInfo
                        {
                            Name = skinName,
                            Description = GetSkinDescription(skinName),
                            PreviewColor = new SolidColorBrush(skin.AccentColor)
                        };
                        AvailableSkins.Add(skinSummaryInfo);
                    }
                }

                _logger.LogDebug("Loaded {ThemeCount} themes for quick switcher", AvailableSkins.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load skins for quick switcher");
            }
        }

        private void LoadCurrentSkin()
        {
            try
            {
                var currentSkin = _skinManager.CurrentSkin;
                if (currentSkin?.Name != null)
                {
                    var currentSkinItem = AvailableSkins.FirstOrDefault(t => t.Name == currentSkin.Name);
                    if (currentSkinItem != null)
                    {
                        // Set without triggering the setter to avoid recursive application
                        _selectedSkin = currentSkinItem;
                        this.RaisePropertyChanged(nameof(SelectedSkin));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load current skin for quick switcher");
            }
        }

        private void ApplySkin(SkinSummaryInfo? skinInfo)
        {
            try
            {
                if (skinInfo != null)
                {
                    _skinManager.ApplySkin(skinInfo.Name);
                    _logger.LogInformation("Quick skin switch to: {ThemeName}", skinInfo.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to apply skin via quick switcher: {ThemeName}", skinInfo?.Name);
            }
        }

        private void OnSkinChanged(object? sender, EventArgs e)
        {
            // Update selected skin when skin changes externally
            try
            {
                LoadCurrentSkin();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update quick switcher after skin change");
            }
        }

        private static string GetSkinDescription(string skinName)
        {
            return skinName switch
            {
                "Dark" => "Professional dark skin",
                "Light" => "Clean light skin",
                "Ocean Blue" => "Deep blue ocean skin",
                "Forest Green" => "Nature-inspired green",
                "Purple Haze" => "Rich purple skin",
                "High Contrast" => "Maximum contrast",
                "Cyberpunk" => "Futuristic neon skin",
                _ => "Custom skin"
            };
        }

        /// <summary>
        /// Releases the resources used by the <see cref="QuickSkinSwitcherViewModel"/> class.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether the method is being called explicitly to release managed resources.
        /// If <c>true</c>, managed resources are released; otherwise, only unmanaged resources are released.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _skinManager.SkinChanged -= OnSkinChanged;
            }
            base.Dispose(disposing);
        }
    }
}