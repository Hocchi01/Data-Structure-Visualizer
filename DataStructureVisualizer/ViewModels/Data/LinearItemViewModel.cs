using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataStructureVisualizer.ViewModels.Data
{
    internal partial class LinearItemViewModel : DataItemViewModelBase
    {
        /// <summary>
        /// 存储上一个颜色，用于动画时颜色恢复
        /// </summary>
        public SolidColorBrush? OldColor { set; get; } = null;
        /// <summary>
        /// 数据背景色
        /// </summary>
        /// <remarks>
        /// 根据数据在集合中大小控制颜色，数值越大透明度越低
        /// </remarks>
        [ObservableProperty]
        private SolidColorBrush? color = null;

        partial void OnColorChanging(SolidColorBrush value)
        {
            OldColor = Color ?? value;
        }

        public void RecoverColor()
        {
            Color = OldColor;
        }

    }
}
