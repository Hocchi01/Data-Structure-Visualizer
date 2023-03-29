using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.Sorts
{
    internal abstract class SortBase
    {
        public ObservableCollection<int> Values { get; set; }
        public Grid Canvas { get; set; }
        public ItemsControl Container { get; set; }
        public MyStoryboard MainStoryboard { get; set; }
        public ObservableCollection<ArrayItemViewModel> DataItems { get; set; }

        public SortBase(ObservableCollection<int> values, Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<ArrayItemViewModel> dataItems)
        {
            Values = values;
            Canvas = canvas;
            Container = container;
            MainStoryboard = myStoryboard;
            DataItems = dataItems;
        }

        public abstract void MainProgram();

        /// <summary>
        /// 将元素条形化
        /// </summary>
        protected void Stick(ItemsControl container)
        {
            float[] scaleRatios = Comm.GetGradient(Values, 2.0f, 5.0f);

            List<AnimationTimeline> animations = new List<AnimationTimeline>();
            List<UIElement> targetControls = new List<UIElement>();
            List<object> targetParams = new List<object>();

            for (int i = 0; i < Values.Count; i++)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(container, i);
                animations.Add(new SimulatedDoubleAnimation(to: scaleRatios[i], time: 500));
                targetControls.Add(itemView.rect);
                targetParams.Add("(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
            }

            MainStoryboard.AddAsyncAnimations(animations, targetControls, targetParams);
        }

        /// <summary>
        /// 将条形化的元素缩回原状态
        /// </summary>
        protected void UnStick(ItemsControl container)
        {
            List<AnimationTimeline> animations = new List<AnimationTimeline>();
            List<UIElement> targetControls = new List<UIElement>();
            List<object> targetParams = new List<object>();

            for (int i = 0; i < Values.Count; i++)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(container, i);
                animations.Add(new SimulatedDoubleAnimation(to: 1.0f, time: 500));
                targetControls.Add(itemView.rect);
                targetParams.Add("(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
            }

            MainStoryboard.AddAsyncAnimations(animations, targetControls, targetParams);
        }
    }
}
