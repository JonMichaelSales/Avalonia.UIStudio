using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Avalonia.UIStudio.Appearance.Converters;

public class BoolToBorderBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isError)
            return isError ? Brushes.Red : Brushes.Pink;
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}