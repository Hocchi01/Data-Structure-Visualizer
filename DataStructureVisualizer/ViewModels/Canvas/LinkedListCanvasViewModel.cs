using CommunityToolkit.Mvvm.ComponentModel;
using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataStructureVisualizer.ViewModels.Canvas
{
    partial class LinkedListCanvasViewModel : CanvasViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<LinkedListItemViewModel> dataItems;

        public override void UpdateDataItems()
        {
            DataItems = new ObservableCollection<LinkedListItemViewModel>();

            List<Color> colors = Comm.GetColorGradientByValues(Values);

            DataItems.Add(new LinkedListItemViewModel() { Type = LinkedListItemType.Head });
            for (int i = 0; i < Values.Count; i++)
            {
                DataItems.Add(new LinkedListItemViewModel()
                {
                    Value = Values[i],
                    Index = i,
                    Color = new SolidColorBrush(colors[i]),
                });
            }
            DataItems.Add(new LinkedListItemViewModel() { Type = LinkedListItemType.Tail });
        }

        public override void Receive(LoadAddAnimationMessage message)
        {
            throw new NotImplementedException();
        }

        public LinkedListCanvasViewModel()
        {
            Type = DS_SecondaryType.LinkedList;
        }
    }
}
