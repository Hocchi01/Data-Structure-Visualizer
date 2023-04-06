using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal abstract class SortFactory : SuccessiveAlgorithmFactory
    {
        private int[] vals;

        private int maxStateIndex = 0;
        private int minStateIndex = 0;

        public SortFactory(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, container, myStoryboard, dataItems)
        {
            vals = new int[count];
            for (int i = 0; i < count; i++)
            {
                vals[i] = DataItems[i].Value ?? 0;
            }
        }


        public abstract void Execute();
        protected abstract void Init();

        /// <summary>
        /// 将元素条形化
        /// </summary>
        protected void Stick(Action? after = null)
        {
            Stick(0, last, after);
        }

        protected void Stick(int lowIndex, int highIndex, Action? after = null)
        {
            MainStoryboard.AddAsyncAnimations(GetStickAnimations(lowIndex, highIndex), null, after);
        }

        protected List<SimulatedDoubleAnimation> GetStickAnimations(int lowIndex, int highIndex)
        {
            float[] scaleRatios = Comm.GetGradient(vals, 2.0f, 5.0f);
            var animations = new List<SimulatedDoubleAnimation>();

            for (int i = lowIndex; i <= highIndex; i++)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, table[i]);
                animations.Add(new SimulatedDoubleAnimation(to: scaleRatios[table[i]], time: 500) { TargetControl = itemView.rect, TargetParam = AnimationHelper.VerticallyScaleParam });
            }

            return animations;
        }

        /// <summary>
        /// 将条形化的元素缩回原状态
        /// </summary>
        protected void UnStick(Action? before = null)
        {
            UnStick(0, last, before);
        }

        protected void UnStick(int lowIndex, int highIndex, Action? before = null)
        {
            MainStoryboard.AddAsyncAnimations(GetUnStickAnimations(lowIndex, highIndex), before);
        }

        protected List<SimulatedDoubleAnimation> GetUnStickAnimations(int lowIndex, int highIndex)
        {
            var animations = new List<SimulatedDoubleAnimation>();

            for (int i = lowIndex; i <= highIndex; i++)
            {
                animations.Add(GetUnStickAnimation(i));
            }

            return animations;
        }

        protected SimulatedDoubleAnimation GetUnStickAnimation(int elemIndex)
        {
            var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, table[elemIndex]);
            return new SimulatedDoubleAnimation(to: 1.0, time: 500) { TargetControl = itemView.rect, TargetParam = AnimationHelper.VerticallyScaleParam };
        }

        protected void UpdateMinElem(int elemIndex, int minIndex)
        {
            DataItems[minIndex].State = DataItemState.Normal;
            DataItems[elemIndex].State = DataItemState.Min;
        }

        protected void SetAllElemsSorted()
        {
            foreach (var item in DataItems)
            {
                item.State = DataItemState.Sorted;
            }
        }

        protected void SetElemMax(int elemIndex)
        {
            if (DataItems[maxStateIndex].State == DataItemState.Max)
                DataItems[maxStateIndex].State = DataItemState.Normal;
            DataItems[elemIndex].State = DataItemState.Max;
            maxStateIndex = elemIndex;
        }

        protected void SetElemMin(int elemIndex)
        {
            if (DataItems[minStateIndex].State == DataItemState.Min)
                DataItems[minStateIndex].State = DataItemState.Normal;
            DataItems[elemIndex].State = DataItemState.Min;
            minStateIndex = elemIndex;
        }

        protected void SetElemSorted(int elemIndex)
        {
            DataItems[elemIndex].State = DataItemState.Sorted;
        }

        protected void IterBegin(UIElement iterator, int beginIndex = 0)
        {
            (iterator.RenderTransform as TranslateTransform).X = AnimationHelper.ArrayStart + AnimationHelper.StepLen * beginIndex; // 起始时指向 1 号位
            iterator.Visibility = Visibility.Visible;
        }

        protected void IterEnd(UIElement iterator)
        {
            iterator.Visibility = Visibility.Hidden;
            iterator.RenderTransform = new TranslateTransform() { X = AnimationHelper.ArrayStart }; // 复位迭代器
        }

        protected void Finish()
        {
            DeactivateAllElems();
            SetAllElemsSorted();
        }

    }
}
