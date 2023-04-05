using DataStructureVisualizer.Common.AnimationLib;
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

        public void MoveElem(int elemIndex, int toIndex, Action? before, Action? after, bool isChangeTable = true)
        {
            var elem = Comm.GetItemFromItemsControlByIndex<SuccessiveItemUserControl>(Container, table[elemIndex]);

            MainStoryboard.AddSyncAnimation(elem.MoveValueItem(toIndex - elemIndex, before, after));

            if (isChangeTable) SwapElemsInTable(elemIndex, toIndex);
        }

        public void MoveElem(int elemIndex, int toIndex, bool isChangeTable = true)
        {
            int elemRealIndex = table[elemIndex];
            MoveElem(elemIndex, toIndex, () => { ActivateElem(elemRealIndex); }, null, isChangeTable);
        }

        //public void 

        public void SwapElems(int index1, int index2, Action? before = null, Action? after = null)
        {
            var elem1 = Comm.GetItemFromItemsControlByIndex<SuccessiveItemUserControl>(Container, table[index1]);
            var elem2 = Comm.GetItemFromItemsControlByIndex<SuccessiveItemUserControl>(Container, table[index2]);

            MainStoryboard.AddAsyncAnimations(new List<SimulatedDoubleAnimation> {
                elem1.MoveValueItem(index2 - index1),
                elem2.MoveValueItem(index1 - index2),
            }, before, after);

            SwapElemsInTable(index1, index2);
        }

        public void MoveIter(UIElement iter, int toIndex, Action? before = null, Action? after = null, double offset = 0)
        {
            double start = AnimationHelper.ArrayStart;
            double step = AnimationHelper.StepLen;
            string param = AnimationHelper.HorizontallyMoveParam;
            MainStoryboard.AddSyncAnimation(new SimulatedDoubleAnimation(to: start + step * toIndex + offset, time: 500, before: before, after: after) { TargetControl = iter, TargetParam = param });
        }




    }
}
