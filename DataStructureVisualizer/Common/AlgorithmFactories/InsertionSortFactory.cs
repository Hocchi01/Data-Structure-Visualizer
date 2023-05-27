using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Structs;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class InsertionSortFactory : SortFactory
    {
        public UIElement Iterator { get; set; }

        public InsertionSortFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
            string codeBlock =
                "for (int i = 1; i < n; i++)\\" +
                "{\\" +
                "   for (int j = i - 1; j >= 0; j--)\\" +
                "   {\\" +
                "       if (a[j + 1] < a[j])\\" +
                "       {\\" +
                "           int tmp = a[j];\\" +
                "           a[j] = a[j + 1];\\" +
                "           a[j + 1] = tmp;\\" +
                "       }\\" +
                "       else break;\\" +
                "   }\\" +
                "}\\";

            CodeBlockPanel.SetCodeBlock(codeBlock);

            codeInfos.Add("for1", new CodeInfo(0));
            codeInfos.Add("for2", new CodeInfo(2));
            codeInfos.Add("if", new CodeInfo(4));
            codeInfos.Add("swap", new CodeInfo(6, 3));
            codeInfos.Add("else", new CodeInfo(10));
        }

        protected override void HiddenAllAuxiliaryControls()
        {
            Iterator.Visibility = Visibility.Hidden;
        }

        public override void Execute()
        {
            bool isLess = false;

            Stick(() => { IterBegin(Iterator, 1); Init(); });

            for (int i = 1; i < count; i++)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    isLess = DataItems[table[j + 1]].Value < DataItems[table[j]].Value;

                    IterNext(j, new LogViewModel("Next elem", "j--;"));

                    CodeBlockPanel.AddAnimation(codeInfos["if"], MainStoryboard.Offset);
                    MainStoryboard.Delay(300);
                    if (isLess) ElemSwap(j, new LogViewModel($"a[{j+1}] < a[{j}]", "Swap adjacent elem", $"int tmp = a[{j}]; a[{j}] = a[{j + 1}]; a[{j + 1}] = tmp;"));
                    if (!isLess || j == 0) // 当迭代器到最左端时不能继续左移了，因此这里需要特判
                    {
                        IterReturn(i + 1, j, isLess, new LogViewModel("Return to front", "i++; j = i - 1;"));
                        break;
                    }
                }
            }

            UnStick(() => { IterEnd(Iterator); Finish(); });
            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        /// <summary>
        /// 交换两元素
        /// </summary>
        /// <param name="iterIndex"></param>
        private void ElemSwap(int iterIndex, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["swap"], MainStoryboard.Offset);
            SwapElems(iterIndex, iterIndex + 1, null, () =>
            {
                SetElemMin(iterIndex);
                SetElemSorted(iterIndex + 1);
            }, log);
        }

        /// <summary>
        /// 迭代器遍历下一个元素
        /// </summary>
        /// <param name="toIndex"></param>
        private void IterNext(int toIndex, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["for2"], MainStoryboard.Offset);
            MoveIter(Iterator, toIndex, null, null, log: log);
        }

        /// <summary>
        /// 一趟插入排序后将迭代器返回未排序序列头部
        /// </summary>
        /// <param name="toIndex"></param>
        /// <param name="preIndex"></param>
        /// <param name="isLess"></param>
        private void IterReturn(int toIndex, int preIndex, bool isLess, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["for1"], MainStoryboard.Offset);

            int sortedIndex = isLess ? preIndex : preIndex + 1;
            if (toIndex > last) return;

            int toRealIndex = table[toIndex];

            MoveIter(Iterator, toIndex,
            () => { SetElemSorted(sortedIndex); },
            () =>
            {
                ActivateElem(toRealIndex);
                SetElemMin(toIndex);
            }, log: log);
        }

        protected override void Init()
        {
            ActivateElem(1);
            SetElemSorted(0);
        }
    }
}
