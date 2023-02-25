using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataStructureVisualizer.Common.AnimationLib
{
    internal static class AnimationHelper
    {
        /// <summary>
        /// 获取当前数据结构视图的虚拟视图容器
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="canvas"></param>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        public static StackPanel GetVirtualLinearContainer<TView>(Panel canvas, ObservableCollection<DataItemViewModelBase> dataItems)
            where TView : UserControl, new()
        {
            StackPanel container = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(8),
                VerticalAlignment = VerticalAlignment.Center
            };

            foreach (var item in dataItems)
            {
                TView dataView = new TView() { DataContext = item };
                container.Children.Add( dataView );
            }

            canvas.Children.Add(container);
            canvas.Children[0].Visibility = Visibility.Hidden;
            
            return container;
        }
    }
}
