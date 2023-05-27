using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Structs;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
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
    internal class BubbleSortFactory : SortFactory
    {
        public UIElement Iterator { get; set; }

        public BubbleSortFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
            string codeBlock =
                "for (int i = n - 1; i >= 0; i--)\\" +
                "{\\" +
                "   for (int j = 0; j < i; j++)\\" +
                "   {\\" +
                "       if (a[j] > a[j + 1])\\" +
                "       {\\" +
                "           int tmp = a[j];\\" +
                "           a[j] = a[j + 1];\\" +
                "           a[j + 1] = tmp;\\" +
                "       }\\" +
                "   }\\" +
                "}\\";
            CodeBlockPanel.SetCodeBlock(codeBlock);

            codeInfos.Add("for1", new CodeInfo(0));
            codeInfos.Add("for2", new CodeInfo(2));
            codeInfos.Add("if", new CodeInfo(4));
            codeInfos.Add("swap", new CodeInfo(6, 3));
        }

        protected override void HiddenAllAuxiliaryControls()
        {
            Iterator.Visibility = Visibility.Hidden;
        }

        public override void Execute()
        {
            bool isGreater = false;

            Stick(() =>
            {
                IterBegin(Iterator);
                Init();
            });

            for (int i = last - 1; i >= 0; i--)
            {
                for (int j = 0; ;)
                {
                    CodeBlockPanel.AddAnimation(codeInfos["if"], MainStoryboard.Offset);
                    MainStoryboard.Delay(300);
                    isGreater = DataItems[table[j]].Value > DataItems[table[j + 1]].Value;

                    if (isGreater) ElemSwap(j, new LogViewModel($"a[{j}] > a[{j+1}]", "Swap adjacent elem", $"int tmp = a[{j}]; a[{j}] = a[{j+1}]; a[{j+1}] = tmp;"));
                    if (++j > i) break;

                    IterNext(j, new LogViewModel("Next elem", "j++;"));
                }
                IterReturn(0, i + 1, new LogViewModel("Return to front", "i--; j = 0;"));
            }

            UnStick(() =>
            {
                IterEnd(Iterator);
                Finish();
            });

            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        /// <summary>
        /// 交换两元素后将后一个位置标记为“当前最大”
        /// </summary>
        /// <param name="iterIndex"></param>
        private void ElemSwap(int iterIndex, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["swap"], MainStoryboard.Offset);
            SwapElems(iterIndex, iterIndex + 1, null, () =>
            {
                SetElemMax(iterIndex + 1);
            }, log);
        }

        /// <summary>
        /// 迭代器遍历下一个元素
        /// </summary>
        /// <param name="toIndex"></param>
        private void IterNext(int toIndex, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["for2"], MainStoryboard.Offset);

            int toRealIndex = table[toIndex];

            MoveIter(Iterator, toIndex, null, () =>
            {
                ActivateElem(toRealIndex);
                SetElemMax(toIndex);
            }, log: log);
        }

        /// <summary>
        /// 一趟冒泡结束后，迭代器返回到数组头部
        /// </summary>
        /// <param name="toIndex"></param>
        /// <param name="sortedIndex"></param>
        private void IterReturn(int toIndex, int sortedIndex, LogViewModel log)
        {
            CodeBlockPanel.AddAnimation(codeInfos["for1"], MainStoryboard.Offset);

            int toRealIndex = table[toIndex];

            MoveIter(Iterator, toIndex, () => { DataItems[sortedIndex].State = DataItemState.Sorted; }, () =>
            {
                ActivateElem(toRealIndex);
                SetElemMax(toIndex);
            }, log: log);
        }

        protected override void Init()
        {
            ActivateElem(0);
            SetElemMax(0);
        }
    }
}
