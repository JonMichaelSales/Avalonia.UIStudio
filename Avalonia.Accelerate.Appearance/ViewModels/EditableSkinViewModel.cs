using System.Reflection.Metadata;
using Avalonia.Accelerate.Appearance.Model;
using Avalonia.Media;
using ReactiveUI;

namespace Avalonia.Accelerate.Appearance.ViewModels;

public class EditableSkinViewModel : ReactiveObject
{
    public Skin Skin { get; set; }
    
    public EditableSkinViewModel(Skin skin)
    {
        Skin = skin;
        Name = new ValidatableProperty<string>(nameof(Name), skin.Name);
        Description = new ValidatableProperty<string>(nameof(Description), skin.Description);

        PrimaryColor = new ValidatableProperty<Color>(nameof(PrimaryColor), skin.PrimaryColor);
        SecondaryColor = new ValidatableProperty<Color>(nameof(SecondaryColor), skin.SecondaryColor);
        PrimaryBackground = new ValidatableProperty<Color>(nameof(PrimaryBackground), skin.PrimaryBackground);
        SecondaryBackground = new ValidatableProperty<Color>(nameof(SecondaryBackground), skin.SecondaryBackground);
            
        AccentColor = new ValidatableProperty<Color>(nameof(AccentColor), skin.AccentColor);
        PrimaryTextColor = new ValidatableProperty<Color>(nameof(PrimaryTextColor), skin.PrimaryTextColor);
        SecondaryTextColor = new ValidatableProperty<Color>(nameof(SecondaryTextColor), skin.SecondaryTextColor);

        FontFamily = new ValidatableProperty<string>(nameof(FontFamily), skin.FontFamily.Name);
        
        
        FontSizeSmall = new ValidatableProperty<double>(nameof(FontSizeSmall), skin.FontSizeSmall);
        FontSizeMedium = new ValidatableProperty<double>(nameof(FontSizeMedium), skin.FontSizeMedium);
        FontSizeLarge = new ValidatableProperty<double>(nameof(FontSizeLarge), skin.FontSizeLarge);

        BorderRadius = new ValidatableProperty<double>(nameof(BorderRadius), skin.BorderRadius);
        BorderThickness = new ValidatableProperty<Thickness>(nameof(BorderThickness), skin.BorderThickness);
        BorderColor = new ValidatableProperty<Color>(nameof(BorderColor), skin.BorderColor);

        SuccessColor = new ValidatableProperty<Color>(nameof(SuccessColor), skin.SuccessColor);
        WarningColor = new ValidatableProperty<Color>(nameof(WarningColor), skin.WarningColor);
        ErrorColor = new ValidatableProperty<Color>(nameof(ErrorColor), skin.ErrorColor);
    }

    /// <summary>
    /// Pushes edited ValidatableProperty values back into Skin model.
    /// Call this before saving or applying.
    /// </summary>
    public void MapBackToSkin()
    {
        Skin.Name = Name.Value;
        Skin.Description = Description.Value;

        Skin.PrimaryColor = PrimaryColor.Value;
        Skin.SecondaryColor = SecondaryColor.Value;
        Skin.PrimaryBackground = PrimaryBackground.Value;
        Skin.SecondaryBackground = SecondaryBackground.Value;
        Skin.AccentColor = AccentColor.Value;
        Skin.PrimaryTextColor = PrimaryTextColor.Value;
        Skin.SecondaryTextColor = SecondaryTextColor.Value;

        Skin.FontFamily = new FontFamily(FontFamily.Value);
        Skin.FontSizeSmall = FontSizeSmall.Value;
        Skin.FontSizeMedium = FontSizeMedium.Value;
        Skin.FontSizeLarge = FontSizeLarge.Value;

        Skin.BorderRadius = BorderRadius.Value;
        Skin.BorderThickness = BorderThickness.Value;
        Skin.BorderColor = BorderColor.Value;

        Skin.SuccessColor = SuccessColor.Value;
        Skin.WarningColor = WarningColor.Value;
        Skin.ErrorColor = ErrorColor.Value;
    }


    public ValidatableProperty<string> Name { get; set; }
    public ValidatableProperty<string> Description { get; set; }

    public ValidatableProperty<Color> PrimaryColor { get; set; }
    public ValidatableProperty<Color> SecondaryColor { get; set; }
    public ValidatableProperty<Color> PrimaryBackground { get; set; }
    public ValidatableProperty<Color> SecondaryBackground { get; set; }
    public ValidatableProperty<Color> AccentColor { get; set; }
    public ValidatableProperty<Color> PrimaryTextColor { get; set; }
    public ValidatableProperty<Color> SecondaryTextColor { get; set; }
    public ValidatableProperty<string> FontFamily { get; set; }
    public ValidatableProperty<string> HeaderFontFamily { get; set; }
    public ValidatableProperty<string> BodyFontFamily { get; set; }
    public ValidatableProperty<string> MonospaceFontFamily { get; set; }
    public ValidatableProperty<double> FontSizeSmall { get; set; }
    public ValidatableProperty<double> FontSizeMedium { get; set; }
    public ValidatableProperty<double> FontSizeLarge { get; set; }

    public ValidatableProperty<double> BorderRadius { get; set; }
    public ValidatableProperty<Thickness> BorderThickness { get; set; }

    public ValidatableProperty<Color> BorderColor { get; set; }

    public ValidatableProperty<Color> SuccessColor { get; set; }
    public ValidatableProperty<Color> WarningColor { get; set; }
    public ValidatableProperty<Color> ErrorColor { get; set; }
}
