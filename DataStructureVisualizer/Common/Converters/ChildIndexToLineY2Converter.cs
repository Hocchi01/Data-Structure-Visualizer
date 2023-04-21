using System;
using System.Globalization;
using System.Windows.Data;

namespace DataStructureVisualizer.Common.Converters
{
    internal class ChildIndexToLineY2Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int?)value == null ? 25 : double.Parse((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
