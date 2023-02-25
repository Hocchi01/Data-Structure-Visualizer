using DataStructureVisualizer.Common.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DataStructureVisualizer.Common.Converters
{
    /// <summary>
    /// 链表类型枚举值转布尔值
    /// </summary>
    /// <remarks>
    /// 用于判断是否是特殊节点（头结点、尾结点）
    /// </remarks>
    class LinkedListItemTypeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (LinkedListItemType)value != LinkedListItemType.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
