using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Avalonia.UIStudio.Appearance.Converters
{
    public class ObjectToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Color c)
                return c;

            if (value is string s && Color.TryParse(s, out var parsed))
                return parsed;

            return Colors.Transparent; // fallback if value is invalid
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}