using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class AlgorithmFactoryBase
    {
        protected int[] vals;
        protected int[] table; // 用于记录当前元素实际位置
        protected int last;

        public Grid Canvas { get; set; }
        public ItemsControl Container { get; set; }
        public MyStoryboard MainStoryboard { get; set; }
        public ObservableCollection<DataItemViewModelBase> DataItems { get; set; }

        public AlgorithmFactoryBase(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems)
        {
            Canvas = canvas;
            Container = container;
            MainStoryboard = myStoryboard;
            DataItems = dataItems;

            vals = new int[DataItems.Count];
            table = new int[DataItems.Count];
            for (int i = 0; i < DataItems.Count; i++)
            {
                vals[i] = DataItems[i].Value ?? 0;
                table[i] = i;
            }

            last = DataItems.Count - 1;
        }


    }
}
