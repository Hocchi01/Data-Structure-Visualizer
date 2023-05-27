using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Structs;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class SelectionSortFactory : SortFactory
    {
        private int minIndex = 0;

        public UIElement Iterator { get; set; }

        public SelectionSortFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
            string codeBlock = 
                "for (int i = 0; i < n - 1; i++)\\" +
                "{\\" +
                "   int minIndex = i;\\" +
                "   for (int j = i + 1; j < n; j++)\\" +
                "   {\\" +
                "       if (a[j] < a[minIndex])\\" +
                "       {\\" +
                "           minIndex = j;\\" +
                "       }\\" +
                "   }\\" +
                "   int tmp = a[i];\\" +
                "   a[i] = a[minIndex];\\" +
                "   a[minIndex] = tmp;\\" +
                "}\\";
            CodeBlockPanel.SetCodeBlock(codeBlock);

            codeInfos.Add("for1", new CodeInfo(0));
            codeInfos.Add("initMin", new CodeInfo(2));
            codeInfos.Add("for2", new CodeInfo(3));
            codeInfos.Add("if", new CodeInfo(5));
            codeInfos.Add("recdMin", new CodeInfo(7));
            codeInfos.Add("swap", new CodeInfo(10, 3));
        }

        protected override void HiddenAllAuxiliaryControls()
        {
            Iterator.Visibility = Visibility.Hidden;
        }

        public override void Execute()
        {
            bool isLess = false;

            Stick(() =>
            {
                IterBegin(Iterator);
                Init();
            });

            for (int i = 0; i < last; i++)
            {
                minIndex = i;
                for (int j = i + 1; j <= last; j++)
                {
                    isLess = DataItems[table[j]].Value < DataItems[table[minIndex]].Value;

                    IterNext(j, minIndex, isLess, new LogViewModel("Next elem", "j++;"));

                    if (isLess) minIndex = j;
                }

                ElemSwap(i, minIndex, new LogViewModel("Swap min and front elem", $"int tmp = a[{minIndex}]; a[{minIndex}] = a[{i}]; a[{i}] = tmp;"));
                IterReturn(i + 1, new LogViewModel("Return to front", "i++; j = i;"));
            }

            UnStick(() =>
            {
                IterEnd(Iterator);
                Finish();
            });
            MainStoryboard.Begin_Ex(Canvas, true);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView, true);
        }

        /// <summary>
        /// 一趟比较后，交换头部和最小元素
        /// </summary>
        /// <param name="leftIndex"></param>
        /// <param name="minIndex"></param>
        private void ElemSwap(int leftIndex, int minIndex, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["swap"], MainStoryboard.Offset);
            SwapElems(leftIndex, minIndex, null, () =>
            {
                DataItems[minIndex].State = DataItemState.Normal;
                DataItems[leftIndex].State = DataItemState.Sorted;
            }, log);
        }

        /// <summary>
        /// 迭代器遍历下一个元素
        /// </summary>
        /// <param name="toIndex"></param>
        /// <param name="minIndex"></param>
        /// <param name="isLess"></param>
        private void IterNext(int toIndex, int minIndex, bool isLess, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["for2"], MainStoryboard.Offset);

            int toRealIndex = table[toIndex];

            MoveIter(iter: Iterator, toIndex: toIndex, log: log, after: () =>
            {
                ActivateElem(toRealIndex);
                if (isLess)
                {
                    UpdateMinElem(toIndex, minIndex);
                }
            });

            CodeBlockPanel.AddAnimation(codeInfos["if"], MainStoryboard.Offset);
            MainStoryboard.Delay(300);

            if (isLess)
            {
                CodeBlockPanel.AddAnimation(codeInfos["recdMin"]);
                MainStoryboard.InsertLog(new LogViewModel($"a[{toIndex}] < a[{minIndex}]", "Record min elem", $"minIndex = {toIndex};"));
            }
        }

        /// <summary>
        /// 一趟结束后，迭代器返回到未排序序列头部
        /// </summary>
        /// <param name="toIndex"></param>
        private void IterReturn(int toIndex, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["for1"], MainStoryboard.Offset);

            int toRealIndex = table[toIndex];

            MoveIter(iter: Iterator, toIndex: toIndex, log: log, after: () =>
            {
                ActivateElem(toRealIndex);
                DataItems[toIndex].State = DataItemState.Min;
            });

            CodeBlockPanel.AddAnimation(codeInfos["initMin"], MainStoryboard.Offset);
            MainStoryboard.Delay(300);
        }

        protected override void Init()
        {
            ActivateElem(0);
            SetElemMin(0);
        }
    }
}
