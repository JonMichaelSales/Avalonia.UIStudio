using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Avalonia.Accelerate.Appearance.Converters
{
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
}