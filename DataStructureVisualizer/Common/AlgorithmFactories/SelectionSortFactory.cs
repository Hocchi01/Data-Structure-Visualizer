using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
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

        public SelectionSortFactory(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, container, myStoryboard, dataItems)
        {
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

                    IterNext(j, minIndex, isLess);

                    if (isLess) minIndex = j;
                }

                ElemSwap(i, minIndex);
                IterReturn(i + 1);
            }

            UnStick(() =>
            {
                IterEnd(Iterator);
                Finish();
            });
            MainStoryboard.Begin_Ex(Canvas, true);
        }

        private void ElemSwap(int leftIndex, int minIndex)
        {
            SwapElems(leftIndex, minIndex, null, () =>
            {
                DataItems[minIndex].State = DataItemState.Normal;
                DataItems[leftIndex].State = DataItemState.Sorted;
            });
        }

        private void IterNext(int toIndex, int minIndex, bool isLess)
        {
            int toRealIndex = table[toIndex];

            MoveIter(Iterator, toIndex, null, () =>
            {
                ActivateElem(toRealIndex);
                if (isLess) UpdateMinElem(toIndex, minIndex);
            });
        }

        private void IterReturn(int toIndex)
        {
            int toRealIndex = table[toIndex];

            MoveIter(Iterator, toIndex, null, () =>
            {
                ActivateElem(toRealIndex);
                DataItems[toIndex].State = DataItemState.Min;
            });
        }

        protected override void Init()
        {
            ActivateElem(0);
            SetElemMin(0);
        }
    }
}
