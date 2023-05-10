using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
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

        public QuickSortFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
        }

        protected override void HiddenAllAuxiliaryControls()
        {
            IterEnd(LowIterator);
            IterEnd(HighIterator);
            Pivot.Visibility = Visibility.Hidden;
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
                ItersReturn(lowIndex, highIndex, new LogViewModel("Only one elem in range", "Do nothing", ""));
            }
        }

        int Partition(int lowIndex, int highIndex)
        {
            ItersReturn(lowIndex, highIndex, new LogViewModel($"Partition range[{lowIndex}, {highIndex}]", $"lowIndex = {lowIndex}; highIndex = {highIndex};"));

            int pivot = DataItems[table[lowIndex]].Value ?? 0; // 将当前表中第一个元素设为枢轴值，对表进行划分
            TmpStorePivotElem(lowIndex, new LogViewModel("Record pivot elem", $"pivot = a[{lowIndex}];"));

            while (lowIndex < highIndex)
            {
                while (lowIndex < highIndex && DataItems[table[highIndex]].Value >= pivot)
                {
                    --highIndex;
                    IterNext(HighIterator, highIndex, new LogViewModel("Left shift highIndex", "highIndex--;"));
                }
                MoveElem(highIndex, lowIndex, log: lowIndex < highIndex ? new LogViewModel($"a[{highIndex}] < pivot", "Left shift less elem", $"a[{lowIndex}] = a[{highIndex}];") : null); // 将比枢轴值小的元素移动到左端

                while (lowIndex < highIndex && DataItems[table[lowIndex]].Value <= pivot)
                {
                    ++lowIndex;
                    IterNext(LowIterator, lowIndex, new LogViewModel("Right shift lowIndex", "lowIndex++;"));
                }
                MoveElem(lowIndex, highIndex, log: lowIndex < highIndex ? new LogViewModel($"a[{lowIndex}] > pivot", "Right shift greater elem", $"a[{highIndex}] = a[{lowIndex}];") : null); // 将比枢轴值大的元素移动到右端
            }

            ReturnPivotElem(lowIndex, new LogViewModel("Rewrite pivot elem", $"a[{lowIndex}] = pivot;"));
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

            CodeBlockPanel.CodeBlockStoryboard.Delay(MainStoryboard.Offset);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
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
        private void IterNext(UIElement iter, int toIndex, LogViewModel log)
        {
            int toRealIndex = table[toIndex];
            MoveIter(iter, toIndex, null, () => { ActivateElem(toRealIndex); }, offset, log);
        }

        /// <summary>
        /// 初始动画：右移数组；显示 pivot
        /// </summary>
        private void LoadBeginAnimation()
        {
            var rightMove = new SimulatedDoubleAnimation(by: (float)offset, time: 500);
            var targetControls = new List<DependencyObject> { Container, LowIterator, HighIterator };
            string param = AnimationHelper.HorizontallyMoveParam;

            MainStoryboard.AddAsyncAnimations(rightMove, targetControls, param, null, () => { Pivot.Visibility = Visibility.Visible; });
        }

        /// <summary>
        /// 收尾动画：隐藏 pivot；左移数组恢复至原位
        /// </summary>
        private void LoadEndAnimation()
        {
            var leftMove = new SimulatedDoubleAnimation(by: -(float)offset, time: 500);
            var targetControls = new List<DependencyObject> { Container, LowIterator, HighIterator };
            string param = AnimationHelper.HorizontallyMoveParam;

            MainStoryboard.AddAsyncAnimations(leftMove, targetControls, param, () => { Pivot.Visibility = Visibility.Hidden; });
        }

        /// <summary>
        /// 将当前趟的 pivot 暂存到外部
        /// </summary>
        /// <param name="elemIndex"></param>
        private void TmpStorePivotElem(int elemIndex, LogViewModel log)
        {
            float by = -(float)(AnimationHelper.StepLen * elemIndex + offset);

            MoveElem(elemIndex: elemIndex, by: by, log: log);
        }

        /// <summary>
        /// 将暂存在 pivot 中的元素挪回数组中
        /// </summary>
        /// <param name="toIndex"></param>
        private void ReturnPivotElem(int toIndex, LogViewModel log)
        {
            float by = (float)(AnimationHelper.StepLen * toIndex + offset);

            MoveElem(toIndex, by, null, () => { SetElemSorted(toIndex); }, log);
        }

        /// <summary>
        /// 将两个迭代器跳到下一个要划分的区间
        /// </summary>
        /// <param name="lowIndex"></param>
        /// <param name="highIndex"></param>
        private void ItersReturn(int lowIndex, int highIndex, LogViewModel log)
        {
            var lowIterMove = GetIterMovementAnimation(LowIterator, lowIndex, null, null, offset);
            var highIterMove = GetIterMovementAnimation(HighIterator, highIndex, null, null, offset);

            // 若划分区间只有一个元素，则无需做任何操作
            Action? after = lowIndex != highIndex ? null : () => 
            {
                ActivateElem(table[lowIndex]);
                SetElemSorted(lowIndex); 
            };

            MainStoryboard.AddAsyncAnimations(new List<SimulatedDoubleAnimation> { lowIterMove, highIterMove }, null, after, log);
        }
    }
}
