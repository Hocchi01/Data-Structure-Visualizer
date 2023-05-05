using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Theme;
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
            if ((string)parameter == "Graph")
            {
                switch ((DataItemState)value)
                {
                    default:
                    case DataItemState.Normal:
                        return new SolidColorBrush(ThemeHelper.GetTheme().PrimaryMid.Color);
                    case DataItemState.Visited:
                        return new SolidColorBrush(ThemeHelper.NewColor);
                    case DataItemState.Matched:
                        return new SolidColorBrush(ThemeHelper.CorrectColor);
                }
            }
            else
            {
                switch ((DataItemState)value)
                {
                    case DataItemState.Normal:
                        return new SolidColorBrush(new Color() { ScA = 0.0F, R = 0, G = 0, B = 0 });
                    case DataItemState.Sorted:
                        return new SolidColorBrush(new Color() { ScA = 1.0F, R = 76, G = 175, B = 80 });
                    case DataItemState.Min:
                    case DataItemState.Max:
                    case DataItemState.Group2:
                        return new SolidColorBrush(new Color() { ScA = 1.0F, R = 41, G = 98, B = 255 });
                    case DataItemState.Actived:
                        return new SolidColorBrush(new Color() { ScA = 1.0F, R = 174, G = 234, B = 0 });
                    case DataItemState.Group1:
                        return new SolidColorBrush(new Color() { ScA = 1.0F, R = 244, G = 67, B = 54 });
                    //case DataItemState.Visited:
                    //    return new SolidColorBrush(new Color() { ScA = 1.0F, R = 255, G = 152, B = 0 });
                    default:
                        return null;
                }
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
