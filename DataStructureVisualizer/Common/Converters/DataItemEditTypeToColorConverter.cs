using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Theme;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.Converters
{
    class DataItemEditTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((DataItemEditType)value)
            {
                case DataItemEditType.None:
                    return ThemeHelper.GetTheme().PrimaryMid.Color;
                case DataItemEditType.New:
                    return new Color() { ScA = 1.0F, R = 255, G = 152, B = 0 };
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
