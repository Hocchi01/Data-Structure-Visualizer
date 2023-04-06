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

        /// <summary>
        /// 迭代器遍历下一个元素
        /// </summary>
        /// <param name="iter"></param>
        /// <param name="toIndex"></param>
        private void IterNext(UIElement iter, int toIndex)
        {
            int toRealIndex = table[toIndex];
            MoveIter(iter, toIndex, null, () => { ActivateElem(toRealIndex); }, offset);
        }

        /// <summary>
        /// 初始动画：右移数组；显示 pivot
        /// </summary>
        private void LoadBeginAnimation()
        {
            var rightMove = new SimulatedDoubleAnimation(by: (float)offset, time: 500);
            var targetControls = new List<UIElement> { Container, LowIterator, HighIterator };
            string param = AnimationHelper.HorizontallyMoveParam;

            MainStoryboard.AddAsyncAnimations(rightMove, targetControls, param, null, () => { Pivot.Visibility = Visibility.Visible; });
        }

        /// <summary>
        /// 收尾动画：隐藏 pivot；左移数组恢复至原位
        /// </summary>
        private void LoadEndAnimation()
        {
            var leftMove = new SimulatedDoubleAnimation(by: -(float)offset, time: 500);
            var targetControls = new List<UIElement> { Container, LowIterator, HighIterator };
            string param = AnimationHelper.HorizontallyMoveParam;

            MainStoryboard.AddAsyncAnimations(leftMove, targetControls, param, () => { Pivot.Visibility = Visibility.Hidden; });
        }

        /// <summary>
        /// 将当前趟的 pivot 暂存到外部
        /// </summary>
        /// <param name="elemIndex"></param>
        private void TmpStorePivotElem(int elemIndex)
        {
            //int elemRealIndex = table[elemIndex];
            //var elem = Comm.GetItemFromItemsControlByIndex<SuccessiveItemUserControl>(Container, elemRealIndex);

            float by = -(float)(AnimationHelper.StepLen * elemIndex + offset);

            //MainStoryboard.AddSyncAnimation(elem.MoveValueItem(by, () => { ActivateElem(elemRealIndex); }));

            MoveElem(elemIndex, by);
        }

        /// <summary>
        /// 将暂存在 pivot 中的元素挪回数组中
        /// </summary>
        /// <param name="toIndex"></param>
        private void ReturnPivotElem(int toIndex)
        {
            //int elemRealIndex = table[toIndex];
            //var elem = Comm.GetItemFromItemsControlByIndex<SuccessiveItemUserControl>(Container, elemRealIndex);

            float by = (float)(AnimationHelper.StepLen * toIndex + offset);

            //MainStoryboard.AddSyncAnimation(elem.MoveValueItem(by, () => { ActivateElem(elemRealIndex); }, () => { SetElemSorted(toIndex); }));

            MoveElem(toIndex, by, null, () => { SetElemSorted(toIndex); });
        }

        /// <summary>
        /// 将两个迭代器跳到下一个要划分的区间
        /// </summary>
        /// <param name="lowIndex"></param>
        /// <param name="highIndex"></param>
        private void ItersReturn(int lowIndex, int highIndex)
        {
            var lowIterMove = GetIterMovementAnimation(LowIterator, lowIndex, null, null, offset);
            var highIterMove = GetIterMovementAnimation(HighIterator, highIndex, null, null, offset);

            // 若划分区间只有一个元素，则无需做任何操作
            Action? after = lowIndex != highIndex ? null : () => 
            {
                ActivateElem(table[lowIndex]);
                SetElemSorted(lowIndex); 
            };

            MainStoryboard.AddAsyncAnimations(new List<SimulatedDoubleAnimation> { lowIterMove, highIterMove }, null, after);
        }
    }
}
