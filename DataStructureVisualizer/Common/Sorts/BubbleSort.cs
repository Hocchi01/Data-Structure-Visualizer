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
    internal class BubbleSort : SortBase
    {
        public Grid Iterator { get; set; }

        public BubbleSort(Grid canvas, ItemsControl container, Grid iterator, MyStoryboard myStoryboard, ObservableCollection<ArrayItemViewModel> dataItems) : base(canvas, container, myStoryboard, dataItems)
        {
            Iterator = iterator;
        }

        public override void MainProgram()
        {
            bool isGreater = false;

            Stick(new Action(IterBegin));

            for (int i = last; i >= 1; IterReturn(0, i), i--)
            {
                for (int j = 0; j < i; j++, IterNext(j, i))
                {
                    isGreater = DataItems[table[j]].Value > DataItems[table[j + 1]].Value;

                    if (isGreater)
                    {
                        int curJ = j;
                        Swap(j, j + 1, () =>
                        {
                            DataItems[curJ].State = DataItemState.Normal;
                            DataItems[curJ + 1].State = DataItemState.Max;
                        });
                    }
                }
            }

            UnStick(new Action(IterEnd));

            MainStoryboard.Completed += (_, _) => { UpdateValues(); };
            MainStoryboard.Begin_Ex(Canvas, true);
        }

        private void IterNext(int toIndex, int iterationEnd)
        {
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

                        // 无需条件判断，一定会执行的
                        DataItems[toIndex - 1].State = DataItemState.Normal;
                        DataItems[toIndex].State = toIndex == iterationEnd ? DataItemState.Sorted : DataItemState.Max;
                    }
                ),
                Iterator,
                "(UIElement.RenderTransform).(TranslateTransform.X)"
            );
        }

        private void IterReturn(int toIndex, int preIndex)
        {
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
                        DataItems[toIndex].State = DataItemState.Max;
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
            DataItems[0].State = DataItemState.Max;
        }

        private void IterEnd()
        {
            Iterator.Visibility = Visibility.Hidden;
            Iterator.RenderTransform = new TranslateTransform() { X = 8 }; // 复位迭代器
            DataItems[table[0]].RecoverColor();
            DataItems[0].State = DataItemState.Sorted;
        }
    }
}
