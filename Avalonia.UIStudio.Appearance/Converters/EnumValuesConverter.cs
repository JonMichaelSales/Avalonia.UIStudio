using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Avalonia.UIStudio.Appearance.Converters;

public class EnumValuesConverter : IValueConverter
{
    public static readonly EnumValuesConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Enum enumVal ? Enum.GetValues(enumVal.GetType()) : Array.Empty<Enum>();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
