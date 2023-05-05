using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Theme;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.Converters
{
    internal class DataItemStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((DataItemState)value)
            {
                default:
                case DataItemState.Normal:
                    return ThemeHelper.GetTheme().PrimaryMid.Color;
                case DataItemState.Visited:
                    return ThemeHelper.NewColor;
                case DataItemState.Matched:
                    return ThemeHelper.CorrectColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
