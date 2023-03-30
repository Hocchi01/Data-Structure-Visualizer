using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.Sorts
{
    internal class SelectionSort : SortBase
    {

        public Grid Iterator { get; set; }

        public SelectionSort(ObservableCollection<int> values, Grid canvas, ItemsControl container, Grid iterator, MyStoryboard myStoryboard, ObservableCollection<ArrayItemViewModel> dataItems) : base(values, canvas, container, myStoryboard, dataItems)
        {
            Iterator = iterator;
        }

        public override void MainProgram()
        {
            int count = vals.Count;

            Stick();

            bool isLess = false;
            IterNext(0, 0, 0, true);
            for (int i = 0; i < count - 1; i++, IterNext(i, count - 1, i, true))
            {
                int minIndex = i;
                for (int j = i + 1; j < count; j++)
                {
                    isLess = vals[j] < vals[minIndex];

                    IterNext(j, j - 1, minIndex, isLess);

                    if (isLess)
                    {
                        minIndex = j;
                    }
                }

                int k = i; // 若直接用 i，由于委托都是在“加载动画”后执行，则会使得最终 i 都为 count - 1
                Swap(i, minIndex, () => { DataItems[k].State = DataItemState.Sorted; });

                int tmp = vals[i];
                vals[i] = vals[minIndex];
                vals[minIndex] = tmp;
            }

            UnStick();

            Iterator.Visibility = Visibility.Visible;
            if (MainStoryboard != null)
            {
                MainStoryboard.Completed += (s, e) => 
                { 
                    Iterator.Visibility = Visibility.Hidden;
                    Iterator.RenderTransform = new TranslateTransform() { X = 8 }; // 复位迭代器
                    DataItems[count - 1].RecoverColor();
                };
                MainStoryboard.Begin_Ex(Canvas, true);
            }

        }
        

        private void IterNext(int curIndex, int preIndex, int minIndex, bool isLess)
        {
            if (curIndex >= DataItems.Count) return;

            MainStoryboard.AddSyncAnimation
            (
                new SimulatedDoubleAnimation
                (
                    to: 8 + curIndex * 52,
                    time: 500,
                    before: null,
                    after: () =>
                    {
                        DataItems[preIndex].RecoverColor();
                        DataItems[curIndex].Color = new SolidColorBrush(new PaletteHelper().GetTheme().SecondaryMid.Color);

                        if (isLess)
                        {
                            DataItems[minIndex].State = DataItemState.Normal;
                            DataItems[curIndex].State = DataItemState.Min;
                        }
                    }
                ),
                Iterator,
                "(UIElement.RenderTransform).(TranslateTransform.X)"
            );
        }

        private void ActionAfterSwap(int index)
        {
            DataItems[index].State = DataItemState.Sorted;
        }
        
    }
}
