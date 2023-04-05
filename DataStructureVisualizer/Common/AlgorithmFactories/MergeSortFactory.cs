using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class MergeSortFactory : SortFactory
    {
        private int[] sortedIndices;
        private int g1StateBegIndex = 0;
        private int g1StateEndIndex = 0;
        private int g2StateBegIndex = 0;
        private int g2StateEndIndex = 0;

        public UIElement Group1Iterator { get; set; }
        public UIElement Group2Iterator { get; set; }
        public ItemsControl TmpArray { get; set; }

        public MergeSortFactory(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, container, myStoryboard, dataItems)
        {
            sortedIndices = new int[count];
        }

        private void MergeSort(int begIndex, int endIndex)
        {
            if (begIndex >= endIndex) return;

            int midIndex = (begIndex + endIndex) / 2;
            int g1BegIndex = begIndex, g1EndIndex = midIndex;
            int g2BegIndex = midIndex + 1, g2EndIndex = endIndex;

            MergeSort(g1BegIndex, g1EndIndex);
            MergeSort(g2BegIndex, g2EndIndex);

            int iter = 0, g1Iter = g1BegIndex, g2Iter = g2BegIndex;

            ItersReturn(g1BegIndex, g2BegIndex);
            MainStoryboard.InsertAction(() => { DivideIntoGroups(g1BegIndex, g1EndIndex, g2BegIndex, g2EndIndex); });
            MainStoryboard.InsertAction(() => { GenerateTmpArray(endIndex - begIndex + 1); });

            while (g1Iter <= g1EndIndex && g2Iter <= g2EndIndex)
            {
                if (DataItems[table[g1Iter]].Value < DataItems[table[g2Iter]].Value)
                {
                    TmpStoreElem(g1Iter, iter);
                    sortedIndices[iter++] = g1Iter++;
                    if (g1Iter <= g1EndIndex) IterNext(Group1Iterator, g1Iter);
                }
                else
                {
                    TmpStoreElem(g2Iter, iter);
                    sortedIndices[iter++] = g2Iter++;
                    if (g2Iter <= g2EndIndex) IterNext(Group2Iterator, g2Iter);
                }
            }

            while (g1Iter <= g1EndIndex)
            {
                TmpStoreElem(g1Iter, iter);
                sortedIndices[iter++] = g1Iter++;
                if (g1Iter <= g1EndIndex) IterNext(Group1Iterator, g1Iter);
            }

            while (g2Iter <= g2EndIndex)
            {
                TmpStoreElem(g2Iter, iter);
                sortedIndices[iter++] = g2Iter++;
                if (g2Iter <= g2EndIndex) IterNext(Group2Iterator, g2Iter);
            }

            MergeElemsInTable(begIndex, iter);
            ElemsReturn(begIndex, endIndex);
        }



        public override void Execute()
        {
            Stick(() =>
            {
                IterBegin(Group1Iterator);
                IterBegin(Group2Iterator, last);
            });

            MergeSort(0, last);

            UnStick(() =>
            {
                IterEnd(Group1Iterator);
                IterEnd(Group2Iterator);
                Finish();
                HideTmpArray();
            });

            MainStoryboard.Begin_Ex(Canvas, true);
        }

        protected override void Init()
        {
            throw new NotImplementedException();
        }

        private void ItersReturn(int g1BegIndex, int g2BegIndex)
        {
            double start = AnimationHelper.ArrayStart;
            double step = AnimationHelper.StepLen;
            string param = AnimationHelper.HorizontallyMoveParam;

            var g1Animation = new SimulatedDoubleAnimation(to: start + step * g1BegIndex, time: 500) { TargetControl = Group1Iterator, TargetParam = param };

            var g2Animation = new SimulatedDoubleAnimation(to: start + step * g2BegIndex, time: 500) { TargetControl = Group2Iterator, TargetParam = param };

            MainStoryboard.AddAsyncAnimations(new List<SimulatedDoubleAnimation> { g1Animation, g2Animation });
        }

        private void DivideIntoGroups(int g1BegIndex, int g1EndIndex, int g2BegIndex, int g2EndIndex)
        {
            for (int i = g1StateBegIndex; i <= g1StateEndIndex; i++)
            {
                DataItems[i].State = DataItemState.Normal;
            }
            for (int i = g2StateBegIndex; i <= g2StateEndIndex; i++)
            {
                DataItems[i].State = DataItemState.Normal;
            }
            for (int i = g1BegIndex; i <= g1EndIndex; i++)
            {
                DataItems[i].State = DataItemState.Group1;
            }
            for (int i = g2BegIndex; i <= g2EndIndex; i++)
            {
                DataItems[i].State = DataItemState.Group2;
            }
            g1StateBegIndex = g1BegIndex;
            g1StateEndIndex = g1EndIndex;
            g2StateBegIndex = g2BegIndex;
            g2StateEndIndex = g2EndIndex;
        }

        private void MergeElemsInTable(int begIndex, int len)
        {
            int[] tmpRealIndices = new int[len];
            for (int i = 0; i < len; i++)
            {
                tmpRealIndices[i] = table[sortedIndices[i]];
            }

            for (int i = 0; i < len; i++)
            {
                table[begIndex + i] = tmpRealIndices[i];
            }
        }

        private void TmpStoreElem(int elemIndex, int toIndex)
        {
            int elemRealIndex = table[elemIndex];

            var downMove = new SimulatedDoubleAnimation(by: 80, time: 500)
            {
                TargetControl = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, elemRealIndex).valueItem,
                TargetParam = AnimationHelper.VerticallyMoveParam
            };
            var scaleBack = GetUnStickAnimations(elemIndex, elemIndex);
            var animations = new List<SimulatedDoubleAnimation>
            {
                downMove
            };
            animations.AddRange(scaleBack);

            MainStoryboard.AddAsyncAnimations(animations, () => { ActivateElem(elemRealIndex); });
            MoveElem(elemIndex, toIndex, false);
        }

        private void IterNext(UIElement iter, int toIndex)
        {
            int toRealIndex = table[toIndex];

            MoveIter(iter, toIndex);
        }

        private void ElemsReturn(int begIndex, int endIndex)
        {
            var rightMove = new SimulatedDoubleAnimation(by: begIndex * (float)AnimationHelper.StepLen, time: 500);
            var targetControls1 = new List<UIElement>() { TmpArray };
            for (int i = begIndex; i <= endIndex; i++)
            {
                targetControls1.Add(Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, table[i]).valueItem);
            }

            var leftMove = new SimulatedDoubleAnimation(by: -begIndex * (float)AnimationHelper.StepLen, time: 500)
            {
                TargetControl = TmpArray,
                TargetParam = AnimationHelper.HorizontallyMoveParam,
            };

            var upMove = new SimulatedDoubleAnimation(by: -80, time: 500);
            var targetControls2 = new List<UIElement>();
            for (int i = begIndex; i <= endIndex; i++)
            {
                targetControls2.Add(Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, table[i]).valueItem);
            }

            var stick = GetStickAnimations(begIndex, endIndex);

            MainStoryboard.AddAsyncAnimations(rightMove, targetControls1, AnimationHelper.HorizontallyMoveParam);
            MainStoryboard.AddAsyncAnimations(upMove, targetControls2, AnimationHelper.VerticallyMoveParam);
            MainStoryboard.AddAsyncAnimations(stick);
            MainStoryboard.AddSyncAnimation(leftMove);
        }

        private void GenerateTmpArray(int len)
        {
            for (int i = 0; i < count; i++)
            {
                var item = Comm.GetItemFromItemsControlByIndex<Grid>(TmpArray, i);
                if (i < len) item.Visibility = Visibility.Visible; else item.Visibility = Visibility.Hidden;
            }
        }

        private void HideTmpArray()
        {
            GenerateTmpArray(0);
        }
    }
}
