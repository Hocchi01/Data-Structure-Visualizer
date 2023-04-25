using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels;
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
    internal class SuccessiveAlgorithmFactory : AlgorithmFactoryBase
    {
        public SuccessiveAlgorithmFactory(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, container, myStoryboard, dataItems)
        {
        }

        protected SimulatedDoubleAnimation GetElemMovementAnimation(int elemIndex, float by, Action? before, Action? after, LogViewModel? log, bool noActivate = false)
        {
            int elemRealIndex = table[elemIndex];

            Action beforeActions = () => { if (!noActivate) ActivateElem(elemRealIndex); }; // 默认前置动作：激活当前移动的元素
            Action afterActions = () => { };
            if (before != null) beforeActions += before;
            if (after != null) afterActions += after;

            var elem = Comm.GetItemFromItemsControlByIndex<SuccessiveItemUserControl>(Container, elemRealIndex);

            return elem.MoveValueItem(by, beforeActions, afterActions, log);
        }

        protected SimulatedDoubleAnimation GetElemMovementAnimation(int elemIndex, int toIndex, Action? before, Action? after, LogViewModel? log, bool noActivate = false)
        {
            return GetElemMovementAnimation(elemIndex, (toIndex - elemIndex) * (float)AnimationHelper.StepLen, before, after, log, noActivate);
        }

        /// <summary>
        /// 容器内部移动元素
        /// </summary>
        /// <param name="elemIndex"></param>
        /// <param name="toIndex"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="isChangeTable"></param>
        public void MoveElem(int elemIndex, int toIndex, Action? before = null, Action? after = null, LogViewModel? log = null, bool isChangeTable = true)
        {
            if (elemIndex == toIndex) return;

            MainStoryboard.AddSyncAnimation(GetElemMovementAnimation(elemIndex, toIndex, before, after, log));

            if (isChangeTable) SwapElemsInTable(elemIndex, toIndex);
        }

        /// <summary>
        /// 非容器内部移动元素
        /// </summary>
        /// <param name="elemIndex"></param>
        /// <param name="by"></param>
        public void MoveElem(int elemIndex, float by, Action? before = null, Action? after = null, LogViewModel? log = null)
        {
            MainStoryboard.AddSyncAnimation(GetElemMovementAnimation(elemIndex, by, before, after, log));
        }

        /// <summary>
        /// 交换两个元素
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        public void SwapElems(int index1, int index2, Action? before = null, Action? after = null, LogViewModel? log = null)
        {
            var elem1Move = GetElemMovementAnimation(index1, index2, null, null, null, true);
            var elem2Move = GetElemMovementAnimation(index2, index1, null, null, null, true);
            MainStoryboard.AddAsyncAnimations(new List<SimulatedDoubleAnimation> { elem1Move, elem2Move }, before, after, log);

            SwapElemsInTable(index1, index2);
        }



        protected SimulatedDoubleAnimation GetIterMovementAnimation(UIElement iter, int toIndex, Action? before = null, Action? after = null, double offset = 0, LogViewModel? log = null)
        {
            double start = AnimationHelper.ArrayStart;
            double step = AnimationHelper.StepLen;
            string param = AnimationHelper.HorizontallyMoveParam;

            return new SimulatedDoubleAnimation(to: start + step * toIndex + offset, time: 500, before: before, after: after) { TargetControl = iter, TargetParam = param, Log = log };
        }

        /// <summary>
        /// 移动迭代器
        /// </summary>
        /// <param name="iter"></param>
        /// <param name="toIndex"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="offset"></param>
        public void MoveIter(UIElement iter, int toIndex, Action? before = null, Action? after = null, double offset = 0, LogViewModel? log = null)
        {
            MainStoryboard.AddSyncAnimation(GetIterMovementAnimation(iter, toIndex, before, after, offset, log));
        }

    }
}
