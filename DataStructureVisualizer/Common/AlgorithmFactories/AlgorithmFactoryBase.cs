using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class AlgorithmFactoryBase
    {
        protected int[] table; // 用于记录当前元素实际位置
        protected int last;
        protected int count;
        protected int activeIndex = 0; // 维护一个当前唯一被激活的元素索引

        protected AnimationTimeline lastAnimation = null;

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

            
            table = new int[DataItems.Count];
            for (int i = 0; i < DataItems.Count; i++)
            {
                table[i] = i;
            }

            count = DataItems.Count;
            last = DataItems.Count - 1;

            MainStoryboard.Completed += (_, _) => { UpdateValues(); };
        }

        public void RemoveElem(int elemIndex)
        {
            int elemRealIndex = table[elemIndex];
            MainStoryboard.InsertAction(() =>
            {
                DataItems[elemRealIndex].Value = null; // 逻辑上删除
                //DataItems[elemRealIndex].Color = null;
                DataItems[elemRealIndex].IsRemoved = true;
            });
        }

        public void WriteElem(int elemIndex, int elemVal)
        {
            int elemRealIndex = table[elemIndex];
            MainStoryboard.InsertAction(() =>
            {
                DataItems[elemRealIndex].Value = elemVal;
                //DataItems[elemRealIndex].Color = new SolidColorBrush(new PaletteHelper().GetTheme().SecondaryMid.Color);
                // 由于【数组:添加动画】时并未移动“空项”的 valueItem，因此目前改变颜色会在末尾显示
            });
        }

        protected void SwapElemsInTable(int index1, int index2)
        {
            int tmp = table[index1];
            table[index1] = table[index2];
            table[index2] = tmp;
        }



        protected void UpdateValues()
        {
            int?[] values = new int?[DataItems.Count];
            for (int i = 0; i < DataItems.Count; i++)
            {
                values[i] = DataItems[table[i]].Value;
            }
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int?[]>(values));
        }

        protected void ActivateElem(int elemIndex)
        {
            if (activeIndex != elemIndex)
            {
                DataItems[activeIndex].Deactivate();
            }
            DataItems[elemIndex].Activate();

            activeIndex = elemIndex;
        }

        protected void DeactivateAllElems()
        {
            foreach (var item in DataItems)
            {
                item.Deactivate();
            }
        }
    }
}
