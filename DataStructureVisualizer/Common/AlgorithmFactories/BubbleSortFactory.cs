using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
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

        public BubbleSortFactory(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, container, myStoryboard, dataItems)
        {
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
                    isGreater = DataItems[table[j]].Value > DataItems[table[j + 1]].Value;

                    if (isGreater) ElemSwap(j);
                    if (++j > i) break;

                    IterNext(j);
                }
                IterReturn(0, i + 1);
            }

            UnStick(() =>
            {
                IterEnd(Iterator);
                Finish();
            });
            MainStoryboard.Begin_Ex(Canvas, true);
        }

        /// <summary>
        /// 交换两元素后将后一个位置标记为“当前最大”
        /// </summary>
        /// <param name="iterIndex"></param>
        private void ElemSwap(int iterIndex)
        {
            SwapElems(iterIndex, iterIndex + 1, null, () =>
            {
                SetElemMax(iterIndex + 1);
            });
        }

        /// <summary>
        /// 迭代器遍历下一个元素
        /// </summary>
        /// <param name="toIndex"></param>
        private void IterNext(int toIndex)
        {
            int toRealIndex = table[toIndex];

            MoveIter(Iterator, toIndex, null, () =>
            {
                ActivateElem(toRealIndex);
                SetElemMax(toIndex);
            });
        }

        /// <summary>
        /// 一趟冒泡结束后，迭代器返回到数组头部
        /// </summary>
        /// <param name="toIndex"></param>
        /// <param name="sortedIndex"></param>
        private void IterReturn(int toIndex, int sortedIndex)
        {
            int toRealIndex = table[toIndex];

            MoveIter(Iterator, toIndex, () => { DataItems[sortedIndex].State = DataItemState.Sorted; }, () =>
            {
                ActivateElem(toRealIndex);
                SetElemMax(toIndex);
            });
        }

        protected override void Init()
        {
            ActivateElem(0);
            SetElemMax(0);
        }
    }
}
