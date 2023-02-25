using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataStructureVisualizer.Common.Converters
{
    /// <summary>
    /// 动画状态枚举值转为布尔值
    /// </summary>
    /// <remarks>
    /// 动画正在运行（Running）则为真，否则为假
    /// </remarks>
    internal class AnimationStateToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return false;
            }
            switch ((string)parameter)
            {
                case "IsRunning": return (AnimationState)value == AnimationState.Running;
                case "IsBegin": return (AnimationState)value != AnimationState.Stopped;
                default: return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
