using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Avalonia.UIStudio.Appearance.Converters
{
    public class BoolToEditModeContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool isEdit && isEdit) ? "Editing Skin" : "View Skin Info";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}