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
        protected List<int> vals;
        protected int[] table; // 用于记录当前元素实际位置

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

            vals = Comm.ObservableCollectionToList(Values);
            table = new int[Values.Count];
            for (int i = 0; i < Values.Count; i++)
            {
                table[i] = i;
            }
        }

        public abstract void MainProgram();

        /// <summary>
        /// 将元素条形化
        /// </summary>
        protected void Stick()
        {
            float[] scaleRatios = Comm.GetGradient(Values, 2.0f, 5.0f);

            List<AnimationTimeline> animations = new List<AnimationTimeline>();
            List<UIElement> targetControls = new List<UIElement>();
            List<object> targetParams = new List<object>();

            for (int i = 0; i < Values.Count; i++)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, i);
                animations.Add(new SimulatedDoubleAnimation(to: scaleRatios[i], time: 500));
                targetControls.Add(itemView.rect);
                targetParams.Add("(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
            }

            MainStoryboard.AddAsyncAnimations(animations, targetControls, targetParams);
        }

        /// <summary>
        /// 将条形化的元素缩回原状态
        /// </summary>
        protected void UnStick(Action? before = null)
        {
            List<AnimationTimeline> animations = new List<AnimationTimeline>();
            List<UIElement> targetControls = new List<UIElement>();
            List<object> targetParams = new List<object>();

            for (int i = 0; i < Values.Count; i++)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, i);
                animations.Add(new SimulatedDoubleAnimation(to: 1.0f, time: 500));
                targetControls.Add(itemView.rect);
                targetParams.Add("(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
            }

            MainStoryboard.AddAsyncAnimations(animations, targetControls, targetParams, before);
        }

        //protected void Swap(int i, int j, Action? after = null)
        //{
        //    Action dataItemsSwap = () =>
        //    {
        //        if (i == j) return;

        //        var tmpI = new ArrayItemViewModel(DataItems[i]);
        //        var tmpJ = new ArrayItemViewModel(DataItems[j]);

        //        tmpI.Index = j;
        //        tmpJ.Index = i;

        //        DataItems[i] = tmpJ;
        //        Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, i).rect.RenderTransform = new ScaleTransform()
        //        {
        //            ScaleY = Comm.GetSingleGradientVal(tmpJ.Value ?? 0, Values, 2.0f, 5.0f),
        //            CenterY = 40
        //        }; 

        //        DataItems[j] = tmpI;
        //        Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, j).rect.RenderTransform = new ScaleTransform()
        //        {
        //            ScaleY = Comm.GetSingleGradientVal(tmpI.Value ?? 0, Values, 2.0f, 5.0f),
        //            CenterY = 40
        //        };

        //        //ClearRenderTransforms();
        //        //DataItems = DataItems; // 触发 Setter 通知前台更新（虽然是 ObservableCollection，但目前没法通过“交换元素”来触发更新事件
        //    };



        //    MainStoryboard.AddAsyncAnimations
        //    (
        //        new List<AnimationTimeline>
        //        {
        //            new SimulatedDoubleAnimation(to: (j - i) * 52, time: 500),
        //            new SimulatedDoubleAnimation(to: (i - j) * 52, time: 500),
        //        },
        //        new List<UIElement>
        //        {
        //            Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, i).valueItem,
        //            Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, j).valueItem
        //        },
        //        new List<object>
        //        {
        //            "(UIElement.RenderTransform).(TranslateTransform.X)",
        //            "(UIElement.RenderTransform).(TranslateTransform.X)"
        //        },
        //        null,
        //        () =>
        //        {
        //            dataItemsSwap();
        //            after?.Invoke();
        //        }
        //    );

        //}

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

        protected void UpdateValues()
        {
            int[] values = new int[DataItems.Count];
            for (int i = 0; i < DataItems.Count; i++)
            {
                values[i] = DataItems[table[i]].Value ?? 0;
            }
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int[]>(values));
        }

        private void ClearRenderTransforms()
        {
            for (int i = 0; i < DataItems.Count; i++)
            {
                Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, i).valueItem.RenderTransform = new TranslateTransform() { X = 0 };
            }
        }
    }
}
