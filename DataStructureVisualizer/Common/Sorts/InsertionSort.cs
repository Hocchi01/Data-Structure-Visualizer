using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.Sorts
{
    internal class InsertionSort : SortBase
    {
        public Grid Iterator { get; set; }

        public InsertionSort(Grid canvas, ItemsControl container, Grid iterator, MyStoryboard myStoryboard, ObservableCollection<ArrayItemViewModel> dataItems) : base(canvas, container, myStoryboard, dataItems)
        {
            Iterator = iterator;
        }

        public override void MainProgram()
        {
            bool isLess = false;
            int j = 0;

            Stick(new Action(IterBegin));

            for (int i = 1; i <= last; i++, IterReturn(i, j))
            {
                for (j = i - 1; j >= 0; j--)
                {
                    isLess = DataItems[table[j + 1]].Value < DataItems[table[j]].Value;

                    IterNext(j);

                    if (isLess)
                    {
                        int curJ = j;
                        Swap(j, j + 1, () =>
                        {
                            DataItems[curJ].State = DataItemState.Min;
                            DataItems[curJ + 1].State = DataItemState.Sorted;
                        });
                    }
                    else
                    {
                        break;
                    }
                }
            }

            UnStick(() => { IterEnd(j + 1); });

            MainStoryboard.Completed += (_, _) => { UpdateValues(); };
            MainStoryboard.Begin_Ex(Canvas, true);
        }

        private void IterNext(int toIndex)
        {
            int toRealIndex = table[toIndex];
            int preRealIndex = table[toIndex + 1];

            MainStoryboard.AddSyncAnimation
            (
                new SimulatedDoubleAnimation
                (
                    by: -52,
                    time: 500
                ),
                Iterator,
                "(UIElement.RenderTransform).(TranslateTransform.X)"
            );
        }

        private void IterReturn(int toIndex, int preIndex)
        {
            if (toIndex > last) return;

            ++preIndex; // 指向刚插入的元素

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
                        DataItems[preIndex].State = DataItemState.Sorted;
                    }
                ),
                Iterator,
                "(UIElement.RenderTransform).(TranslateTransform.X)"
            );
        }

        private void IterBegin()
        {
            (Iterator.RenderTransform as TranslateTransform).X = 8 + 52; // 起始时指向 1 号位
            Iterator.Visibility = Visibility.Visible;
            DataItems[1].Color = new SolidColorBrush(new PaletteHelper().GetTheme().SecondaryMid.Color);
            DataItems[0].State = DataItemState.Sorted;
        }

        private void IterEnd(int endActiveIndex)
        {
            Iterator.Visibility = Visibility.Hidden;
            Iterator.RenderTransform = new TranslateTransform() { X = 8 }; // 复位迭代器
            DataItems[table[endActiveIndex]].RecoverColor();
            DataItems[endActiveIndex].State = DataItemState.Sorted;
        }
    }
}
