using DataStructureVisualizer.Common.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.Converters
{
    class DataItemStateToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((DataItemState)value)
            {
                case DataItemState.Normal:
                    return new SolidColorBrush(new Color() { ScA = 1.0F, R = 238, G = 238, B = 238 });
                case DataItemState.Sorted:
                    return new SolidColorBrush(new Color() { ScA = 1.0F, R = 76, G = 175, B = 80 });
                case DataItemState.Min:
                case DataItemState.Max:
                    return new SolidColorBrush(new Color() { ScA = 1.0F, R = 41, G = 98, B = 255 });
                case DataItemState.Actived:
                    return new SolidColorBrush(new Color() { ScA = 1.0F, R = 174, G = 234, B = 0 });
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
