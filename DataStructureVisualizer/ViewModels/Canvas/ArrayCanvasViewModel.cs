using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
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
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Reflection;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Media.Animation;
using DataStructureVisualizer.Common.Sorts;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DataStructureVisualizer.ViewModels.Canvas
{
    internal partial class ArrayCanvasViewModel : CanvasViewModelBase, IRecipient<LoadSortAnimationMessage>
    {
        [ObservableProperty]
        private ObservableCollection<ArrayItemViewModel> dataItems;

        public override void UpdateDataItems()
        {
            DataItems = new ObservableCollection<ArrayItemViewModel>();

            List<Color> colors = Comm.GetColorGradientByValues(Values);
            for (int i = 0; i < Values.Count; i++)
            {
                DataItems.Add(new ArrayItemViewModel()
                {
                    Value = Values[i],
                    Index = i,
                    Color = new SolidColorBrush(colors[i]),
                    OriginalColor = new SolidColorBrush(colors[i]),
                });
            }
        }

        /// <summary>
        /// 响应【添加工具】的“执行添加动画”消息
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Receive(LoadAddAnimationMessage message)
        {
            // 若是尾部追加元素则不执行动画
            if (message.Index == DataItems.Count)
            {
                Values.Add(message.Value);
                return;
            }

            int count = DataItems.Count;
            Grid canvas = (Grid)GetCanvas();
            var container = canvas.FindName("arrItemsControl") as ItemsControl;
            var paletteHelper = new PaletteHelper();

            // TODO 02: 在数组末尾添加一个“空项”
            DataItems.Add(new ArrayItemViewModel() { Index = count });

            // TODO 03: 编写动画主体
            MainStoryboard = new MyStoryboard();
            for (int i = count - 1; i >= message.Index; i--)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(container, i);
                var oldColor = DataItems[i].Color;

                Action after = () => { itemView.rect.Fill = oldColor; };
                if (i == message.Index)
                {
                    after += () =>
                    {
                        Values.Insert(message.Index, message.Value);
                        WeakReferenceMessenger.Default.Send(new AddAnyLogMessage(new LogViewModel() { Content = $"a[{message.Index}] = {message.Value};" }));
                    };
                }

                MainStoryboard.AddSyncAnimation
                (
                    new SimulatedDoubleAnimation
                    (
                        to: 52, 
                        time: 500,
                        before: () => { itemView.rect.Fill = new SolidColorBrush(paletteHelper.GetTheme().SecondaryMid.Color); },
                        after: after,
                        log: new LogViewModel() { Content = $"a[{i+1}] = a[{i}];" }
                    ),
                    itemView.valueItem,
                    "(UIElement.RenderTransform).(TranslateTransform.X)"
                );
            }

            // TODO 05: 执行动画
            MainStoryboard.Begin_Ex(canvas, true);
        }

        /// <summary>
        /// 响应【排序工具】的“加载排序动画”消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(LoadSortAnimationMessage message)
        {
            MainStoryboard = new MyStoryboard();
            Grid canvas = (Grid)GetCanvas();
            var container = canvas.FindName("arrItemsControl") as ItemsControl;
            var iterator = canvas.FindName("iterator") as Grid;

            SortBase sort = null;

            switch (message.Type)
            {
                case SortType.SelectionSort:
                    sort = new SelectionSort(canvas, container, iterator, MainStoryboard, DataItems);
                    break;
                case SortType.QuickSort:
                    break;
                case SortType.BubbleSort:
                    sort = new BubbleSort(canvas, container, iterator, MainStoryboard, DataItems);
                    break;
                case SortType.InsertionSort:
                    sort = new InsertionSort(canvas, container, iterator, MainStoryboard, DataItems);
                    break;
            }

            sort?.MainProgram();
        }

        public override void Receive(LoadRemoveAnimationMessage message)
        {
            int index = message.Index;
            // 若是删除尾部元素则不执行动画
            if (index == DataItems.Count - 1)
            {
                Values.RemoveAt(index);
                return;
            }

            int count = DataItems.Count;
            Grid canvas = (Grid)GetCanvas();
            var container = canvas.FindName("arrItemsControl") as ItemsControl;
            var paletteHelper = new PaletteHelper();

            MainStoryboard = new MyStoryboard();

            // 删除元素
            DataItems[index].Clear();

            for (int i = index + 1; i < count; i++)
            {

            }
        }

        public ArrayCanvasViewModel()
        {
            Type = DS_SecondaryType.Array;
        }


    }

    class ArrayAnimationHelper
    {
        public ObservableCollection<ArrayItemViewModel> DataItems { get; set; }

    }


}
