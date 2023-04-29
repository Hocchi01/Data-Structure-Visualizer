using CommunityToolkit.Mvvm.ComponentModel;
using DataStructureVisualizer.Common.Enums;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataStructureVisualizer.ViewModels.Data
{
    internal partial class DataItemViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private int? value = null;

        public int? Index { get; set; } = null;

        [ObservableProperty]
        private DataItemEditType editType = DataItemEditType.None;

        [ObservableProperty]
        private DataItemState state = DataItemState.Normal;

        [ObservableProperty]
        private bool isRemoved = false;

        /// <summary>
        /// 根据当前数据值梯度计算出的原始颜色
        /// </summary>
        public SolidColorBrush? OriginalColor { set; get; } = null;

        /// <summary>
        /// 数据背景色
        /// </summary>
        /// <remarks>
        /// 根据数据在集合中大小控制颜色，数值越大透明度越低
        /// </remarks>
        [ObservableProperty]
        private SolidColorBrush? color = null;

        public void RecoverColor()
        {
            Color = OriginalColor;
        }

        public void Clear()
        {
            Value = null;
            Color = null;
        }

        public void Activate()
        {
            Color = new SolidColorBrush(new PaletteHelper().GetTheme().SecondaryMid.Color);
        }

        public void Deactivate()
        {
            Color = OriginalColor;
        }
    }
}
