using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.UIStudio.Appearance.Model;
using Avalonia.UIStudio.Appearance.Services;
using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.ViewModels;

public class SkinEditorViewModel : ReactiveObject
{
    private Skin? _selectedSkinDetails;
    public Skin? SelectedSkinDetails
    {
        get => _selectedSkinDetails;
        set => this.RaiseAndSetIfChanged(ref _selectedSkinDetails, value);
    }

    public ObservableCollection<string> Errors { get; } = new();
    public ObservableCollection<string> Warnings { get; } = new();

    public ReactiveCommand<Unit, Unit> ApplyChangesCommand { get; }

    private readonly SkinValidator _validator = new();

    public SkinEditorViewModel()
    {
        ApplyChangesCommand = ReactiveCommand.Create(ApplyChanges);
    }

    public void LoadSkin(Skin skin)
    {
        SelectedSkinDetails = skin;
        Errors.Clear();
        Warnings.Clear();
    }

    public void LoadSkinWithValidation(Skin skin, SkinValidationResult validation)
    {
        SelectedSkinDetails = skin;
        Errors.Clear();
        Warnings.Clear();

        foreach (var msg in validation.ValidationMessages)
        {
            if (msg.IsError)
                Errors.Add(msg.Message);
            else
                Warnings.Add(msg.Message);
        }
    }


    private void ApplyChanges()
    {
        Errors.Clear();
        Warnings.Clear();

        if (SelectedSkinDetails is null)
        {
            Errors.Add("No skin is currently loaded.");
            return;
        }

        var result = _validator.ValidateSkin(SelectedSkinDetails);

        foreach (var msg in result.ValidationMessages)
        {
            if (msg.IsError)
                Errors.Add(msg.Message);
            else
                Warnings.Add(msg.Message);
        }

        if (result.IsValid)
        {
            // If you'd like to do something here like notify save completed, you can add a callback or event.
        }
    }
}