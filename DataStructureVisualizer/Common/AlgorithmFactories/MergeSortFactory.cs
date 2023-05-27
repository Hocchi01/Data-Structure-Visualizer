using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Structs;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
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
        private int[] mergedIndices;
        private int g1StateBegIndex = 0;
        private int g1StateEndIndex = 0;
        private int g2StateBegIndex = 0;
        private int g2StateEndIndex = 0;

        public UIElement Group1Iterator { get; set; }
        public UIElement Group2Iterator { get; set; }
        public ItemsControl TmpArray { get; set; }

        public MergeSortFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
            mergedIndices = new int[count];

            string codeBlock =
                "int* B = (int*)malloc((n+1) * sizeof(int));\\" +
                "void MergeSort(int A[], int low, int high)\\" +
                "{\\" +
                "   if (low < high)\\" +
                "   {\\" +
                "       int mid = (low + high) / 2;\\" +
                "       int i = low, j = mid + 1, k = 0;\\" +
                "       MergeSort(A, low, mid);\\" +
                "       MergeSort(A, mid+1, high);\\" +
                "       while (i <= mid && j <= high)\\" +
                "       {\\" +
                "           if (A[i] <= A[j])\\" +
                "               B[k++] = A[i++];\\" +
                "           else\\" +
                "               B[k++] = A[j++];\\" +
                "       }\\" +
                "       while (i <= mid)    B[k++] = A[i++];\\" +
                "       while (j <= high)   B[k++] = A[j++];\\" +
                "       for (int m = 0; m < k; m++)\\" +
                "           A[low + m] = B[m];\\" +
                "   }\\" +
                "}\\";
            CodeBlockPanel.SetCodeBlock(codeBlock);

            codeInfos.Add("func", new CodeInfo(1));
            codeInfos.Add("ms_l", new CodeInfo(7));
            codeInfos.Add("ms_r", new CodeInfo(8));
            codeInfos.Add("w1", new CodeInfo(9));
            codeInfos.Add("g1", new CodeInfo(12));
            codeInfos.Add("g2", new CodeInfo(14));
            codeInfos.Add("w2", new CodeInfo(16));
            codeInfos.Add("w3", new CodeInfo(17));
            codeInfos.Add("copy", new CodeInfo(18, 2));
        }

        protected override void HiddenAllAuxiliaryControls()
        {
            Group1Iterator.Visibility = Visibility.Hidden;
            Group2Iterator.Visibility = Visibility.Hidden;
        }

        private void MergeSort(int begIndex, int endIndex)
        {
            //CodeBlockPanel.AddAnimation(codeInfos["func"], time: 250);
            //CodeBlockPanel.CodeBlockStoryboard.Delay(150);
            if (begIndex >= endIndex) return;

            int midIndex = (begIndex + endIndex) / 2;
            int g1BegIndex = begIndex, g1EndIndex = midIndex;
            int g2BegIndex = midIndex + 1, g2EndIndex = endIndex;

            //CodeBlockPanel.AddAnimation(codeInfos["ms_l"], time: 250);
            MergeSort(g1BegIndex, g1EndIndex);
            //CodeBlockPanel.AddAnimation(codeInfos["ms_r"], time: 250);
            MergeSort(g2BegIndex, g2EndIndex);

            int iter = 0, g1Iter = g1BegIndex, g2Iter = g2BegIndex;

            MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
            ItersReturn(g1BegIndex, g2BegIndex, new LogViewModel($"Merge [{g1BegIndex}, {g1EndIndex}] and [{g2BegIndex}, {g2EndIndex}]", $"MergeSort({g1BegIndex}, {g2EndIndex})"));
            MainStoryboard.InsertAction(() => { DivideIntoGroups(g1BegIndex, g1EndIndex, g2BegIndex, g2EndIndex); });
            MainStoryboard.InsertAction(() => { GenerateTmpArray(endIndex - begIndex + 1); });

            CodeBlockPanel.AddAnimation(codeInfos["w1"], MainStoryboard.Offset);
            CodeBlockPanel.CodeBlockStoryboard.Delay(150);
            while (g1Iter <= g1EndIndex && g2Iter <= g2EndIndex)
            {
                MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;

                if (DataItems[table[g1Iter]].Value <= DataItems[table[g2Iter]].Value)
                {
                    CodeBlockPanel.AddAnimation(codeInfos["g1"]);
                    OperateGroup1(ref g1Iter, ref iter, g1EndIndex, new LogViewModel($"a[{g1Iter}] <= a[{g2Iter}]", "Pop Group1", $"b[{iter}] = a[{g1Iter}];"));
                }
                else
                {
                    CodeBlockPanel.AddAnimation(codeInfos["g2"]);
                    OperateGroup2(ref g2Iter, ref iter, g2EndIndex, new LogViewModel($"a[{g1Iter}] > a[{g2Iter}]", "Pop Group2", $"b[{iter}] = a[{g2Iter}];"));
                }

                CodeBlockPanel.AddAnimation(codeInfos["w1"], MainStoryboard.Offset);
                CodeBlockPanel.CodeBlockStoryboard.Delay(150);
            }

            CodeBlockPanel.AddAnimation(codeInfos["w2"]);
            MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
            while (g1Iter <= g1EndIndex)
            {
                OperateGroup1(ref g1Iter, ref iter, g1EndIndex, new LogViewModel($"Group1 exists elem", "Pop Group1", $"b[{iter}] = a[{g1Iter}];"));
            }

            CodeBlockPanel.AddAnimation(codeInfos["w3"], MainStoryboard.Offset);
            while (g2Iter <= g2EndIndex)
            {
                OperateGroup2(ref g2Iter, ref iter, g2EndIndex, new LogViewModel($"Group2 exists elem", "Pop Group2", $"b[{iter}] = a[{g2Iter}];"));
            }

            MergeElemsInTable(begIndex, iter);

            CodeBlockPanel.AddAnimation(codeInfos["copy"], MainStoryboard.Offset);
            ElemsReturn(begIndex, endIndex, new LogViewModel("Write-back merged sequence"));
            CodeBlockPanel.CodeBlockStoryboard.Offset = MainStoryboard.Offset;
        }

        private void OperateGroup1(ref int g1Iter, ref int iter, int g1EndIndex, LogViewModel log)
        {
            TmpStoreElem(g1Iter, iter, log);
            mergedIndices[iter++] = g1Iter++;
            if (g1Iter <= g1EndIndex) IterNext(Group1Iterator, g1Iter);
        }

        private void OperateGroup2(ref int g2Iter, ref int iter, int g2EndIndex, LogViewModel log)
        {
            TmpStoreElem(g2Iter, iter, log);
            mergedIndices[iter++] = g2Iter++;
            if (g2Iter <= g2EndIndex) IterNext(Group2Iterator, g2Iter);
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
            CodeBlockPanel.CodeBlockStoryboard.Delay(MainStoryboard.Offset);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        protected override void Init()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将迭代器跳到下一次要归并的两组元素头部
        /// </summary>
        /// <param name="g1BegIndex"></param>
        /// <param name="g2BegIndex"></param>
        private void ItersReturn(int g1BegIndex, int g2BegIndex, LogViewModel log)
        {
            var g1IterMove = GetIterMovementAnimation(Group1Iterator, g1BegIndex);
            var g2IterMove = GetIterMovementAnimation(Group2Iterator, g2BegIndex);

            MainStoryboard.AddAsyncAnimations(new List<SimulatedDoubleAnimation> { g1IterMove, g2IterMove }, log: log);
        }

        /// <summary>
        /// 分别标记当前正在归并的两组元素
        /// </summary>
        /// <param name="g1BegIndex"></param>
        /// <param name="g1EndIndex"></param>
        /// <param name="g2BegIndex"></param>
        /// <param name="g2EndIndex"></param>
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

        /// <summary>
        /// 一次归并完成后，更改对应的 table
        /// </summary>
        /// <param name="begIndex"></param>
        /// <param name="len"></param>
        private void MergeElemsInTable(int begIndex, int len)
        {
            int[] tmpRealIndices = new int[len];
            for (int i = 0; i < len; i++)
            {
                tmpRealIndices[i] = table[mergedIndices[i]];
            }

            for (int i = 0; i < len; i++)
            {
                table[begIndex + i] = tmpRealIndices[i];
            }
        }

        /// <summary>
        /// 归并时将元素存入临时数组中
        /// </summary>
        /// <param name="elemIndex"></param>
        /// <param name="toIndex"></param>
        private void TmpStoreElem(int elemIndex, int toIndex, LogViewModel log)
        {
            int elemRealIndex = table[elemIndex];

            var downMove = new SimulatedDoubleAnimation(by: 80, time: 500)
            {
                TargetControl = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, elemRealIndex).valueItem,
                TargetParam = AnimationHelper.VerticallyMoveParam
            };

            var animations = new List<SimulatedDoubleAnimation>
            {
                GetUnStickAnimation(elemIndex),
                downMove
            };

            MainStoryboard.AddAsyncAnimations(animations, () => { ActivateElem(elemRealIndex); }, log: log);
            MoveElem(elemIndex, toIndex, null, null, null, false);
        }

        /// <summary>
        /// 迭代器遍历下一个元素
        /// </summary>
        /// <param name="iter"></param>
        /// <param name="toIndex"></param>
        private void IterNext(UIElement iter, int toIndex)
        {
            int toRealIndex = table[toIndex];
            MoveIter(iter, toIndex);
        }

        /// <summary>
        /// 将排好序的元素返回到原数组中
        /// </summary>
        /// <param name="begIndex"></param>
        /// <param name="endIndex"></param>
        private void ElemsReturn(int begIndex, int endIndex, LogViewModel log)
        {
            var rightMove = new SimulatedDoubleAnimation(by: begIndex * (float)AnimationHelper.StepLen, time: 500);
            var leftMove = new SimulatedDoubleAnimation(by: -begIndex * (float)AnimationHelper.StepLen, time: 500)
            {
                TargetControl = TmpArray,
                TargetParam = AnimationHelper.HorizontallyMoveParam,
            };
            var upMove = new SimulatedDoubleAnimation(by: -80, time: 500);
            var stick = GetStickAnimations(begIndex, endIndex);

            var valueItems = new List<DependencyObject>();
            for (int i = begIndex; i <= endIndex; i++)
            {
                valueItems.Add(Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(Container, table[i]).valueItem);
            }

            var tmpArrayContainer = new List<DependencyObject> { TmpArray };
            tmpArrayContainer.AddRange(valueItems);

            MainStoryboard.AddAsyncAnimations(rightMove, tmpArrayContainer, AnimationHelper.HorizontallyMoveParam, () => { DeactivateElem(); }, log: log);
            MainStoryboard.AddAsyncAnimations(upMove, valueItems, AnimationHelper.VerticallyMoveParam);
            MainStoryboard.AddAsyncAnimations(stick);
            MainStoryboard.AddSyncAnimation(leftMove);
        }

        /// <summary>
        /// 根据所需长度生成临时数组
        /// </summary>
        /// <param name="len"></param>
        private void GenerateTmpArray(int len)
        {
            for (int i = 0; i < count; i++)
            {
                var item = Comm.GetItemFromItemsControlByIndex<Grid>(TmpArray, i);
                if (i < len) item.Visibility = Visibility.Visible; else item.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 隐藏临时数组
        /// </summary>
        private void HideTmpArray()
        {
            GenerateTmpArray(0);
        }
    }
}
