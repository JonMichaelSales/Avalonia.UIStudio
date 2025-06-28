using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Avalonia.UIStudio.Appearance.Converters;

public class FontFamilyEqualityConverter : IValueConverter
{
    public static readonly FontFamilyEqualityConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is FontFamily selected)
        {
            return new FontFamily(selected.Name); // flatten to string-based value
        }

        return AvaloniaProperty.UnsetValue;
    }
}