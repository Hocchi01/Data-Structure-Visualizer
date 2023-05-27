using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.Common.Structs;
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
            string codeBlock =
                "void QuickSort(int A[], int low, int high)\\" +
                "{\\" +
                "   if (low < high)\\" +
                "   {\\" +
                "       int pivot_pos = Partition(A, low, high);\\" +
                "       QuickSort(A, low, pivot_pos - 1);\\" +
                "       QuickSort(A, pivot_pos + 1, high);\\" +
                "   }\\" +
                "}\\" +
                "   \\" +
                "int Partition(int A[], int low, int high)\\" +
                "{\\" +
                "   int pivot = A[low];\\" +
                "   while (low < high)\\" +
                "   {\\" +
                "       while (low < high && A[high] >= pivot)\\" +
                "           --high;\\" +
                "       A[low] = A[high];\\" +
                "       while (low < high && A[low] <= pivot)\\" +
                "           ++low;\\" +
                "       A[high] = A[low];\\" +
                "   }\\" +
                "   A[low] = pivot;\\" +
                "   return low;\\" +
                "}\\";
            CodeBlockPanel.SetCodeBlock(codeBlock);

            codeInfos.Add("func_qs", new CodeInfo(0));
            codeInfos.Add("if", new CodeInfo(2));
            codeInfos.Add("p", new CodeInfo(4));
            codeInfos.Add("qs_l", new CodeInfo(5));
            codeInfos.Add("qs_r", new CodeInfo(6));
            codeInfos.Add("func_p", new CodeInfo(10));
            codeInfos.Add("record", new CodeInfo(12));
            codeInfos.Add("w1", new CodeInfo(13));
            codeInfos.Add("w2", new CodeInfo(15));
            codeInfos.Add("high", new CodeInfo(16));
            codeInfos.Add("move_h", new CodeInfo(17));
            codeInfos.Add("w3", new CodeInfo(18));
            codeInfos.Add("low", new CodeInfo(19));
            codeInfos.Add("move_l", new CodeInfo(20));
            codeInfos.Add("ret_pivot", new CodeInfo(22));
            codeInfos.Add("return", new CodeInfo(23));
            
        }

        protected override void HiddenAllAuxiliaryControls()
        {
            IterEnd(LowIterator);
            IterEnd(HighIterator);
            Pivot.Visibility = Visibility.Hidden;
        }

        void QuickSort(int lowIndex, int highIndex)
        {
            CodeBlockPanel.AddAnimation(codeInfos["func_qs"], MainStoryboard.Offset, 500);
            CodeBlockPanel.AddAnimation(codeInfos["if"]);
            if (lowIndex < highIndex) // 如果只有一个元素（即：low == high）则无需排序
            {
                CodeBlockPanel.AddAnimation(codeInfos["p"]);
                int pivotIndex = Partition(lowIndex, highIndex);

                CodeBlockPanel.AddAnimation(codeInfos["qs_l"], time: 500);
                QuickSort(lowIndex, pivotIndex - 1);

                CodeBlockPanel.AddAnimation(codeInfos["qs_r"], time: 500);
                QuickSort(pivotIndex + 1, highIndex);
            }
            else if (lowIndex == highIndex)
            {
                ItersReturn(lowIndex, highIndex, new LogViewModel("Only one elem in range", "Do nothing", ""));
            }
        }

        int Partition(int lowIndex, int highIndex)
        {
            MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
            CodeBlockPanel.AddAnimation(codeInfos["func_p"]);

            ItersReturn(lowIndex, highIndex, new LogViewModel($"Partition range[{lowIndex}, {highIndex}]", $"lowIndex = {lowIndex}; highIndex = {highIndex};"));

            int pivot = DataItems[table[lowIndex]].Value ?? 0; // 将当前表中第一个元素设为枢轴值，对表进行划分
            TmpStorePivotElem(lowIndex, new LogViewModel("Record pivot elem", $"pivot = a[{lowIndex}];"));

            CodeBlockPanel.AddAnimation(codeInfos["w1"], MainStoryboard.Offset);
            while (lowIndex < highIndex)
            {
                CodeBlockPanel.AddAnimation(codeInfos["w2"]);
                MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
                while (lowIndex < highIndex && DataItems[table[highIndex]].Value >= pivot)
                {
                    CodeBlockPanel.AddAnimation(codeInfos["high"]);

                    --highIndex;
                    IterNext(HighIterator, highIndex, new LogViewModel("Left shift highIndex", "highIndex--;"));

                    CodeBlockPanel.AddAnimation(codeInfos["w2"], MainStoryboard.Offset);
                    MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
                }

                CodeBlockPanel.AddAnimation(codeInfos["move_h"]);
                MoveElem(highIndex, lowIndex, log: lowIndex < highIndex ? new LogViewModel($"a[{highIndex}] < pivot", "Left shift less elem", $"a[{lowIndex}] = a[{highIndex}];") : null); // 将比枢轴值小的元素移动到左端

                CodeBlockPanel.AddAnimation(codeInfos["w3"], MainStoryboard.Offset);
                MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
                while (lowIndex < highIndex && DataItems[table[lowIndex]].Value <= pivot)
                {
                    CodeBlockPanel.AddAnimation(codeInfos["low"]);

                    ++lowIndex;
                    IterNext(LowIterator, lowIndex, new LogViewModel("Right shift lowIndex", "lowIndex++;"));

                    CodeBlockPanel.AddAnimation(codeInfos["w3"], MainStoryboard.Offset);
                    MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
                }

                CodeBlockPanel.AddAnimation(codeInfos["move_l"]);
                MoveElem(lowIndex, highIndex, log: lowIndex < highIndex ? new LogViewModel($"a[{lowIndex}] > pivot", "Right shift greater elem", $"a[{highIndex}] = a[{lowIndex}];") : null); // 将比枢轴值大的元素移动到右端

                CodeBlockPanel.AddAnimation(codeInfos["w1"], MainStoryboard.Offset, 300);
                MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
            }

            ReturnPivotElem(lowIndex, new LogViewModel("Rewrite pivot elem", $"a[{lowIndex}] = pivot;"));

            CodeBlockPanel.AddAnimation(codeInfos["return"]);
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

            MainStoryboard.Begin_Ex(Canvas);
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
            CodeBlockPanel.AddAnimation(codeInfos["record"], MainStoryboard.Offset);

            float by = -(float)(AnimationHelper.StepLen * elemIndex + offset);

            MoveElem(elemIndex: elemIndex, by: by, log: log);
        }

        /// <summary>
        /// 将暂存在 pivot 中的元素挪回数组中
        /// </summary>
        /// <param name="toIndex"></param>
        private void ReturnPivotElem(int toIndex, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["ret_pivot"], MainStoryboard.Offset, 300);

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
