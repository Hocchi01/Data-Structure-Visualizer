using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataStructureVisualizer.Common.Converters
{
    internal class BooleanToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return new GridLength(1, GridUnitType.Star);
            }
            return GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
