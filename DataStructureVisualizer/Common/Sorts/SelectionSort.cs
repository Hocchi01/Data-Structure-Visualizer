using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
            int count = Values.Count;
            Stick(Container);

            for (int i = 0; i < count - 1; i++, IterNext(i, count - 1))
            {

                for (int j = i; j < count; j++, IterNext(j, j - 1))
                {
                    // 激活

                    

                }
            }

            UnStick(Container);

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

        private void IterNext(int curIndex, int preIndex)
        {
            if (curIndex >= Values.Count) return;

            var paletteHelper = new PaletteHelper();

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
                        DataItems[curIndex].Color = new SolidColorBrush(paletteHelper.GetTheme().SecondaryMid.Color);
                    }
                ),
                Iterator,
                "(UIElement.RenderTransform).(TranslateTransform.X)"
            ); 
        }
    }
}
