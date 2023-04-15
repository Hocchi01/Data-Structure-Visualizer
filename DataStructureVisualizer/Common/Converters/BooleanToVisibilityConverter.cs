﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataStructureVisualizer.Common.Converters
{
    internal class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)parameter == "reverse")
            {
                return (bool)value ? Visibility.Hidden : Visibility.Visible;
            }
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
