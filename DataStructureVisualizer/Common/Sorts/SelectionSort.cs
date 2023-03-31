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
        private int last;
        private int minIndex = 0;
        // private int activeIndex = 0;
        public Grid Iterator { get; set; }

        public SelectionSort(ObservableCollection<int> values, Grid canvas, ItemsControl container, Grid iterator, MyStoryboard myStoryboard, ObservableCollection<ArrayItemViewModel> dataItems) : base(values, canvas, container, myStoryboard, dataItems)
        {
            Iterator = iterator;
            last = DataItems.Count - 1;
        }

        public override void MainProgram()
        {
            Stick();

            bool isLess = false;

            IterBegin();
            for (int i = 0; i < last; i++, IterReturn(i, minIndex == last ? i - 1 : last))
            {
                minIndex = i;
                for (int j = i + 1; j <= last; j++)
                {
                    isLess = DataItems[table[j]].Value < DataItems[table[minIndex]].Value;

                    IterNext(j, minIndex, isLess);

                    if (isLess)
                    {
                        minIndex = j;
                    }
                }

                int curI = i; // 若直接用 i，由于委托都是在“加载动画”后执行，则会使得最终 i 都为 count - 1
                int curMinIndex = minIndex;
                Swap(i, minIndex, () => 
                {
                    DataItems[curMinIndex].State = DataItemState.Normal;
                    DataItems[curI].State = DataItemState.Sorted;
                });
            }

            UnStick(new Action(IterEnd));
            MainStoryboard.Completed += (_, _) => { UpdateValues(); };
            MainStoryboard.Begin_Ex(Canvas, true);
        }
        

        private void IterNext(int toIndex, int minIndex, bool isLess)
        {
            if (toIndex >= DataItems.Count) return;

            int toRealIndex = table[toIndex];
            int preRealIndex = table[toIndex - 1];

            MainStoryboard.AddSyncAnimation
            (
                new SimulatedDoubleAnimation
                (
                    by: 52,
                    time: 500,
                    before: null,
                    after: () =>
                    {
                        DataItems[preRealIndex].RecoverColor();
                        DataItems[toRealIndex].Color = new SolidColorBrush(new PaletteHelper().GetTheme().SecondaryMid.Color);

                        if (isLess)
                        {
                            DataItems[minIndex].State = DataItemState.Normal;
                            DataItems[toIndex].State = DataItemState.Min;
                        }
                    }
                ),
                Iterator,
                "(UIElement.RenderTransform).(TranslateTransform.X)"
            );
        }

        private void IterReturn(int toIndex, int preIndex)
        {
            // if (toIndex == last) return;

            int toRealIndex = table[toIndex];
            int preRealIndex = table[preIndex];

            MainStoryboard.AddSyncAnimation
            (
                new SimulatedDoubleAnimation
                (
                    to: 8 + toIndex * 52,
                    time: 500,
                    before: null,
                    after: () =>
                    {
                        DataItems[preRealIndex].RecoverColor();
                        DataItems[toRealIndex].Color = new SolidColorBrush(new PaletteHelper().GetTheme().SecondaryMid.Color);
                        DataItems[toIndex].State = DataItemState.Min;
                    }
                ),
                Iterator,
                "(UIElement.RenderTransform).(TranslateTransform.X)"
            );
        }

        private void IterBegin()
        {
            Iterator.Visibility = Visibility.Visible;
            DataItems[0].Color = new SolidColorBrush(new PaletteHelper().GetTheme().SecondaryMid.Color);
            DataItems[0].State = DataItemState.Min;
        }

        private void IterEnd()
        {
            Iterator.Visibility = Visibility.Hidden;
            Iterator.RenderTransform = new TranslateTransform() { X = 8 }; // 复位迭代器
            DataItems[table[last]].RecoverColor();
            DataItems[last].State = DataItemState.Sorted;
        }
    }
}
