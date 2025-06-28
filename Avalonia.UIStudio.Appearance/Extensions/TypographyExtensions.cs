using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Extensions;

public static class TypographyExtensions
{
    public static SerializableTypography? ToSerializable(this TypographyScale? scale, Skin skin)
    {
        if (scale == null)
            return null;

        return new SerializableTypography
        {
            DisplayLarge = scale.DisplayLarge,
            DisplayMedium = scale.DisplayMedium,
            DisplaySmall = scale.DisplaySmall,
            HeadlineLarge = scale.HeadlineLarge,
            HeadlineMedium = scale.HeadlineMedium,
            HeadlineSmall = scale.HeadlineSmall,
            TitleLarge = scale.TitleLarge,
            TitleMedium = scale.TitleMedium,
            TitleSmall = scale.TitleSmall,
            LabelLarge = scale.LabelLarge,
            LabelMedium = scale.LabelMedium,
            LabelSmall = scale.LabelSmall,
            BodyLarge = scale.BodyLarge,
            BodyMedium = scale.BodyMedium,
            BodySmall = scale.BodySmall,
        };
    }

    public static TypographyScale ToTypographyScale(this SerializableTypography serial)
    {
        return new TypographyScale
        {
            DisplayLarge = serial.DisplayLarge,
            DisplayMedium = serial.DisplayMedium,
            DisplaySmall = serial.DisplaySmall,
            HeadlineLarge = serial.HeadlineLarge,
            HeadlineMedium = serial.HeadlineMedium,
            HeadlineSmall = serial.HeadlineSmall,
            TitleLarge = serial.TitleLarge,
            TitleMedium = serial.TitleMedium,
            TitleSmall = serial.TitleSmall,
            LabelLarge = serial.LabelLarge,
            LabelMedium = serial.LabelMedium,
            LabelSmall = serial.LabelSmall,
            BodyLarge = serial.BodyLarge,
            BodyMedium = serial.BodyMedium,
            BodySmall = serial.BodySmall
        };
    }
}