using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
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

        public void ElemMove(int elemIndex, int toIndex)
        {
            MainStoryboard.AddSyncAnimation
            (
                new SimulatedDoubleAnimation(by: (toIndex - elemIndex) * 52, time: 500),
                Comm.GetItemFromItemsControlByIndex(Container, table[elemIndex]).FindName("valueItem") as UIElement,
                "(UIElement.RenderTransform).(TranslateTransform.X)"
            );
        }
    }
}
