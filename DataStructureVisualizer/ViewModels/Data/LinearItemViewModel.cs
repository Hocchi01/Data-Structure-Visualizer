using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataStructureVisualizer.ViewModels.Data
{
    internal class LinearItemViewModel : DataItemViewModelBase
    {
        /// <summary>
        /// 数据背景色
        /// </summary>
        /// <remarks>
        /// 根据数据在集合中大小控制颜色，数值越大透明度越低
        /// </remarks>
        public SolidColorBrush? Color { get; set; } = null;

    }
}
