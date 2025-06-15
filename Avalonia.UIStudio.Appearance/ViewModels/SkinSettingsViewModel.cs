using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.ViewModels;

public class SkinSettingsViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private readonly ISkinImportExportService _skinImportExportService;
    private readonly ISkinManager _skinManager;
    private readonly SkinValidator _skinValidator = new();

    private EditableSkinViewModel? _editableSkin;
    private bool _isApplyingSkin;

    private bool _isEditMode;
    private bool _isSubscribedToSkinChanged;
    private SkinSummaryInfo? _selectedSkin;
    private Color _validatedPrimaryColor;
    private SkinValidationResult? _validationResult;

    //public SkinSettingsViewModel() : this(
    //    Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance, SkinManager.Instance)
    //{
    //}

    public SkinSettingsViewModel(ILogger<SkinSettingsViewModel> logger, ISkinManager skinManager,
        ISkinImportExportService skinImportExportService)
    {
        _logger = logger;
        _skinManager = skinManager;
        _skinImportExportService = skinImportExportService;
        AvailableSkins = new ObservableCollection<SkinSummaryInfo>();
        ApplySkinCommand = ReactiveCommand.Create(ApplySkin);
        ApplyChangesCommand = ReactiveCommand.Create(SaveChanges);
        _skinManager.SkinChanged += _skinManager_SkinChanged;
        _isSubscribedToSkinChanged = true;
        LoadAvailableSkins();
        LoadCurrentSkin();
    }

    public ObservableCollection<SkinSummaryInfo> AvailableSkins { get; }

    public ReactiveCommand<Unit, Unit> ApplySkinCommand { get; }
    public ReactiveCommand<Unit, Unit> ApplyChangesCommand { get; }

    public SkinSummaryInfo? SelectedSkin
    {
        get => _selectedSkin;
        set
        {
            if (this.RaiseAndSetIfChanged(ref _selectedSkin, value) != null)
                if (value != null && !_isApplyingSkin)
                {
                    _isApplyingSkin = true;

                    _skinManager.ApplySkin(value.Name);
                    _logger.LogInformation("Skin changed to: {ThemeName}", value.Name);

                    LoadEditableSkin();

                    _isApplyingSkin = false;
                }
        }
    }

    public EditableSkinViewModel? EditableSkin
    {
        get => _editableSkin;
        private set
        {
            if (this.RaiseAndSetIfChanged(ref _editableSkin, value) != null)
            {
                AttachPropertyChangedHandler();
                ValidateSkin();
            }
        }
    }

    // Renamed and fixed: ValidatedProperties is a stable dictionary
    public Dictionary<string, ValidatedProperty> ValidatedProperties { get; } = new();

    // Added: indexer so you can use {Binding [FontSizeLarge]} in XAML
    public ValidatedProperty? this[string propertyName]
    {
        get
        {
            ValidatedProperties.TryGetValue(propertyName, out var vp);
            return vp;
        }
    }

    public SkinValidationResult? ValidationResult
    {
        get => _validationResult;
        private set
        {
            this.RaiseAndSetIfChanged(ref _validationResult, value);

            // Clear and repopulate ValidatedProperties dictionary
            ValidatedProperties.Clear();

            if (ValidationResult != null)
                foreach (var result in ValidationResult.ValidationMessages)
                foreach (var prop in result.InvolvedProperties)
                {
                    var add = new ValidatedProperty
                    {
                        OriginalMessage = result,
                        Message = result.Message,
                        IsValid = !result.IsError,
                        Name = prop
                    };

                    if (result.SuggestedValues.TryGetValue(prop, out var suggestedValue))
                        add.SuggestedValue = suggestedValue;

                    ValidatedProperties[prop] = add;
                }

            // Notify UI that dictionary contents changed
            this.RaisePropertyChanged(nameof(ValidatedProperties));
            this.RaisePropertyChanged(nameof(Errors));
            this.RaisePropertyChanged(nameof(Warnings));
        }
    }

    public List<string> Errors => ValidationResult?.Errors ?? new List<string>();
    public List<string> Warnings => ValidationResult?.Warnings ?? new List<string>();

    public bool IsEditMode
    {
        get => _isEditMode;
        set => this.RaiseAndSetIfChanged(ref _isEditMode, value);
    }

    private void _skinManager_SkinChanged(object? sender, EventArgs e)
    {
        if (_isApplyingSkin)
            // Skip handling SkinChanged caused by our own ApplySkin
            return;

        _logger.LogInformation("SkinChanged detected. Reloading EditableSkin.");
        LoadEditableSkin();
        ValidateSkin();
    }

    public void ResetToDefault()
    {
        try
        {
            var defaultSkin = AvailableSkins.FirstOrDefault(t => t.Name == "Dark");
            if (defaultSkin != null)
            {
                SelectedSkin = defaultSkin;
                _logger.LogInformation("Reset to default skin: Dark");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reset to default skin");
        }
    }

    private void LoadAvailableSkins()
    {
        try
        {
            var skinNames = _skinManager?.GetAvailableSkinNames();

            AvailableSkins.Clear();

            if (skinNames != null)
                foreach (var skinName in skinNames)
                {
                    var skin = _skinManager?.GetSkin(skinName);
                    if (skin != null)
                    {
                        var skinInfo = new SkinSummaryInfo
                        {
                            Name = skinName,
                            Description = GetSkinDescription(skinName),
                            PreviewColor = new SolidColorBrush(skin.AccentColor)
                        };
                        AvailableSkins.Add(skinInfo);
                    }
                }

            _logger.LogInformation("Loaded {ThemeCount} available skins", AvailableSkins.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load available skins");
        }
    }

    private void LoadCurrentSkin()
    {
        try
        {
            var currentSkin = _skinManager.CurrentSkin;
            if (currentSkin?.Name != null)
                SelectedSkin = AvailableSkins.FirstOrDefault(t => t.Name == currentSkin.Name);

            SelectedSkin ??= AvailableSkins.FirstOrDefault(t => t.Name == "Dark");

            LoadEditableSkin();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load current skin");
        }
    }

    private void LoadEditableSkin()
    {
        var currentSkin = _skinManager.CurrentSkin;
        if (currentSkin != null) EditableSkin = new EditableSkinViewModel(CloneSkin(currentSkin));
    }

    private void ApplySkin()
    {
        try
        {
            if (SelectedSkin != null && !_isApplyingSkin)
            {
                _isApplyingSkin = true;

                _skinManager.ApplySkin(SelectedSkin.Name);
                SkinManager.Instance.ApplySkin(SelectedSkin.Name);
                _logger.LogInformation("Applied skin: {ThemeName}", SelectedSkin.Name);

                LoadEditableSkin();

                _isApplyingSkin = false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to apply skin: {ThemeName}", SelectedSkin?.Name);
            _isApplyingSkin = false; // Ensure flag is reset even on exception
        }
    }

    private async void SaveChanges()
    {
        try
        {
            if (EditableSkin?.Skin != null)
            {
                // 1️⃣ First: push ValidatableProperty values back to Skin
                EditableSkin.MapBackToSkin();
                var userOverridePath = $"./Skins/UserOverrides/{EditableSkin.Skin.Name}/skin.json";
                Directory.CreateDirectory(Path.GetDirectoryName(userOverridePath)!);

                await _skinImportExportService.ExportSkinAsync(EditableSkin.Skin, userOverridePath);

                _logger.LogInformation("Saved edited skin to: {FilePath}", userOverridePath);

                // 3️⃣ Reload available skins so SkinManager knows the updated version
                _skinManager.ReloadSkins();

                // 4️⃣ Re-apply the saved skin from SkinManager
                _skinManager.ApplySkin(EditableSkin.Skin.Name);

                _logger.LogInformation("Reloaded and re-applied saved skin: {SkinName}", EditableSkin.Skin.Name);

                // 5️⃣ Reload EditableSkin with the fresh saved version
                LoadEditableSkin();
                var currentSkin = _skinManager.CurrentSkin;
                if (currentSkin?.Name != null)
                    SelectedSkin = AvailableSkins.FirstOrDefault(t => t.Name == currentSkin.Name);
                ValidateSkin();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save and apply edited skin");
        }
    }


    private void ValidateSkin()
    {
        if (EditableSkin?.Skin == null) return;

        var result = _skinValidator.ValidateSkin(EditableSkin.Skin);
        ValidationResult = result;

        _logger.LogInformation("Validated EditableSkin: {ErrorCount} errors, {WarningCount} warnings",
            result.Errors.Count, result.Warnings.Count);
    }

    private void AttachPropertyChangedHandler()
    {
        if (EditableSkin != null)
        {
            EditableSkin.PropertyChanged -= EditableSkin_PropertyChanged;
            EditableSkin.PropertyChanged += EditableSkin_PropertyChanged;
        }
    }

    private void EditableSkin_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ValidateSkin();
    }

    private Skin CloneSkin(Skin original)
    {
        return new Skin
        {
            Name = original.Name,
            Description = original.Description,
            PrimaryColor = original.PrimaryColor,
            SecondaryColor = original.SecondaryColor,
            AccentColor = original.AccentColor,
            PrimaryBackground = original.PrimaryBackground,
            SecondaryBackground = original.SecondaryBackground,
            PrimaryTextColor = original.PrimaryTextColor,
            SecondaryTextColor = original.SecondaryTextColor,
            FontFamily = original.FontFamily,
            FontSizeSmall = original.FontSizeSmall,
            FontSizeMedium = original.FontSizeMedium,
            FontSizeLarge = original.FontSizeLarge,
            FontWeight = original.FontWeight,
            BorderColor = original.BorderColor,
            BorderThickness = original.BorderThickness,
            BorderRadius = original.BorderRadius,
            ErrorColor = original.ErrorColor,
            WarningColor = original.WarningColor,
            SuccessColor = original.SuccessColor,
            Typography = original.Typography?.Clone(),
            ControlThemeUris = original.ControlThemeUris,
            StyleUris = original.StyleUris,
            AssetUris = original.AssetUris,
            HeaderFontFamily = original.HeaderFontFamily,
            BodyFontFamily = original.BodyFontFamily,
            MonospaceFontFamily = original.MonospaceFontFamily,
            LineHeight = original.LineHeight,
            LetterSpacing = original.LetterSpacing,
            EnableLigatures = original.EnableLigatures
        };
    }

    private static string GetSkinDescription(string skinName)
    {
        return skinName switch
        {
            "Dark" => "Professional dark skin with blue accents. Easy on the eyes for extended use.",
            "Light" => "Clean light skin with dark text. Perfect for bright environments.",
            "Ocean Blue" => "Deep blue skin inspired by ocean depths. Calming and focused.",
            "Forest Green" => "Nature-inspired green skin. Relaxing and earthy.",
            "Purple Haze" => "Rich purple skin with mystical vibes. Creative and bold.",
            "High Contrast" => "Maximum contrast for accessibility. Clear and distinct colors.",
            "Cyberpunk" => "Futuristic neon skin with hot pink accents. Edgy and modern.",
            _ => "Custom skin with unique color combinations."
        };
    }
}