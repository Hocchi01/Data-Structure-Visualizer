using DataStructureVisualizer.Common.AnimationLib;
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
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class QuickSortFactory : SortFactory
    {
        private double offset = 60;

        public UIElement LowIterator { get; set; }
        public UIElement HighIterator { get; set; }
        public UIElement Pivot { get; set; }

        public QuickSortFactory(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, container, myStoryboard, dataItems)
        {
        }

        void QuickSort(int lowIndex, int highIndex)
        {
            if (lowIndex < highIndex) // 如果只有一个元素（即：low == high）则无需排序
            {
                int pivotIndex = Partition(lowIndex, highIndex);
                QuickSort(lowIndex, pivotIndex - 1);
                QuickSort(pivotIndex + 1, highIndex);
            }
            else if (lowIndex == highIndex)
            {
                ItersReturn(lowIndex, highIndex);
            }
        }

        int Partition(int lowIndex, int highIndex)
        {
            ItersReturn(lowIndex, highIndex);

            int pivot = DataItems[table[lowIndex]].Value ?? 0; // 将当前表中第一个元素设为枢轴值，对表进行划分
            TmpStorePivotElem(lowIndex);

            while (lowIndex < highIndex)
            {
                while (lowIndex < highIndex && DataItems[table[highIndex]].Value >= pivot)
                {
                    --highIndex;
                    IterNext(HighIterator, highIndex);
                }
                MoveElem(highIndex, lowIndex); // 将比枢轴值小的元素移动到左端

                while (lowIndex < highIndex && DataItems[table[lowIndex]].Value <= pivot)
                {
                    ++lowIndex;
                    IterNext(LowIterator, lowIndex);
                }
                MoveElem(lowIndex, highIndex); // 将比枢轴值大的元素移动到右端
            }

            ReturnPivotElem(lowIndex);
            return lowIndex;
        }

        public override void Execute()
        {
            Stick(() =>
            {
                IterBegin(LowIterator);
                IterBegin(HighIterator, last);
            });

            LoadBeginAnimation();

            QuickSort(0, last);

            LoadEndAnimation();

            UnStick(() =>
            {
                IterEnd(LowIterator);
                IterEnd(HighIterator);
                Finish();
                Pivot.Visibility = Visibility.Hidden;
            });

            MainStoryboard.Begin_Ex(Canvas, true);
        }

        protected override void Init()
        {
            // throw new NotImplementedException();
        }

        private void IterNext(UIElement iter, int toIndex)
        {
            int toRealIndex = table[toIndex];

            MoveIter(iter, toIndex, null, () => { ActivateElem(toRealIndex); }, offset);
        }

        private void LoadBeginAnimation()
        {
            var rightMove = new SimulatedDoubleAnimation(by: (float)offset, time: 500);
            var targetControls = new List<UIElement> { Container, LowIterator, HighIterator };
            string param = AnimationHelper.HorizontallyMoveParam;

            MainStoryboard.AddAsyncAnimations(rightMove, targetControls, param, null, () => { Pivot.Visibility = Visibility.Visible; });
        }

        private void LoadEndAnimation()
        {
            var leftMove = new SimulatedDoubleAnimation(by: -(float)offset, time: 500);
            var targetControls = new List<UIElement> { Container, LowIterator, HighIterator };
            string param = AnimationHelper.HorizontallyMoveParam;

            MainStoryboard.AddAsyncAnimations(leftMove, targetControls, param, () => { Pivot.Visibility = Visibility.Hidden; });
        }

        private void TmpStorePivotElem(int elemIndex)
        {
            int elemRealIndex = table[elemIndex];
            var elem = Comm.GetItemFromItemsControlByIndex<SuccessiveItemUserControl>(Container, elemRealIndex);

            float by = -(float)(AnimationHelper.StepLen * elemIndex + offset);

            MainStoryboard.AddSyncAnimation(elem.MoveValueItem(by, () => { ActivateElem(elemRealIndex); }));
        }

        private void ReturnPivotElem(int toIndex)
        {
            int elemRealIndex = table[toIndex];
            var elem = Comm.GetItemFromItemsControlByIndex<SuccessiveItemUserControl>(Container, elemRealIndex);

            float by = (float)(AnimationHelper.StepLen * toIndex + offset);

            MainStoryboard.AddSyncAnimation(elem.MoveValueItem(by, () => { ActivateElem(elemRealIndex); }, () => { SetElemSorted(toIndex); }));
        }

        private void ItersReturn(int lowIndex, int highIndex)
        {
            double start = AnimationHelper.ArrayStart;
            double step = AnimationHelper.StepLen;
            string param = AnimationHelper.HorizontallyMoveParam;

            var lowAnimation = new SimulatedDoubleAnimation(to: start + step * lowIndex + offset, time: 500) { TargetControl = LowIterator, TargetParam = param };

            var highAnimation = new SimulatedDoubleAnimation(to: start + step * highIndex + offset, time: 500) { TargetControl = HighIterator, TargetParam = param };

            Action? after = lowIndex != highIndex ? null : () => 
            {
                ActivateElem(table[lowIndex]);
                SetElemSorted(lowIndex); 
            };

            MainStoryboard.AddAsyncAnimations(new List<SimulatedDoubleAnimation> { lowAnimation, highAnimation }, null, after);
        }
    }
}
