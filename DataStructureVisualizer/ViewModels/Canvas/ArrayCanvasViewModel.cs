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
using CommunityToolkit.Mvvm.ComponentModel;
using DataStructureVisualizer.Common.AlgorithmFactories;
using DataStructureVisualizer.Common.Extensions;
using DataStructureVisualizer.Test;
using System.ComponentModel;

namespace DataStructureVisualizer.ViewModels.Canvas
{
    internal partial class ArrayCanvasViewModel : CanvasViewModelBase, IRecipient<LoadSortAnimationMessage>
    {
        [ObservableProperty]
        private ObservableCollection<ArrayItemViewModel> dataItems;

        public override void UpdateDataItems(GenerateDataMessage? message)
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
        public async override void Receive(LoadAddAnimationMessage message)
        {
            int addIndex = message.Index;
            int addVal = message.Value;
            int count = DataItems.Count;

            // 若是尾部追加元素则不执行动画
            if (addIndex == count)
            {
                Values.Add(message.Value);
                return;
            }

            Grid canvas = (Grid)GetCanvas();
            var container = canvas.FindName("arrItemsControl") as ItemsControl;
            MainStoryboard = new MyStoryboard();
            var codeBlockPanel = CodeBlockPanelViewModel.Instance;
            var codeBlockPanelView = GetCodeBlockPanelView();
            codeBlockPanel.CodeBlockStoryboard = new MyStoryboard();

            // TODO 01: 在数组末尾添加一个“空项”
            DataItems.Add(new ArrayItemViewModel() { Index = count });

            // 等待前台尾部 UI 元素追加完毕
            await Task.Delay(100);
            var lastItem = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(container, DataItems.Count - 1);
            (lastItem.valueItem.RenderTransform as TranslateTransform).X += (addIndex - count) * AnimationHelper.StepLen;
            lastItem.valueItem.Opacity = 0;

            var aaf = new ArrayAlgorithmFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems.RevertElems());
            aaf.AddElem(addIndex, addVal);
        }

        /// <summary>
        /// 响应【排序工具】的“加载排序动画”消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(LoadSortAnimationMessage message)
        {
            MainStoryboard = new MyStoryboard();
            Grid canvas = (Grid)GetCanvas();
            var codeBlockPanelView = GetCodeBlockPanelView();
            var container = canvas.FindName("arrItemsControl") as ItemsControl;
            var codeBlockPanel = CodeBlockPanelViewModel.Instance;
            codeBlockPanel.CodeBlockStoryboard = new MyStoryboard();

            SortFactory sf = null;

            switch (message.Type)
            {
                case SortType.SelectionSort:
                    var selectionIterator = canvas.FindName("iterator") as Grid;
                    sf = new SelectionSortFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems.RevertElems()) { Iterator = selectionIterator };
                    break;

                case SortType.QuickSort:
                    var lowIterator = canvas.FindName("lowIterator") as Grid;
                    var highIterator = canvas.FindName("highIterator") as Grid;
                    var pivot = canvas.FindName("pivot") as Grid;
                    sf = new QuickSortFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems.RevertElems()) { LowIterator = lowIterator, HighIterator = highIterator, Pivot = pivot };
                    break;

                case SortType.BubbleSort:
                    var bubbleIterator = canvas.FindName("iterator") as Grid;
                    sf = new BubbleSortFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems.RevertElems()) { Iterator = bubbleIterator };
                    break;

                case SortType.InsertionSort:
                    var insertionIterator = canvas.FindName("iterator") as Grid;
                    sf = new InsertionSortFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems.RevertElems()) { Iterator = insertionIterator };
                    break;

                case SortType.MergeSort:
                    var group1Iterator = canvas.FindName("iterator") as Grid;
                    var group2Iterator = canvas.FindName("iterator2") as Grid;
                    var tmpArray = canvas.FindName("tmpArray") as ItemsControl;
                    sf = new MergeSortFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems.RevertElems()) { Group1Iterator = group1Iterator, Group2Iterator = group2Iterator, TmpArray = tmpArray };
                    break;
            }

            sf?.Execute();
        }

        /// <summary>
        /// 响应【删除工具】的“加载删除动画”消息
        /// </summary>
        /// <param name="message"></param>
        public override void Receive(LoadRemoveAnimationMessage message)
        {
            int rmvIndex = message.Index;
            int count = DataItems.Count;

            // 若是删除尾部元素则不执行动画
            if (rmvIndex == count - 1)
            {
                Values.RemoveAt(rmvIndex);
                return;
            }

            Grid canvas = (Grid)GetCanvas();
            var container = canvas.FindName("arrItemsControl") as ItemsControl;
            MainStoryboard = new MyStoryboard();
            var codeBlockPanel = CodeBlockPanelViewModel.Instance;
            var codeBlockPanelView = GetCodeBlockPanelView();
            codeBlockPanel.CodeBlockStoryboard = new MyStoryboard();

            var aaf = new ArrayAlgorithmFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems.RevertElems());
            aaf.RemoveElemAlgorithm(rmvIndex);

            var saf = new SuccessiveAlgorithmFactory(canvas, container, MainStoryboard, DataItems.RevertElems());
        }

        public ArrayCanvasViewModel()
        {
            Type = DS_SecondaryType.Array;
        }


    }
}
