using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views.Data;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace DataStructureVisualizer.Common.Sorts
{
    internal abstract class SortBase
    {
        private int[] vals;

        protected int[] table; // 用于记录当前元素实际位置
        protected int last;

        public Grid Canvas { get; set; }
        public ItemsControl Container { get; set; }
        public MyStoryboard MainStoryboard { get; set; }
        public ObservableCollection<ArrayItemViewModel> DataItems { get; set; }

        public SortBase(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<ArrayItemViewModel> dataItems)
        {
            Canvas = canvas;
            Container = container;
            MainStoryboard = myStoryboard;
            DataItems = dataItems;

            vals = new int[DataItems.Count];
            table = new int[DataItems.Count];
            for (int i = 0; i < DataItems.Count; i++)
            {
                vals[i] = DataItems[i].Value ?? 0;
                table[i] = i;
            }

            last = DataItems.Count - 1;
        }

        public abstract void MainProgram();

        /// <summary>
        /// 将元素条形化
        /// </summary>
        protected void Stick(Action? after = null)
        {
            float[] scaleRatios = Comm.GetGradient(vals, 2.0f, 5.0f);

            List<AnimationTimeline> animations = new List<AnimationTimeline>();
            List<UIElement> targetControls = new List<UIElement>();
            List<object> targetParams = new List<object>();

            for (int i = 0; i <= last; i++)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, i);
                animations.Add(new SimulatedDoubleAnimation(to: scaleRatios[i], time: 500));
                targetControls.Add(itemView.rect);
                targetParams.Add("(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
            }

            MainStoryboard.AddAsyncAnimations(animations, targetControls, targetParams, null, after);
        }

        /// <summary>
        /// 将条形化的元素缩回原状态
        /// </summary>
        protected void UnStick(Action? before = null)
        {
            List<AnimationTimeline> animations = new List<AnimationTimeline>();
            List<UIElement> targetControls = new List<UIElement>();
            List<object> targetParams = new List<object>();

            for (int i = 0; i <= last; i++)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, i);
                animations.Add(new SimulatedDoubleAnimation(to: 1.0f, time: 500));
                targetControls.Add(itemView.rect);
                targetParams.Add("(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
            }

            MainStoryboard.AddAsyncAnimations(animations, targetControls, targetParams, before);
        }

        /// <summary>
        /// 交换两个元素
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="after"></param>
        protected void Swap(int i, int j, Action? after = null)
        {
            MainStoryboard.AddAsyncAnimations
            (
                new List<AnimationTimeline>
                {
                    new SimulatedDoubleAnimation(by: (j - i) * 52, time: 500),
                    new SimulatedDoubleAnimation(by: (i - j) * 52, time: 500),
                },
                new List<UIElement>
                {
                    Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, table[i]).valueItem,
                    Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, table[j]).valueItem
                },
                new List<object>
                {
                    "(UIElement.RenderTransform).(TranslateTransform.X)",
                    "(UIElement.RenderTransform).(TranslateTransform.X)"
                },
                null,
                after
            );

            int tmp = table[i];
            table[i] = table[j];
            table[j] = tmp;
        }

        /// <summary>
        /// 排序动画执行完毕后更新源数据
        /// </summary>
        protected void UpdateValues()
        {
            int[] values = new int[DataItems.Count];
            for (int i = 0; i < DataItems.Count; i++)
            {
                values[i] = DataItems[table[i]].Value ?? 0;
            }
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int[]>(values));
        }
    }
}
